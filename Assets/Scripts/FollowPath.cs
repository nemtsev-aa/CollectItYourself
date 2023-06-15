using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {
    public enum MovemantType {
        Moveing,
        Lerping
    }

    public MovemantType Type = MovemantType.Moveing;
    public MovementPath MyPath;
    public float speed = 1;
    public float maxDistance = 0.1f;

    private IEnumerator<Transform> pointInPath;

    private void Start() {
        if (MyPath == null) {
            Debug.Log("Путь не задан!");
            return;
        }
        pointInPath = MyPath.GetNextPathPoint();
        pointInPath.MoveNext();
        if (pointInPath.Current == null) {
            Debug.Log("Нужны точки!");
            return;
        }
        transform.position = pointInPath.Current.position;
    }

    private void Update() {
        if (pointInPath == null || pointInPath.Current == null) return;

        if (Type == MovemantType.Moveing) {
            transform.position = Vector3.MoveTowards(transform.position, pointInPath.Current.position, Time.deltaTime * speed);
        }
        else if (Type == MovemantType.Lerping) {
            transform.position = Vector3.Lerp(transform.position, pointInPath.Current.position, Time.deltaTime * speed);
        }

        var distanceSqure = (transform.position - pointInPath.Current.position).sqrMagnitude;
        if (distanceSqure < maxDistance * maxDistance) {
            pointInPath.MoveNext();
        }
    }
}
