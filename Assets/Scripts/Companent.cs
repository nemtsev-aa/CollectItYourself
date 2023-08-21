using System;
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
    [Header("Parameters")]
    public SwitchBox SwitchBox;
    public CompanentType Type;
    public int SlotNumber;
    public List<Contact> Contacts = new List<Contact>();

    [Header("View")]
    [SerializeField] private TextMeshProUGUI _nameText;
    public List<ObjectView> ObjectViews = new List<ObjectView>();
    public List<ElectricFieldMovingView> ElectricFieldMovingViews = new List<ElectricFieldMovingView>();

    // »нициализаци€ элемента в момент создани€
    public virtual void Init() {
        foreach (Contact iContact in Contacts) {
            iContact.Init(this);
        }
        
        foreach (ObjectView objectView in ObjectViews) {
            objectView.Init(this);
        }

        foreach (ElectricFieldMovingView electricView in ElectricFieldMovingViews) {
            electricView.SetObject(this);
        }
    }

    public override void Start() {
        base.Start();
        Init();
    }

    #region Managment
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
        foreach (ObjectView objectView in ObjectViews) {
            objectView.Select();
        }

        foreach (Contact iContact in Contacts) {
            Wire wire = iContact.ConnectionWire;
            if (wire != null) {
                wire.Select();
            }
        }
    }

    /// <summary>
    /// »зменени€, возникающие с объектом при правильном подключении (возникновение светового потока в лампе, активизаци€ индикатора)
    /// </summary>
    public virtual void Activate() {

    }

    /// <summary>
    /// »зменени€, возникающие с объектом при неправильном подключении (отсутствие светового потока в лампе, деактивизаци€ индикатора)
    /// </summary>
    public virtual void Deactivate() {

    }

    /// <summary>
    /// ”правл€ющее воздействие (переключение)
    /// </summary>
    public virtual void Action() {

    }

    public override void Unselect() {
        base.Unselect();
        
        foreach (ObjectView objectView in ObjectViews) {
            objectView.Unselect();
        }

        foreach (Contact iContact in Contacts) {
            Wire wire = iContact.ConnectionWire;
            if (wire != null) {
                wire.Unselect();
            }
        }
    }

    public override void OnMouseDrag() {
        base.OnMouseDrag();
        UpdateLocationWires();
    }
    #endregion
    
    public void UpdateLocationWires() {
        for (int i = 0; i < Contacts.Count; i++) {
            Contact iContact = Contacts[i];
            iContact.ContactPositionChanged?.Invoke();
        }
    }

    public void RemoveCompanent() {
        SwitchBox.RemoveCompanent(this);
    }

    //public void GetCompanentConnectedToContact(ConnectionData connectionData) {
    //    //Companent companent = Contacts.FirstOrDefault(companent => companent.T == searchId);
        
    //}

    public Contact GetContactByType(ContactType contactType) {
        return Contacts.Find(x => x.ContactType == contactType);
    }

    public ElectricFieldMovingView GetElectricFieldMovingView(Contact contact) {
        foreach (var iElectricFieldView in ElectricFieldMovingViews) {
            if (iElectricFieldView.GetParentContact(contact.ContactType)) {
                return iElectricFieldView;
            }
        }
        return null;
    }

    public void ShowFirstCurrentFlowLine() {
        
    }
}
