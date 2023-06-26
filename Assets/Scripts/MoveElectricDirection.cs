using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveElectricDirection : MonoBehaviour {
    [SerializeField] private List<MovementPath> movementPaths = new List<MovementPath>();
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _positiveDirectionMaterial;
    [SerializeField] private Material _negativeDirectionMaterial;

    public void Initialization() {
        if (movementPaths.Count > 0) {
            for (int i = 0; i < movementPaths.Count; i++) {
                movementPaths[i].Initialization(_defaultMaterial, _positiveDirectionMaterial, _negativeDirectionMaterial);
            }
        }
    }

    public void SwichDirections() {
        foreach (MovementPath iPath in movementPaths) {
            iPath.SwichDirection();
        }
    }

    public void SetStatuses(bool status) {
        foreach (MovementPath iPath in movementPaths) {
            iPath.SetStatus(status);
        }
    }

}
