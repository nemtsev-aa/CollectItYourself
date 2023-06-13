using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveWindow : MonoBehaviour
{
    [SerializeField] private Management Management;
    [SerializeField] private TextMeshProUGUI _taskName;
    [SerializeField] private Image _principalShemaImage;

    public void Show()
    {
        gameObject.SetActive(true);
        Management.ShowTask();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowTask(Task task) {
        _taskName.text = task.Name;
        _principalShemaImage.sprite = task.TaskData[0].PrincipalShemas;
    }

    public void HideTask(Task task) {
        _taskName.text = "";
        _principalShemaImage.sprite = null;
    }
}
