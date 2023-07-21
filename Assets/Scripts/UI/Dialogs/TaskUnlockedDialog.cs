using System;
using UI;
using UnityEngine;

public class TaskUnlockedDialog : Dialog {
    public TaskUnlockedView TaskUnlockedView { get { return _taskUnlockedView; } private set { } }
    public Action OnTaskUnlocked;

    [SerializeField] private TaskUnlockedView _taskUnlockedView;
}
