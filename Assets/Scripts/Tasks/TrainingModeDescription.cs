using System.Collections.Generic;
using UnityEngine;

public enum TrainingModeType {
    RepeatAfterCoach,
    BuildByVariants,
    TroubleFinding
}


[CreateAssetMenu(fileName = nameof(TrainingModeDescription), menuName = "TrainingMode/" + nameof(TrainingModeDescription))]
[System.Serializable]
public class TrainingModeDescription : Description {
    public TrainingModeType CurrentType;
}

