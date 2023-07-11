using EPOOutline;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincipalSchemeCompanent : SelectableObject {

    [SerializeField] private Outlinable _outlinable;
    [ColorUsage(true)]
    [SerializeField] private Color _selectColor;
    private Color _hoverColor;

    public override void Start() {
        _hoverColor = _outlinable.FrontParameters.Color;
    }

    public override void OnHover() {
        if (!IsSelected) {
            SelectIndicator.SetActive(true);
            _outlinable.FrontParameters.Color = _hoverColor;
        }
    }

    public override void OnUnhover() {
        if (!IsSelected) {
            SelectIndicator.SetActive(false);
        }
    }

    public override void Select() {
        IsSelected = true;  
        SelectIndicator.SetActive(IsSelected);
        _outlinable.FrontParameters.Color = _selectColor;
    }

    public override void Unselect() {
        IsSelected = false;
        _outlinable.FrontParameters.Color = _hoverColor;
        SelectIndicator.SetActive(IsSelected);
    }
}
