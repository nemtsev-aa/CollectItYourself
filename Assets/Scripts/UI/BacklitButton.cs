using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BacklitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Button Button;
    public bool Status;

    [SerializeField] private Image _outline;

    public void OnPointerEnter(PointerEventData eventData) {
        ShowOutline();
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!Status) {
            HideOutline();
        }
    }

    public void ShowOutline() {
        _outline.gameObject.SetActive(true);
    }

    public void HideOutline() {
        _outline.gameObject.SetActive(false);
    }
}
