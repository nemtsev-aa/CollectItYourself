using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BacklitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Button Button;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _outline;

    public void OnPointerEnter(PointerEventData eventData) {
        ShowOutline();
    }

    public void OnPointerExit(PointerEventData eventData) {
        HideOutline();
    }

    public void ShowOutline() {
        _outline.gameObject.SetActive(true);
    }

    public void HideOutline() {
        _outline.gameObject.SetActive(false);
    }
}
