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
    private Vector3 offset;
    private Pointer _pointer;

    public virtual void Start() {
        if (SelectIndicator != null) SelectIndicator.SetActive(false);
        _defaultScale = transform.localScale;
        _pointer = ServiceLocator.Current.Get<Pointer>();
    }

    #region Managment
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
    #endregion

    public virtual void OnMouseDown() {
        if (Movable) {
            offset = transform.position - _pointer.GetPosition();
        }
    }

    public virtual void OnMouseDrag() {
        if (Movable) {
             transform.position = _pointer.GetPosition() + offset;
        }
    }

    public virtual void OnMouseUp() {
        
    }

    public WagoClip GetParentWagoClip() {
        return GetComponent<WagoClip>();
    }

    public Wire GetParentWire() {
        return GetComponent<Wire>();
    }

    public virtual Companent GetParentCompanent() {
        return GetComponent<Companent>();
    }
}

