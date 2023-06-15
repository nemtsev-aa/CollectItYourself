using System.Collections.Generic;
using UnityEngine;

public class MovementPath : MonoBehaviour {
    public enum PathTypes {
        Linear,
        Loop
    }

    public PathTypes PathType;
    public int movementDirection = 1;
    public int moveingTo = 0;
    public Color Color;

    public Transform[] PathElements;

    private void OnDrawGizmos() {
        if (PathElements == null || PathElements.Length < 2) return;

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
