using System;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public bool IsSelected;
    public GameObject SelectIndicator;
    public string Name;
    public bool Movable;

    public event Action<SelectableObject, bool> OnSelect;
    public event Action<SelectableObject, bool> OnUnselect;

    private Vector3 _defaultScale;
    private Plane _dragPlane;
    private Vector3 offset;
    private Camera _myMainCamera;

    public virtual void Start() {
        SelectIndicator.SetActive(false);
        _defaultScale = transform.localScale;
        _myMainCamera = Camera.main;
    }

    public virtual void OnHover() {
        transform.localScale = _defaultScale * 1.05f;
    }

    public virtual void OnUnhover() {
        transform.localScale = _defaultScale;
    }

    public virtual void Select() {
        IsSelected = true;
        if (SelectIndicator != null) SelectIndicator.SetActive(true);
        OnSelect?.Invoke(this, true);
    }

    public virtual void Unselect() {
        IsSelected = false;
        if (SelectIndicator != null) SelectIndicator.SetActive(false);
        OnUnselect?.Invoke(this, false);
    }

    public virtual Companent GetParentCompanent() {
        return GetComponent<Companent>();
    }

    public WagoClip GetParentWagoClip() {
        return GetComponent<WagoClip>();
    }

    public Wire GetParentWire() {
        return GetComponent<Wire>();
    }

    public virtual void OnMouseDown() {
        if (Movable) {
            _dragPlane = new Plane(_myMainCamera.transform.forward, transform.position);
            Ray camRay = _myMainCamera.ScreenPointToRay(Input.mousePosition);

            _dragPlane.Raycast(camRay, out float planeDistance);
            offset = transform.position - camRay.GetPoint(planeDistance);
        }
    }

    public virtual void OnMouseDrag() {
        if (Movable) {
            Ray camRay = _myMainCamera.ScreenPointToRay(Input.mousePosition);

            _dragPlane.Raycast(camRay, out float planeDistance);
            transform.position = camRay.GetPoint(planeDistance) + offset;
        }
    }

    public virtual void OnMouseUp() {
        
    }
}

