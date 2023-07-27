using EPOOutline;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ObjectView : MonoBehaviour {
    public SelectableObject Object;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Material _defaultMaterial;
    public Transform[] PathElements;
    public LineRenderer LineRenderer;

    [Header("Outline")]
    [SerializeField] private Outlinable _outlinable;
    [ColorUsage(true)]
    [SerializeField] private Color _selectColor;

    public Action PathChanged;

    private Color _hoverColor;
    private List<Material> _material = new List<Material>(); // Создаем новый массив материалов

    // Инициализация элемента в момент создания
    public void Initialization(SelectableObject selectableObject) {
        Object = selectableObject;
        _defaultMaterial = Instantiate(_defaultMaterial);
        //_defaultMaterial.SetColor("_BaseColor", _defaultColor);

        _material = new List<Material>(); // Создаем список материалов для LineRenderer
        _material.Add(_defaultMaterial);
        LineRenderer.materials = _material.ToArray();

        if (!_outlinable) _outlinable = Object.gameObject.transform.GetComponent<Outlinable>();
        _hoverColor = _outlinable.FrontParameters.Color;

        UpdatePoints();

        if (Object is PolyWire) {
            Wire wire = Object.GetComponent<Wire>();
            PathChanged += wire.GenerateMeshCollider;
            if (PathElements.Length > 0) {
                foreach (Transform iPoint in PathElements) {
                    WirePoint iWirePoint = iPoint.GetComponent<WirePoint>();
                    iWirePoint.Initialize(wire);
                }
            }
        } else if (Object is BezierWire) {
            BezierWire wire = Object.GetComponent<BezierWire>();
            PathChanged += wire.GenerateMeshCollider;
            if (wire.BezierPointCreator.BezierPoints.Count() > 0) {
                foreach (Transform iPoint in wire.BezierPointCreator.BezierPoints) {
                    WirePoint iWirePoint = iPoint.GetComponent<WirePoint>();
                    iWirePoint.Initialize(wire);
                }
            }
        }
    }

    public void OnHover(bool isSelected) {
        if (!isSelected) {
            _outlinable.enabled = true;
            _outlinable.FrontParameters.Color = _hoverColor;
        }
        else {
            _outlinable.enabled = true;
            _outlinable.FrontParameters.Color = _selectColor;
        }
    }

    public void OnUnhover(bool isSelected) {
        if (isSelected) {
            _outlinable.FrontParameters.Color = _selectColor;
            _outlinable.enabled = true;
        } else {
            _outlinable.FrontParameters.Color = _hoverColor;
            _outlinable.enabled = false;
        }
    }

    public void ShowName() {
        _nameText.text += " " + Object.Name;
    }

    public void Select() {
        _outlinable.OutlineParameters.Color = _selectColor;
    }

    public void Unselect() {
        if (Object != null) {
            _outlinable.enabled = false;
            _outlinable.OutlineParameters.Color = _hoverColor;
        }
    }

    [ContextMenu("SetColor")]
    public void SetColor(Color newColor) {
        _material[0].SetColor("_BaseColor", newColor);
        LineRenderer.materials = _material.ToArray(); // Присваиваем новый массив материалов к LineRenderer
    }

    [ContextMenu("SetColor")]
    public void SetColor() {
        _material[0].SetColor("_BaseColor", _defaultColor);
        LineRenderer.materials = _material.ToArray(); // Присваиваем новый массив материалов к LineRenderer
    }

    [ContextMenu("UpdatePoints")]
    public void UpdatePoints() {
        if (Object is BezierWire) {
            BezierWire wire = Object.GetComponent<BezierWire>();
            wire.BezierPointCreator.SetWirePoints(wire.StartContact.transform, wire.EndContact.transform);
            wire.BezierPointCreator.UpdatePointsPosition();
            PathElements = (Transform[])wire.BezierPointCreator.Points;
        }

        if (PathElements == null || PathElements.Length < 2) return;

        LineRenderer.positionCount = PathElements.Length;
        for (int i = 0; i < PathElements.Length; i++) {
            Vector3 iVector3 = PathElements[i].position;
            LineRenderer.SetPosition(i, iVector3);
        }
    }

    private void OnDrawGizmosSelected() {
        Debug.Log("ObjectWiew: OnDrawGizmosSelected");
        UpdatePoints();
        for (int i = 1; i < PathElements.Length; i++) {
            Gizmos.color = _defaultColor;
            Gizmos.DrawLine(PathElements[i - 1].position, PathElements[i].position);
        }
    }

    private void OnDisable() {
        if (Object is Wire) {
            Wire wire = Object.GetComponent<Wire>();
            PathChanged -= wire.GenerateMeshCollider;
        }
    }
}
