using UnityEngine;

public class LampState : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _status;

    public void SetState(bool status) {
        _status = status;
        
        if (_status) _animator.SetTrigger("Show");
        else _animator.SetTrigger("Hide");
    }
}
