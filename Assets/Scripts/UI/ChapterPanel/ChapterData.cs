using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ChapterData), menuName = "LearningMode/" + nameof(ChapterData))]
public class ChapterData : ScriptableObject {
    [field: SerializeField] public string ChepterTitle { get; private set; }
    [field: SerializeField] public Sprite ChepterIcon { get; private set; }
    [field: SerializeField] public int ProgressValue { get; private set; }
    [field: SerializeField] public string ChepterDescription { get; private set; }
    [field: SerializeField] public int ExpAmountToComplete { get; private set; }
    [field: SerializeField] public int ExpAmountToUnlock { get; private set; }
    
    [field: SerializeField] public int CurrentExpAmount;
}
