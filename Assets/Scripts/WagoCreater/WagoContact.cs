using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagoContact : Contact {
    public WagoClip ParentWagoClip; /// Wago-зажим, которому принадлежит данный контакт
    public int Number; /// Номер контакта в зажиме
    public Contact ConnectedContact; // Подключенный контакт

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
        this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material = Material;
    }

    public void ResetMaterial() {
        this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material = _defaultMaterial;
    }

    public void AddNewConnectToList() {
        ConnectionData newConnect = GetConnectionData();
        if (!ParentWagoClip.Connections.Contains(newConnect)) {
            ParentWagoClip.Connections.Add(newConnect);
        }

    }

    public void RemoveConnectFromList() {
        ConnectionData removeConnect = GetConnectionData();
        if (ParentWagoClip.Connections.Contains(removeConnect)) {
            ParentWagoClip.Connections.Remove(removeConnect);
            ConnectedContact = null;
        } else {
            Debug.Log("Контакт: " + ConnectedContact.ContactType + "не подключен!");
        }
    }

    private ConnectionData GetConnectionData() {
        if (ConnectedContact == null) return new();

        Companent companent = ConnectedContact.GetParentCompanent();
        ConnectionData connect = new();
        connect.ContactType = ConnectedContact.ContactType;
        connect.CompanentType = companent.Type;
        connect.CompanentName = companent.Name;

        return connect;
    }

    public bool GetConnectionStatus() {
        if (ConnectedContact != null) {
            return true;
        } else {
            return false;
        }
    }

    public override void ResetContact() {
        base.ResetContact();
        RemoveConnectFromList();
        ConnectedContact = null;
        ResetMaterial();
    }


}
