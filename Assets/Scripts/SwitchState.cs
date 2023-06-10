using UnityEngine;

public class SwitchState : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _status;

    public void SetState() {
        if (_status) {
            _status = false;
            _animator.SetTrigger("Blue");
        } else {
            _status = true;
            _animator.SetTrigger("Yellow");
        } 
    }
}
