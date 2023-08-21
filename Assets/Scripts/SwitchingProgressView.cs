using TMPro;
using UnityEngine;

public class SwitchingProgressView : MonoBehaviour {
    [SerializeField] private RectTransform _filledImage;
    [SerializeField] private float _defaultWidth;
    [SerializeField] private TextMeshProUGUI _progressValueText;

    private int _pastProgress;

    private void OnValidate() {
        _defaultWidth = _filledImage.sizeDelta.x;
    }

    public void UpdateProgress(float max, int current) {
        float percent = current / max;
        _filledImage.sizeDelta = new Vector2(_defaultWidth * percent, _filledImage.sizeDelta.y);

        if (_progressValueText != null) _progressValueText.text = $"{current}/{max}";
    }
}
