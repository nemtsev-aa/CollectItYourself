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
    /// <summary>
    /// Подключающий провод
    /// </summary>
    public Wire ConnectionWire;

    private Material defaultMaterial;

    public override void Start() {
        defaultMaterial = this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
    }

    public override void OnHover() {
        base.OnHover();
        transform.localScale = Vector3.one * 1.5f;
    }

    public void SetMaterial() {
        this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material = Material;
    }

    public void ResetMaterial() {
        this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material = defaultMaterial;
    }
}
