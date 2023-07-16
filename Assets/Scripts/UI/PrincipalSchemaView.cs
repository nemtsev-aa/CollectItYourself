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

    public void Init(Stopwatch stopwatch) {
        _timeView.Init(stopwatch);
    }

    public void Show(string name, Sprite sprite) {
        _name.text = name;
        _image.sprite = sprite;
    }
}
