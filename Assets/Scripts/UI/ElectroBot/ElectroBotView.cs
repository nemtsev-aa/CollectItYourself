using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ElectroBotStatus {
    Idle,
    Crash
}

public class ElectroBotView : MonoBehaviour {
    public ElectroBotStatus ElectroBotStatus => _currentElectroBotStatus;
    private ElectroBotStatus _currentElectroBotStatus;
    
    [Tooltip("Аниматор")]
    [SerializeField] private Animator _animator;

    public void SetStatus(ElectroBotStatus electroBotStatus) {
        _currentElectroBotStatus = electroBotStatus;
        switch (_currentElectroBotStatus) {
            case ElectroBotStatus.Idle:
                _animator.SetTrigger("Idle");
                break;
            case ElectroBotStatus.Crash:
                _animator.ResetTrigger("Idle");
                break;
            default:
                break;
        }
    }
}
