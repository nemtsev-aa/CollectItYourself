using UnityEngine;

public enum MotionSensorStateType {
    Hide,
    Show,
    Blink
}

public class MotionSensorState : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _status;
    private MotionSensorStateType _currentState = MotionSensorStateType.Hide;

    public void SetState() {
        if (!_status) {
            if (_currentState == MotionSensorStateType.Hide) {
                _animator.SetTrigger("Show");
                _currentState = MotionSensorStateType.Show;
                _status = false;
            } else if (_currentState == MotionSensorStateType.Show) {
                _animator.SetTrigger("Blink");
                _currentState = MotionSensorStateType.Blink;
                _status = true;
            }
        } else {
            _currentState = MotionSensorStateType.Hide;
            _animator.SetTrigger("Hide");
            _status = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Motion");
        _currentState = MotionSensorStateType.Blink;
    }
}
