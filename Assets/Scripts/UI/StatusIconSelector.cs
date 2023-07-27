using UnityEngine;
using UnityEngine.UI;

public class StatusIconSelector : MonoBehaviour {
    [SerializeField] private Image _statusImage;
    [SerializeField] private Sprite _lockImage;
    [SerializeField] private Sprite _unlockImage;
    [SerializeField] private Sprite _compliteImage;

    public void SetStatus(TaskStatus status) {
        switch (status) {
            case TaskStatus.Lock:
                _statusImage.sprite = _lockImage;
                break;
            case TaskStatus.Unlock:
                _statusImage.sprite = _unlockImage;
                break;
            case TaskStatus.Complite:
                _statusImage.sprite = _compliteImage;
                break;
            default:
                break;
        }
    }
}
