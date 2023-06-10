using UnityEngine;

public class LampState : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _status;

    public void SetState() {
        if (_status) {
            _status = false;
            _animator.SetTrigger("Hide");
        }
        else {
            _status = true;
            _animator.SetTrigger("Show");
        }
    }
}
