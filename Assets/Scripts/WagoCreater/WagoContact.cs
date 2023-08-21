using System;
using UnityEngine;

public class WagoContact : Contact {
    public WagoClip ParentWagoClip; /// Wago-зажим, которому принадлежит данный контакт
    public int Number; /// Номер контакта в зажиме
    public Contact ConnectedContact; // Подключенный контакт
    public bool ConnectionStatus {
        get {
            if (ConnectedContact != null) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    private Material _defaultMaterial;
    private Vector3 _defaultLocalScale;

    public override void Start() {
        _defaultMaterial = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
        _defaultLocalScale = transform.localScale;
    }   

    public override void OnHover() {
        transform.localScale = _defaultLocalScale * 1.5f;
    }

    public override void OnUnhover() {
        transform.localScale = _defaultLocalScale;
    }

    public void SetMaterial() {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material = Material;
    }

    public void ResetMaterial() {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material = _defaultMaterial;
    }

    public void AddNewConnectToList() {
        ConnectionData newConnect = GetConnectionData();
        if (!ParentWagoClip.Connections.Contains(newConnect)) {
            ParentWagoClip.Connections.Add(newConnect);
            ParentWagoClip.ParentSwitchBox.OnConnectionsCountChanged?.Invoke(true);
        }
    }

    public void RemoveConnectFromList() {
        ConnectionData removeConnect = GetConnectionData();
        if (ParentWagoClip.Connections.Contains(removeConnect)) {
            ParentWagoClip.Connections.Remove(removeConnect);
            ConnectedContact = null;
            ParentWagoClip.ParentSwitchBox.OnConnectionsCountChanged?.Invoke(false);
        } else {
            Debug.Log("Контакт: " + ConnectedContact.ContactType + "не подключен!");
        }
    }

    public ConnectionData GetConnectionData() {
        if (ConnectedContact == null) return new();

        Companent companent = ConnectedContact.GetParentCompanent();
        ConnectionData connect = new();
        connect.ContactType = ConnectedContact.ContactType;
        connect.CompanentType = companent.Type;
        connect.CompanentName = companent.Name;

        return connect;
    }

    /// <summary>
    /// Подключенный к Wago-контакту компанент
    /// </summary>
    /// <returns></returns>
    public Companent GetConnectionCompanent() {
        if (ConnectedContact == null) return null;

        Companent companent = ConnectedContact.GetParentCompanent();
        if (companent != null) return companent;
        else return null; 
    }

    public ElectricFieldMovingView GetElectricFieldMovingView() {
        foreach (var iElectricFieldView in ParentWagoClip.ElectricFieldMovingViews) {
            if (iElectricFieldView.GetParentContact(this)) {
                return iElectricFieldView;
            }
        }
        return null;
    }

    public void SetElectricFieldMovingDirection(Contact startContact) {
        if (startContact.ContactType == ContactType.Neutral || startContact.ContactType == ContactType.GroundConductor) {
            GetElectricFieldMovingView().SwichDirection();
            Debug.Log($"Направление у контакта {this} изменено!");
        }



        //if (ConnectionWire.ElectricFieldMovingView.CurrentDirection == DirectionType.Positive) {
        //    GetElectricFieldMovingView().SetDirection(DirectionType.Negative);
        //} else {
        //    GetElectricFieldMovingView().SetDirection(DirectionType.Positive);
        //}
    }

    public override void ResetContact() {
        base.ResetContact();
        RemoveConnectFromList();
        ConnectedContact = null;
        ResetMaterial();
    }
}
