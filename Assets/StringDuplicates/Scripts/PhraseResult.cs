using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StringDuplicate
{
    [System.Serializable]
    public struct PhraseResult
    {
        public string Original;
        public string Target;
        public List<ComponentWordItem> Components;
        public ConfigResult ConfigResult;

        [Space]
        public DuplicateConfiguration ConfigToCompare;
        public ResultDecision HasDuplicateResult;

        public PhraseResult(string original, string target, DuplicateConfiguration configToCompare)
        {
            Original = original;
            Target = target;
            Components = new List<ComponentWordItem>();
            ConfigResult = new ConfigResult();
            ConfigToCompare = configToCompare;
            HasDuplicateResult = new ResultDecision();

            Components = CreateAndCalculateComponents(Original, Target);

            ConfigResult.SimpleDistance = Original.DamerauLevenshteinDistanceTo(Target);
            ConfigResult.SumDistance = Components.Select(c => c.Distance).Sum();
            ConfigResult.AverageDistance = Components.Select(c => c.Distance).Average();
            ConfigResult.MaxDistance = Components.Select(c => c.Distance).Max();
            ConfigResult.MinDistance = Components.Select(c => c.Distance).Min();
            ConfigResult.AmountOfDifferences = Components.Count(c => c.Distance > 0);
            ConfigResult.DifferencesPerComponents = (double)ConfigResult.AmountOfDifferences / Components?.Count ?? 0;

            HasDuplicateResult = GenerateResult(configToCompare);
        }

        private List<ComponentWordItem> CreateAndCalculateComponents(string Original, string Target)
        {
            List<string> originalSplited = Original.Split(' ').ToList();
            List<string> targetSplited = Target.Split(' ').ToList();

            bool firstIsSmaller = originalSplited.Count < targetSplited.Count;
            int sizeCompare = originalSplited.Count < targetSplited.Count ? originalSplited.Count : targetSplited.Count;

            List<ComponentWordItem> compareList = new List<ComponentWordItem>();

            for (int i = 0; i < sizeCompare; i++)
            {
                ComponentWordItem compareItem = new ComponentWordItem(originalSplited[i], targetSplited[i]);
                compareList.Add(compareItem);
            }

            return compareList;
        }

        private ResultDecision GenerateResult(DuplicateConfiguration configuration)
        {
            ResultDecision resultDecision = new ResultDecision();
            resultDecision.Reasons = new List<string>();

            if (ConfigResult.AmountOfDifferences <= configuration.maxOneWord)
            {
                if (ConfigResult.SimpleDistance >= configuration.Min_SimpleDistanceForOneWord &&
                    ConfigResult.SimpleDistance <= configuration.Max_SimpleDistanceForOneWord)
                {
                    resultDecision.Reasons.Add("One word with differences");
                }
            }
            else if (ConfigResult.AmountOfDifferences > configuration.maxOneWord &&
                    ConfigResult.AmountOfDifferences <= configuration.maxFewWords)
            {
                if (ConfigResult.SimpleDistance >= configuration.Min_SimpleDistanceForOneWord &&
                    ConfigResult.SimpleDistance <= configuration.Max_SimpleDistanceForOneWord)
                {
                    resultDecision.Reasons.Add("A few words with differences");
                }
            }
            else
            {
                if (ConfigResult.SimpleDistance >= configuration.Min_SimpleDistance &&
                    ConfigResult.SimpleDistance <= configuration.Max_SimpleDistance)
                {
                    resultDecision.Reasons.Add("Simple Distance");

                    if (ConfigResult.AmountOfDifferences >= configuration.Min_AmountOfDifferences &&
                        ConfigResult.AmountOfDifferences <= configuration.Max_AmountOfDifferences)
                    {
                        resultDecision.Reasons.Add("Amount of Differences");

                        if (ConfigResult.SumDistance >= configuration.Min_SumDistance &&
                            ConfigResult.SumDistance <= configuration.Max_SumDistance)
                        {
                            resultDecision.Reasons.Add("Sum Distance");
                        }

                        if (ConfigResult.AverageDistance >= configuration.Min_AverageDistance &&
                            ConfigResult.AverageDistance <= configuration.Max_AverageDistance)
                        {
                            resultDecision.Reasons.Add("Average Distance");
                        }

                        if (ConfigResult.MaxDistance >= configuration.Min_MaxDistance &&
                            ConfigResult.MaxDistance <= configuration.Max_MaxDistance)
                        {
                            resultDecision.Reasons.Add("Max Distance");
                        }

                        if (ConfigResult.MinDistance >= configuration.Min_MinDistance &&
                            ConfigResult.MinDistance <= configuration.Max_MinDistance)
                        {
                            resultDecision.Reasons.Add("Min Distance");
                        }

                        if (ConfigResult.DifferencesPerComponents >= configuration.Min_DifferencePerComponent &&
                            ConfigResult.DifferencesPerComponents <= configuration.Max_DifferencePerComponent)
                        {
                            resultDecision.Reasons.Add("Differences per Component");
                        }
                    }
                }
            }

            resultDecision.Result = resultDecision.Reasons.Count > 0;

            return resultDecision;
        }
    }

    [System.Serializable]
    public struct ResultDecision
    {
        public bool Result;
        public List<string> Reasons;
    }

    [System.Serializable]
    public struct DuplicateConfiguration
    {
        [Space]
        public int maxOneWord;
        public int maxFewWords;

        public int Min_SimpleDistance;
        public int Max_SimpleDistance;

        [Space]
        public int Min_SimpleDistanceForOneWord;
        public int Max_SimpleDistanceForOneWord;

        [Space]
        public int Min_SimpleDistanceForFewWord;
        public int Max_SimpleDistanceForFewWord;

        [Space]
        public int Min_SumDistance;
        public int Max_SumDistance;

        [Space]
        public double Min_AverageDistance;
        public double Max_AverageDistance;

        [Space]
        public int Min_MaxDistance;
        public int Max_MaxDistance;

        [Space]
        public int Min_MinDistance;
        public int Max_MinDistance;

        [Space]
        public int Min_AmountOfDifferences;
        public int Max_AmountOfDifferences;

        [Space]
        public double Min_DifferencePerComponent;
        public double Max_DifferencePerComponent;
    }

    [System.Serializable]
    public struct DuplicatePhraseResult
    {
        public string Origin;
        public string Target;
        public ConfigResult ConfigResult;
        public ResultDecision Results;
    }

    [System.Serializable]
    public struct ConfigResult
    {
        public int SimpleDistance;
        public int SumDistance;
        public double AverageDistance;
        public int MaxDistance;
        public int MinDistance;
        public int AmountOfDifferences;
        public double DifferencesPerComponents;
    }
}