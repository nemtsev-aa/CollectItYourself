using UI;
using UnityEngine;

public class TaskDescriptionDialog : Dialog {
    public TrainingTaskDescriptionView TrainingTaskDescriptionView { get { return _trainingTaskDescriptionView; } private set { } } 
    [SerializeField] private TrainingTaskDescriptionView _trainingTaskDescriptionView;
    
    public override void Awake() {
        base.Awake();
    }
}
