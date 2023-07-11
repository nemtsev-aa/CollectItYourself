using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrincipalSchemaView : MonoBehaviour
{
    [Tooltip("Надпись - название задания")]
    [SerializeField] private TextMeshProUGUI _name;
    [Tooltip("Изображение - схема")]
    [SerializeField] private Image _image;
    [Tooltip("Панель секундомера")]
    [SerializeField] private TimeView _timeView;

    public void Show(SwitchBox switchBox) {
        _name.text = switchBox.TaskName;
        //_image.sprite = switchBox.SwitchBoxData.Task.TaskDataList[switchBox.SwitchBoxData.PartNumber-1].PrincipalShema;
    }
}
