using EPOOutline;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum WagoType {
    WagoD_2,
    WagoD_3,
    WagoD_5,
    WagoU_2,
    WagoU_3,
    WagoU_5
}

public class WagoClip : Clips {
    public bool IsSelected;
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
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Animator _animator;

    public event Action<bool> OnSelect;
    public event Action<bool> OnUnselect;

    [SerializeField] private Outlinable _outlinable;
    [ColorUsage(true)]
    [SerializeField] private Color _selectColor;

    private Color _hoverColor;
    public override void Start() {
        _hoverColor = _outlinable.FrontParameters.Color;
        SelectIndicator.SetActive(false);
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

    public void ShowName() {
        _nameText.text = "WAGO " + Name;
    }
    public override void Select() {
        base.Select();
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
            Debug.Log(iConnection.CompanentName + "_" + iConnection.ContactType);
        }
    }
}
