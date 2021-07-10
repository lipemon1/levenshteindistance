using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "String Importer", menuName = "ScriptableObjects/New String Importer", order = 1)]
public class DuplicatedPhrases : ScriptableObject
{
    public List<string> firstList = new List<string>();

    public DuplicateConfiguration Config;

    [Space]
    public DuplicatesResult DuplicatesResult;

    [Space]
    public DuplicateListsResults DuplicateListsResult;

    private static DuplicatedPhrases _instance;
    public static DuplicatedPhrases Instance
    {
        get
        {
            if(_instance == null)
                _instance = Resources.Load<DuplicatedPhrases>("String Importer");

            return _instance;
        }
    }

    [ContextMenu("Compare Now")]
    [ExecuteInEditMode]
    public void CompareNow()
    {
        if (firstList.Count <= 0) return;

        DuplicatesResult = new DuplicatesResult();
        DuplicatesResult.GenerateCases(firstList.Distinct().ToList(), Config);

        DuplicateListsResult = SeparateStrings(firstList);
    }

    public DuplicateListsResults SeparateStrings(List<string> stringToSeparate)
    {
        if (stringToSeparate.Count <= 0) return new DuplicateListsResults();

        DuplicatesResult duplicatesResult = new DuplicatesResult();
        duplicatesResult.GenerateCases(stringToSeparate.Distinct().ToList(), Config);

        List<string> DuplicateList = duplicatesResult.GetPossiblesDuplicates();
        List<string> NotDuplicateList = stringToSeparate.Distinct().Except(DuplicateList).ToList();

        DuplicateListsResult = new DuplicateListsResults(NotDuplicateList, DuplicateList);

        return DuplicateListsResult;
    }
}

[System.Serializable]
public struct DuplicatesResult
{
    public Dictionary<string, List<string>> CasesToCompareDic;

    public List<DuplicatePhraseResult> NotDuplicates;
    public List<DuplicatePhraseResult> PossibleDuplicates;

    [Space]
    List<PhraseResult> comparisonsMade;

    public void GenerateCases(List<string> stringsToCompare, DuplicateConfiguration compareConfig)
    {
        CasesToCompareDic = new Dictionary<string, List<string>>();
        comparisonsMade = new List<PhraseResult>();

        NotDuplicates = new List<DuplicatePhraseResult>();
        PossibleDuplicates = new List<DuplicatePhraseResult>();

        for (int i = 0; i < stringsToCompare.Count; i++)
        {
            string curItem = stringsToCompare[i];
            foreach (string newItemToCompare in GetListWithoutItem(stringsToCompare, curItem))
            {
                List<string> listToCompareNow = AddNewCompareCase(curItem, newItemToCompare);

                if (listToCompareNow.Count <= 1) continue;

                PhraseResult phraseResult = new PhraseResult(curItem, newItemToCompare, compareConfig);
                comparisonsMade.Add(phraseResult);

                if (!phraseResult.HasDuplicateResult.Result)
                {
                    NotDuplicates.Add(new DuplicatePhraseResult()
                    {
                        Origin = phraseResult.Original,
                        Target = phraseResult.Target,
                        ConfigResult = phraseResult.ConfigResult,
                        Results = phraseResult.HasDuplicateResult
                    });
                }
                else
                {
                    PossibleDuplicates.Add(new DuplicatePhraseResult()
                    {
                        Origin = phraseResult.Original,
                        Target = phraseResult.Target,
                        ConfigResult = phraseResult.ConfigResult,
                        Results = phraseResult.HasDuplicateResult
                    });
                }
            }
        }

        NotDuplicates = NotDuplicates.Distinct().ToList();
        PossibleDuplicates = PossibleDuplicates.Distinct().ToList();
    }

    private List<string> GetListWithoutItem(List<string> originalList, string itemToRemove)
    {
        List<string> newList = new List<string>();
        foreach (var item in originalList)
        {
            newList.Add(item);
        }

        newList.Distinct();
        newList.Remove(itemToRemove);
        return newList;
    }

    private List<string> AddNewCompareCase(string target, string newCase)
    {
        string keyValue = GetKeyValue(target, newCase);

        List<string> casesFromThisTarget;
        if(CasesToCompareDic.TryGetValue(keyValue, out casesFromThisTarget))
        {
            casesFromThisTarget.Add(newCase);
            CasesToCompareDic.Remove(keyValue);
            CasesToCompareDic.Add(keyValue, casesFromThisTarget);

            return casesFromThisTarget;
        }
        else
        {
            List<string> newList = new List<string>();
            newList.Add(newCase);
            CasesToCompareDic.Add(keyValue, newList);

            return CasesToCompareDic[keyValue];
        }
    }

    private string GetKeyValue(string target, string newCase)
    {
        List<string> keyValueList = new List<string>();
        keyValueList.Add(target);
        keyValueList.Add(newCase);

        keyValueList = keyValueList.OrderBy(kv => kv).ToList();

        string keyValue = "";

        foreach (string item in keyValueList)
        {
            keyValue += item;
        }

        return keyValue;
    }

    public List<string> GetPossiblesDuplicates()
    {
        List<string> possibleDuplicates = new List<string>();
        foreach (DuplicatePhraseResult possibleDuplicate in PossibleDuplicates)
        {
            possibleDuplicates.Add(possibleDuplicate.Origin);
            possibleDuplicates.Add(possibleDuplicate.Target);
        }

        possibleDuplicates = possibleDuplicates.Distinct().ToList();

        return possibleDuplicates;
    }
}

[System.Serializable]
public struct DuplicateListsResults
{
    public List<string> NotDuplicateList;
    public List<string> DuplicateList;

    public DuplicateListsResults(List<string> NotDuplicates, List<string> DuplicateList)
    {
        this.NotDuplicateList = NotDuplicates;
        this.DuplicateList = DuplicateList;
    }
}