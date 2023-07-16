using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrincipalSchemaView : MonoBehaviour
{
    [Tooltip("������� - �������� �������")]
    [SerializeField] private TextMeshProUGUI _name;
    [Tooltip("����������� - �����")]
    [SerializeField] private Image _image;
    [Tooltip("������ �����������")]
    [SerializeField] private TimeView _timeView;

    public void Init(Stopwatch stopwatch) {
        _timeView.Init(stopwatch);
    }

    public void Show(string name, Sprite sprite) {
        _name.text = name;
        _image.sprite = sprite;
    }
}
