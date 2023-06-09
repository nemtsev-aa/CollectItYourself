using System;
using System.Collections.Generic;
using UnityEngine;

public enum WagoType {
    WagoD_2,
    WagoD_3,
    WagoD_5,
    WagoU_2,
    WagoU_3,
    WagoU_5
}

public class WagoClip : Clips {
    [Header("Parameters")]
    public SwitchBox ParentSwitchBox;
    public WagoType WagoType;
    public List<WagoContact> WagoContacts = new List<WagoContact>();
    public List<ConnectionData> Connections = new List<ConnectionData>();

    [Header("View")]
    public List<ObjectView> ObjectViews = new List<ObjectView>();
    public List<ElectricFieldMovingView> ElectricFieldMovingViews = new List<ElectricFieldMovingView>();

    // ������������� �������� � ������ ��������
    public void Initialization() {
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
        ParentSwitchBox.ActiveWagoClip = this;
    }

    public override void Unselect() {
        base.Unselect();
        foreach (ObjectView objectView in ObjectViews) {
            objectView.Unselect();
        }
    }

    public override void OnMouseDrag() {
        base.OnMouseDrag();
        foreach (var iObjView in ObjectViews) {
            iObjView.UpdatePoints();
        }
        UpdateLocationEndContact();
    }

    public override void OnMouseUp() {
        base.OnMouseUp();
        for (int i = 0; i < WagoContacts.Count; i++) {
            WagoContact iWagoContact = WagoContacts[i];
            iWagoContact.ConnectionWire.GenerateMeshCollider();
        } 
    }

    public void UpdateLocationEndContact() {
        for (int i = 0; i < WagoContacts.Count; i++) {
            WagoContact iWagoContact = WagoContacts[i];
            iWagoContact.ContactPositionChanged?.Invoke();
        }
    }

    [ContextMenu("ShowParentCompanent")]
    public void ShowParentCompanent() {
        foreach (var iConnection in Connections) {
            Debug.Log(iConnection.CompanentName + "_" + iConnection.ContactType);
        }
    }

    public void DeleteClip() {
        Debug.Log("DeleteClip");
        ParentSwitchBox.RemoveWagoClipFromList(this);
        Destroy(gameObject);
    }
}
