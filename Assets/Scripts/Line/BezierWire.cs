using UnityEngine;

public class BezierWire : Wire {
    public BezierPointCreator BezierPointCreator => _bezierPointCreator;
    
    [SerializeField] private BezierPointCreator _bezierPointCreator;

    [ContextMenu("Init")]
    public void Init() {
        _bezierPointCreator.SetWirePoints(StartContact.transform, EndContact.transform);
        _bezierPointCreator.Init();
        _bezierPointCreator.UpdatePointsPosition();
        ObjectView.PathElements = (Transform[])_bezierPointCreator.Points;
    }
}
