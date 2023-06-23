using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WagoTypeSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    [SerializeField] private WagoCreator _wagoCreator;
    [SerializeField] private WagoClipData _wagoClipData;
    [SerializeField] private Image _wagoView;

    //private Button _button;

    private void Start() {
        //_button = GetComponent<Button>();
        //_button.onClick.AddListener();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _wagoView.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData) {
        _wagoCreator.CreateWago(_wagoClipData);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _wagoView.gameObject.SetActive(false);
    }
}
