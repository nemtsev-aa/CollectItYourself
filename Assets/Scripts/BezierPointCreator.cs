using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BezierPointCreator : MonoBehaviour {
    public IEnumerable<Transform> BezierPoints => _bezierPoints;
    public IEnumerable<Transform> Points => _points;

    [SerializeField] private Transform[] _bezierPoints = new Transform[4];
    [SerializeField] private Transform[] _points;
    [SerializeField] private int _pointsCount = 20;
    [SerializeField] private Transform _pointsParent;

    [ContextMenu("Init")]
    public void Init() {
        _points = new Transform[_pointsCount];
        
        for (int i = 0; i < _pointsCount; i++) {
            Transform newPoint = new GameObject().transform;
            newPoint.gameObject.name = "Point" + i;
            newPoint.parent = _pointsParent;
            _points[i] = newPoint;
        }

        _points[0].position = _bezierPoints[0].position;
        _points[_pointsCount-1].position = _bezierPoints[3].position;

    }

    public void SetWirePoints(Transform startPoint, Transform endPoint) {
        _bezierPoints[0].position = startPoint.position;
        _bezierPoints[3].position = endPoint.position;
    }

    public void UpdatePointsPosition() {
        for (int i = 0; i < _pointsCount-1; i++) {
            float paremeter = (float)i / _pointsCount;
            Vector3 point = Bezier.GetPoint(_bezierPoints[0].position, _bezierPoints[1].position, _bezierPoints[2].position, _bezierPoints[3].position, paremeter);
            _points[i].position = point;
        }
    }

    [ContextMenu("DrawGizmosLine")]
    public void DrawGizmosLine() {
        UpdatePointsPosition();

        Vector3 preveousePoint = _bezierPoints[0].position;
        for (int i = 0; i < _pointsCount; i++) {
            Vector3 point = _points[i].position;
            Gizmos.DrawLine(preveousePoint, point);
            preveousePoint = point;
        }
    }
}

