using UnityEngine;

public enum MotionSensorStateType {
    Hide,
    Show,
    Blink
}

public class MotionSensorState : MonoBehaviour {

    [SerializeField] private Animator _animator;
    private bool _status;
    private MotionSensorStateType _currentState = MotionSensorStateType.Hide;
    
    private void Start() {
        SetState(_currentState);
    }

    public void SetState(MotionSensorStateType type) {
        _currentState = type;
        switch (type) {
            case MotionSensorStateType.Hide:
                _animator.SetTrigger("Hide");
                _status = false;
                break;
            case MotionSensorStateType.Show:
                _animator.SetTrigger("Show");
                _status = true;
                break;
            case MotionSensorStateType.Blink:
                _animator.SetTrigger("Blink");
                _status = true;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        _currentState = MotionSensorStateType.Blink;
        SetState(_currentState);
    }
}
