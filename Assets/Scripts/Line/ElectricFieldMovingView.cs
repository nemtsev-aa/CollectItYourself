using System;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFieldMovingView : MonoBehaviour {
    public SelectableObject Object;
    public ObjectView ObjectView;
    public bool Status;
    public Material ElecticFieldMaterial => _electicFieldMaterial;


    [SerializeField] private Color _backColor;
    [SerializeField] private Color _electicFieldColor;
    public DirectionType CurrentDirection = DirectionType.Positive;

    [SerializeField] private Material _backMaterial;
    [SerializeField] private Material _electicFieldMaterial;
    [SerializeField] private LineRenderer _lineRenderer;

    private float _defaulMoveSpeed;
    private List<Material> _material;
    private List<Material> _materials;
    private float _time;

    // Инициализация элемента в момент создания
    public void Initialization() {
        _backMaterial = Instantiate(_backMaterial);
        _electicFieldMaterial = Instantiate(_electicFieldMaterial);

        _defaulMoveSpeed = _electicFieldMaterial.GetFloat("_Speed");
        if (_defaulMoveSpeed == 0f) {
            Debug.Log($"{Object.name} скорость электрического поля 0!");
        }

        _backMaterial.SetColor("_BaseColor", _backColor);
        _electicFieldMaterial.SetColor("_Color", _electicFieldColor);

        _material = new List<Material> {
            _backMaterial
        }; // Создаем новый массив материалов

        _materials = new List<Material> {
            _backMaterial,
            _electicFieldMaterial
        }; // Создаем новый массив материалов

        //_lineRenderer.materials = _material.ToArray();

        SwitchBox switchBox = null;
        if (Object is WagoClip) {
            switchBox = Object.GetParentWagoClip().ParentSwitchBox;   
        } else if (Object is Wire) {
            switchBox = Object.GetParentWire().SwitchBox;          
        } else if (Object is Companent) {
            switchBox = Object.GetParentCompanent().SwitchBox;
        }

        if (switchBox) {
            switchBox.OnShowCurrent += SetStatus;
        } else {
            Debug.Log("ElectricFieldMovingView: SwitchBox not found");
        }

        UpdatePoints();
        SetDirection(CurrentDirection);
        _lineRenderer.enabled = false;
    }

    public void SetObject(SelectableObject selectableObject) {
        Object = selectableObject;
        Initialization();
    }

    public void SetMaterial(Material material) {
        _electicFieldMaterial = material;
        _lineRenderer.material = _electicFieldMaterial;
    }

    //private void Update() {
    //    UpdatePoints();
    //    SetStatus(Status);
    //    SetDirection(CurrentDirection);
    //    //_time += Time.deltaTime;
    //    //if (_time > 2f) {
    //    //    _time = 0f;
    //    //    SwichDirection();
    //    //}
    //}

    public void SetStatus(bool status) {
        Status = status;
        _lineRenderer.enabled = status;
        if (Status) {
            ShowCurrentFlow(_defaulMoveSpeed);
        } else {
            HideCurrentFlow();
        }
    }

    [ContextMenu("SetColor")]
    public void SetColor(Color electicFieldColor) {
        _electicFieldColor = electicFieldColor;
        _electicFieldMaterial.SetColor("_Color", _electicFieldColor);
        _lineRenderer.material = _electicFieldMaterial;
        //Debug.Log($"Цвет установлен! {_lineRenderer.materials[0].color}");
    }

    public void SetDirection(DirectionType direction) {
        //Debug.Log($"{CurrentDirection}  скорость {_defaulMoveSpeed}");
        //CurrentDirection = direction;
        //float newSpeed = CurrentDirection == DirectionType.Negative ? newSpeed = -_defaulMoveSpeed : _defaulMoveSpeed;
        //Debug.Log($"{direction}  скорость {newSpeed}");
        //ShowCurrentFlow(newSpeed);
       
        if (CurrentDirection == DirectionType.Negative) {
            SwichDirection();
        }
    }

    [ContextMenu("SwichDirection")]
    public void SwichDirection() {
        _defaulMoveSpeed = (-1) * _defaulMoveSpeed;
        CurrentDirection = (CurrentDirection == DirectionType.Positive) ? (CurrentDirection = DirectionType.Negative) : (CurrentDirection = DirectionType.Positive);
        ShowCurrentFlow(_defaulMoveSpeed);
    }

    public void ShowCurrentFlow(float newSpeed) {
        _electicFieldMaterial.SetFloat("_Speed", newSpeed);
        _lineRenderer.material = _electicFieldMaterial;
    }

    private void HideCurrentFlow() {
        _lineRenderer.materials = _material.ToArray(); // Присваиваем новый массив материалов к LineRenderer
    }

    public void UpdatePoints() {
        Transform[] PathElements = ObjectView.PathElements;
        if (PathElements == null || PathElements.Length < 2) return;

        _lineRenderer.positionCount = PathElements.Length;
        for (int i = 0; i < PathElements.Length; i++) {
            _lineRenderer.SetPosition(i, PathElements[i].position);
        }
    }

    private void OnDrawGizmos() {
        UpdatePoints();
        Transform[] PathElements = ObjectView.PathElements;
        for (int i = 1; i < PathElements.Length; i++) {
            Gizmos.color = _backColor;
            Gizmos.DrawLine(PathElements[i - 1].position, PathElements[i].position);
        }
    }

    private void OnDisable() {
        SwitchBox switchBox = new SwitchBox();
        if (Object is WagoClip) {
            switchBox = Object.GetParentWagoClip().ParentSwitchBox;
        }
        else if (Object is Wire) {
            switchBox = Object.GetParentWire().SwitchBox;
        }
        else if (Object is Companent) {
            switchBox = Object.GetParentCompanent().SwitchBox;
        }

        if (switchBox) {
            switchBox.OnShowCurrent -= SetStatus;
        }
    }
}
