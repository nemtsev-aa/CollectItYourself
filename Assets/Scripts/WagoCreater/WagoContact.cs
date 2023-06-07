using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagoContact : Contact
{
    /// <summary>
    /// Wago-�����, �������� ����������� ������ �������
    /// </summary>
    public WagoClip ParentWagoClip;
    /// <summary>
    /// ����� �������� � ������
    /// </summary>
    public int Number;
    /// <summary>
    /// ������������ �������
    /// </summary>
    public Contact ConnectedContact;
    /// <summary>
    /// ������������ ������
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
