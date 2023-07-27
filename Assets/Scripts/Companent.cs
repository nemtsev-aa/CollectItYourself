using EPOOutline;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [Header("Parameters")]
    public SwitchBox SwitchBox;
    public CompanentType Type;
    public int SlotNumber;
    public List<Contact> Contacts = new List<Contact>();

    [Header("View")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Animator _animator;
    public List<ObjectView> ObjectViews = new List<ObjectView>();
    public List<ElectricFieldMovingView> ElectricFieldMovingViews = new List<ElectricFieldMovingView>();

    // Инициализация элемента в момент создания
    public void Initialization() {
        foreach (Contact iContact in Contacts) {
            iContact.Initialize(this);
        }
        
        foreach (ObjectView objectView in ObjectViews) {
            objectView.Initialization(this);
        }

        foreach (ElectricFieldMovingView electricView in ElectricFieldMovingViews) {
            electricView.SetObject(this);
        }
    }

    public override void Start() {
        base.Start();
        Initialization();
    }

    [ContextMenu("ShowName")]
    public void ShowName() {
        _nameText.text = Name;
    }

    public override void OnHover() {
        foreach (ObjectView objectView in ObjectViews) {
            objectView.OnHover(IsSelected);
        }
    }

    public override void OnUnhover() {
        foreach (ObjectView objectView in ObjectViews) {
            objectView.OnUnhover(IsSelected);
        }
    }

    public override void Select() {
        base.Select();
        if (_animator) {
            _animator.enabled = true;
            _animator.SetTrigger("Show");
            _animator.ResetTrigger("Hide");
        }
        foreach (ObjectView objectView in ObjectViews) {
            objectView.Select();
        }
    }

    public override void Unselect() {
        base.Unselect();
        if (_animator) {
            _animator.ResetTrigger("Show");
            _animator.SetTrigger("Hide");
        }
        foreach (ObjectView objectView in ObjectViews) {
            objectView.Unselect();
        }
    }

    public override void OnMouseDrag() {
        base.OnMouseDrag();
        UpdateLocationWires();
    }

    public void UpdateLocationWires() {
        for (int i = 0; i < Contacts.Count; i++) {
            Contact iContact = Contacts[i];
            iContact.ContactPositionChanged?.Invoke();
        }
    }

    public void RemoveCompanent() {
        SwitchBox.RemoveCompanent(this);
    }

    public void GetCompanentConnectedToContact(ConnectionData connectionData) {
        //Companent companent = Contacts.FirstOrDefault(companent => companent.T == searchId);
        
    }
}
