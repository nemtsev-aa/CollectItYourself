using EPOOutline;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CompanentType {
    Input,
    Output,
    PowerSocket,
    Lamp,
    Selector,
    MotionSensor
}

public class Companent : SelectableObject {
    public bool IsSelected;

    public SwitchBox SwitchBox;
    public CompanentType Type;
    public string Name;
    public int SlotNumber;
    public List<Contact> Contacts = new List<Contact>();

    [Header("View")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Animator _animator;

    public event Action<bool> OnSelect;
    public event Action<bool> OnUnselect;

    [SerializeField] private Outlinable _outlinable;
    [ColorUsage(true)]
    [SerializeField] private Color _selectColor;

    private Color _hoverColor;
    private Plane _dragPlane;
    private Vector3 offset;
    private Camera _myMainCamera;

    //public bool IsMove;

    private void Awake() {
        _myMainCamera = Camera.main;
    }

    public override void Start() {
        _hoverColor = _outlinable.FrontParameters.Color;
    }

    public override void OnHover() {
        if (!IsSelected) {
            //SelectIndicator.SetActive(true);
            _outlinable.enabled = true;
            _outlinable.FrontParameters.Color = _hoverColor;
        }
    }

    public override void OnUnhover() {
        if (!IsSelected) {
            _outlinable.enabled = false;
            //SelectIndicator.SetActive(false);
        }
    }

    public void MovingActivate(bool status) {
        //Debug.Log("Перемещение: " + status);
        gameObject.GetComponent<BoxCollider>().enabled = status;
    }

    [ContextMenu("ShowName")]
    public void ShowName() {
        _nameText.text = Name;
    }

    public override void Select() {
        _animator.enabled = true;
        _animator.SetTrigger("Show");
        _animator.ResetTrigger("Hide");

        IsSelected = true;
        SelectIndicator.SetActive(IsSelected);

        _outlinable.OutlineParameters.Color = _selectColor;
        OnSelect?.Invoke(IsSelected);
    }

    public override void Unselect() {
        //Debug.Log("Unselect Companent");
        _animator.ResetTrigger("Show");
        _animator.SetTrigger("Hide");

        IsSelected = false;
        _outlinable.enabled = false;
        _outlinable.OutlineParameters.Color = _hoverColor;
        SelectIndicator.SetActive(IsSelected);
        OnUnselect?.Invoke(IsSelected);
    }

    public void OnMouseDown() {
        MovingActivate(true);
        //Debug.Log("Перемещение запущено");
        _dragPlane = new Plane(_myMainCamera.transform.forward, transform.position);
        Ray camRay = _myMainCamera.ScreenPointToRay(Input.mousePosition);

        _dragPlane.Raycast(camRay, out float planeDistance);
        offset = transform.position - camRay.GetPoint(planeDistance);
    }

    public void OnMouseDrag() {
        //Debug.Log("Активно");
        Ray camRay = _myMainCamera.ScreenPointToRay(Input.mousePosition);
        _dragPlane.Raycast(camRay, out float planeDistance);
        transform.position = camRay.GetPoint(planeDistance) + offset;
        UpdateLocationWires();
    }

    public virtual void OnMouseUp() {
        //Debug.Log("Перемещение завершено");
    }

    public void UpdateLocationWires() {
        for (int i = 0; i < Contacts.Count; i++) {
            Contact iContact = Contacts[i];
            if (iContact.ConnectionWire != null) {
                iContact.ConnectionWire.StartContact.transform.position = iContact.transform.position;
                iContact.ConnectionWire.LineRenderer.SetPosition(0, iContact.transform.position);
            }
        }
    }

    public void RemoveCompanent() {
        SwitchBox.RemoveCompanent(this);
    }
}
