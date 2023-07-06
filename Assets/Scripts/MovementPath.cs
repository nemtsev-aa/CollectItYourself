using System.Collections.Generic;
using UnityEngine;
public enum DirectionType {
    Positive,
    Negative
}

public class MovementPath : MonoBehaviour {
    [field: SerializeField] public bool Status { get; private set; }

    public enum PathTypes {
        Linear,
        Loop
    }

    public PathTypes PathType;
    public int movementDirection = 1;
    public int moveingTo = 0;
    public Color Color;

    public DirectionType CurrentDirection = DirectionType.Positive;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _positiveDirectionMaterial;
    [SerializeField] private Material _negativeDirectionMaterial;

    public Transform[] PathElements;
    public LineRenderer LineRenderer;

    public void Initialization(Material defaultMaterial, Material positiveDirectionMaterial, Material negativeDirectionMaterial) {
        _defaultMaterial = defaultMaterial;
        _positiveDirectionMaterial = positiveDirectionMaterial;
        _negativeDirectionMaterial = negativeDirectionMaterial;
    }

    private void Update() {
        SetStatus(Status);
    }

    public void SetStatus(bool status) {
        Status = status;
        if (Status) {
            ShowCurrentFlow();
        } else {
            HideCurrentFlow();
        }
    }

    public void SetDirection(DirectionType direction) {
        CurrentDirection = direction;
    }

    [ContextMenu("SwichDirection")]
    public void SwichDirection() {
        if (CurrentDirection == DirectionType.Positive) {
            CurrentDirection = DirectionType.Negative;
        } else {
            CurrentDirection = DirectionType.Positive;
        }
    }

    private void ShowCurrentFlow() {
        if (CurrentDirection == DirectionType.Positive) {
            LineRenderer.material = _positiveDirectionMaterial;
        } else {
            LineRenderer.material = _negativeDirectionMaterial;
        }
        
    }

    private void HideCurrentFlow() {
        LineRenderer.material = _defaultMaterial;
    }


    private void OnDrawGizmos() {
        if (PathElements == null || PathElements.Length < 2) return;
        
        LineRenderer.positionCount = PathElements.Length;
        for (int i = 0; i < PathElements.Length; i++) {
            Vector3 iVector3 = PathElements[i].position;
            LineRenderer.SetPosition(i, iVector3);
        }
                
        for (int i = 1; i < PathElements.Length; i++) {
            Gizmos.color = Color;
            Gizmos.DrawLine(PathElements[i - 1].position, PathElements[i].position);
        }

        if (PathType == PathTypes.Loop) {
            Gizmos.color = Color;
            Gizmos.DrawLine(PathElements[0].position, PathElements[PathElements.Length-1].position);
        }
    }

    public IEnumerator<Transform> GetNextPathPoint() {
        if (PathElements == null || PathElements.Length < 1) yield break;

        while (true) {
            yield return PathElements[moveingTo];
            if (PathElements.Length == 1) continue;

            if (PathType == PathTypes.Linear) {
                if (moveingTo <= 0) {
                    movementDirection = 1;
                }
                else if (moveingTo >= PathElements.Length - 1) {
                    movementDirection = -1;
                }
            }

            moveingTo += movementDirection;
            
            if (PathType == PathTypes.Loop) {
                if (moveingTo >= PathElements.Length) {
                    moveingTo = 0;
                }
                
                if (moveingTo < 0) {
                    moveingTo = PathElements.Length - 1;
                }
            }
        }
    }
}
