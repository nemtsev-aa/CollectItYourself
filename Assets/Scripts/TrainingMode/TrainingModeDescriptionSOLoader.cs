using System.Collections.Generic;
using UnityEngine;

public class TrainingModeDescriptionSOLoader : MonoBehaviour, IService {
    public IEnumerable<TrainingModeDescription> Descriptions => _descriptions;
    [SerializeField] private List<TrainingModeDescription> _descriptions;

    public Description GetTrainingModeDescription(TrainingModeType type) {
        return _descriptions.FindLast(t => t.CurrentType == type);
    }
}
