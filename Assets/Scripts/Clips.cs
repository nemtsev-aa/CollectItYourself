using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clips : Connections
{
    private Plane _dragPlane;
    private Vector3 offset;
    private Camera _myMainCamera;

    private void Awake() {
        _myMainCamera = Camera.main;
    }

    public virtual void OnMouseDown() {
        Debug.Log("Перемещение запущено");
        _dragPlane = new Plane(_myMainCamera.transform.forward, transform.position);
        Ray camRay = _myMainCamera.ScreenPointToRay(Input.mousePosition);

        _dragPlane.Raycast(camRay, out float planeDistance);
        offset = transform.position - camRay.GetPoint(planeDistance);
    }

    public virtual void OnMouseDrag() {
        Ray camRay = _myMainCamera.ScreenPointToRay(Input.mousePosition);

        _dragPlane.Raycast(camRay, out float planeDistance);
        transform.position = camRay.GetPoint(planeDistance) + offset;
    }

    public virtual void OnMouseUp() {
        Debug.Log("Перемещение завершено");
    }

}
