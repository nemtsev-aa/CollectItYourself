using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePoint : SelectableObject {

    private Wire _parentWire;

    public void Initialize(Wire wire) {
        _parentWire = wire;
    }

    public override void OnMouseDrag() {
        base.OnMouseDrag();
        //Debug.Log("WirePoint: OnMouseDrag");
        _parentWire.ObjectView.UpdatePoints();
    }

    public override void OnMouseUp() {
        base.OnMouseUp();
        _parentWire.ObjectView.PathChanged?.Invoke();
    }
}
