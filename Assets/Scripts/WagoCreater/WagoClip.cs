using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum WagoType {
    WagoD,
    WagoU
}

public class WagoClip : Clips {
    [Header("Parameters")]
    public SwitchBox SwitchBox;
    /// <summary>
    /// Имя зажима
    /// </summary>
    public string Name;
    /// <summary>
    /// Список контактов, принадлежащих зажиму
    /// </summary>
    public List<WagoContact> WagoContacts = new List<WagoContact>();
    /// <summary>
    /// Подключения
    /// </summary>
    public List<ConnectionData> Connections = new List<ConnectionData>();

    [Header("View")]
    public WagoType WagoType;
    [SerializeField] private Image _wagoView;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Animator _animator;

    public void ShowName() {
        _nameText.text = "WAGO " + Name;
    }
    public override void Select() {
        base.Select();
        _animator.enabled = true;
        _animator.SetTrigger("Show");
        _animator.ResetTrigger("Hide");
    }
    public override void Unselect() {
        //Debug.Log("Unselect Companent");
        _animator.ResetTrigger("Show");
        _animator.SetTrigger("Hide");
    }
    public override void OnMouseDrag() {
        base.OnMouseDrag();
        UpdateLocationWires();
    }
    public void UpdateLocationWires() {
        for (int i = 0; i < WagoContacts.Count; i++) {
            WagoContact iWagoContact = WagoContacts[i];
            if (iWagoContact.ConnectionWire != null) {
                iWagoContact.ConnectionWire.EndContact.transform.position = iWagoContact.transform.position;
                iWagoContact.ConnectionWire.LineRenderer.SetPosition(1, iWagoContact.transform.position);
            }
        }
    }

    [ContextMenu("ShowParentCompanent")]
    public void ShowParentCompanent() {
        foreach (var iConnection in Connections) {
            Debug.Log(iConnection.Companent.GetComponent<Companent>().Name + "_" + iConnection.Contact.CurrentContactType);
        }
    }
}
