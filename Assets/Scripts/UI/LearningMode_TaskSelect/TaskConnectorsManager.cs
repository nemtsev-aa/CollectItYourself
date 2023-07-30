using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class TaskConnectorsManager : MonoBehaviour {
    [SerializeField] private List<TaskConnect> _taskConnects;
    [SerializeField] private TaskConnect _taskConnectPrefab;

    public void CreateConnect(TaskVariantCard startCard, TaskVariantCard nextCards) {
        RectTransform startPointTransform = new RectTransform();
        RectTransform nextRectTransform = new RectTransform();
        
        if (startCard.TaskData.Type != nextCards.TaskData.Type) { // Следующая карта на высшем уровне
            startPointTransform = startCard.Points[0];      // Up
            nextRectTransform = nextCards.Points[1];        // Down
        } else { // Задания на одном уровне
            startPointTransform = startCard.Points[3];      // Left
            nextRectTransform = nextCards.Points[2];        // Right
        }

        TaskConnect newConnect = Instantiate(_taskConnectPrefab, transform);
        RectTransform[] newPoints = new RectTransform[2] { startPointTransform, nextRectTransform };
        newConnect.GetComponent<UILineConnector>().transforms = newPoints;
        _taskConnects.Add(newConnect);
    }

    public void Reset() {
        foreach (var item in _taskConnects) {
            Destroy(item.gameObject);
        }
        _taskConnects.Clear();
    }
}
