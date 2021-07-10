using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamerauLevenshteinTestBehavior : MonoBehaviour
{
    public DuplicatedPhrases DuplicateScriptable;



    [ContextMenu("Compare Now")]
    public void CompareNow()
    {
        DuplicateScriptable.CompareNow();
    }
}