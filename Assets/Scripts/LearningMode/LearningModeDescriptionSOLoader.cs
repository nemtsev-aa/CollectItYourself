using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningModeDescriptionSOLoader : MonoBehaviour, IService {
    public IEnumerable<LearningModeDescription> Descriptions => _descriptions;
    [SerializeField] private List<LearningModeDescription> _descriptions;

    public Description GetLearningModeDescription(LearningModeType type) {
        return _descriptions.FindLast(t => t.CurrentType == type);
    }
}
