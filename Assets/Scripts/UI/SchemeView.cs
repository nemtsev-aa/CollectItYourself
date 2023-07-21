using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SchemeView : MonoBehaviour {
    public Image Image => _schemeImage;

    [SerializeField] private BacklitButton _zoomButton;
    [SerializeField] private Image _schemeImage;

    private bool _isZoom;
    private Vector3 _defaultSize;

    public void Init(Sprite sprite) {
        _zoomButton.Button.onClick.AddListener(ShowZoom);
        _defaultSize = _schemeImage.transform.localScale;
        _schemeImage.sprite = sprite;
    }

    public void ShowZoom () {
        if (_isZoom) {
            _isZoom = false;
            _schemeImage.transform.parent.transform.DOScale(_defaultSize, 0.5f);
        } else {
            _isZoom = true;
            _schemeImage.transform.parent.transform.DOScale(_defaultSize * 2f, 0.5f);
        }
    }
}
