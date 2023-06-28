using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagoContact : Contact
{
    /// <summary>
    /// Wago-зажим, которому принадлежит данный контакт
    /// </summary>
    public WagoClip ParentWagoClip;
    /// <summary>
    /// Номер контакта в зажиме
    /// </summary>
    public int Number;
    /// <summary>
    /// Подключенный контакт
    /// </summary>
    public Contact ConnectedContact;

    private Material _defaultMaterial;
    private Vector3 _defaultLocalScale;

    public override void Start() {
        _defaultMaterial = this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
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
        ConnectionData newConnect = GetConnectedCompanent();
        if (!ParentWagoClip.Connections.Contains(newConnect)) {
            ParentWagoClip.Connections.Add(newConnect);
        }
    }

    public void RemoveConnectFromList() {
        ConnectionData removeConnect = GetConnectedCompanent();
        if (ParentWagoClip.Connections.Contains(removeConnect)) {
            ParentWagoClip.Connections.Remove(removeConnect);
            ConnectedContact = null;
        } else {
            Debug.Log("Контакт: " + ConnectedContact.ContactType + "не подключен!");
        }
    }

    private ConnectionData GetConnectedCompanent() {
        if (ConnectedContact == null) return new();

        Companent companent = ConnectedContact.transform.parent.GetComponent<SelectableCollider>().SelectableObject.GetParentCompanent();
        ConnectionData connect = new();
        connect.ContactType = ConnectedContact.ContactType;
        connect.CompanentType = companent.Type;
        connect.CompanentName = companent.Name;

        return connect;
    }
}
