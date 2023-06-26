using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveElectricDirectionManager : MonoBehaviour
{
    public bool Status;

    [SerializeField] private List<MoveElectricDirection> moveElectricDirections = new List<MoveElectricDirection>();

    [ContextMenu("Initialization")]
    public void Initialization() {
        foreach (MoveElectricDirection iMoveDirection in moveElectricDirections) {
            iMoveDirection.Initialization();
        } 
    }

    [ContextMenu("ResetStatus")]
    public void ResetStatus() {
        if (Status == false) {
            Status = true;
        } else {
            Status = false;
        }
        
        foreach (MoveElectricDirection iMoveDirection in moveElectricDirections) {
            iMoveDirection.SetStatuses(Status);
        }
    }

    public void SwichDirections() {
        foreach (MoveElectricDirection iMoveDirection in moveElectricDirections) {
            iMoveDirection.SwichDirections();
        }
    }



}
