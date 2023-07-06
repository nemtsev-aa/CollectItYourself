using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFieldMovingView : MonoBehaviour
{
    public SelectableObject Object;
    public ObjectView ObjectView;

    public bool Status;
    
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
    public void Initialization(Material backMaterial, Material electicalFieldMaterial) {
        _backMaterial = Instantiate(backMaterial);
        _electicFieldMaterial = Instantiate(electicalFieldMaterial);

        _defaulMoveSpeed = _electicFieldMaterial.GetFloat("_Speed");
        _backMaterial.SetColor("_BaseColor", _backColor);
        _electicFieldMaterial.SetColor("_Color", _electicFieldColor);

        _material = new List<Material> {
            _backMaterial
        }; // Создаем новый массив материалов

        _materials = new List<Material> {
            _backMaterial,
            _electicFieldMaterial
        }; // Создаем новый массив материалов

        SwitchBox switchBox = null;
        if (Object is WagoClip) {
            switchBox = Object.GetParentWagoClip().SwitchBox;   
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

        _lineRenderer.enabled = false;
    }

    private void Start() {
        Initialization(_backMaterial, _electicFieldMaterial);
    }

    public void SetObject(SelectableObject selectableObject) {
        Object = selectableObject;

    }

    private void Update() {
        UpdatePoints();
        SetStatus(Status);
        _time += Time.deltaTime;
        if (_time > 2f) {
            _time = 0f;
            SwichDirection();
        }
    }

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
    public void SetColor() {
        _materials[0].SetColor("_BaseColor", _backColor);
        _materials[1].SetColor("_Color", _electicFieldColor);
        _lineRenderer.materials = _materials.ToArray(); // Присваиваем новый массив материалов к LineRenderer
    }

    public void SetDirection(DirectionType direction) {
        CurrentDirection = direction;
    }

    [ContextMenu("SwichDirection")]
    public void SwichDirection() {
        _defaulMoveSpeed = (-1) * _defaulMoveSpeed;
        CurrentDirection = (CurrentDirection == DirectionType.Positive) ? (CurrentDirection = DirectionType.Negative) : (CurrentDirection = DirectionType.Positive);
        
        ShowCurrentFlow(_defaulMoveSpeed);
    }

    private void ShowCurrentFlow(float newSpeed) {
        _materials[1].SetFloat("_Speed", newSpeed);
        _lineRenderer.materials = _materials.ToArray(); // Присваиваем новый массив материалов к LineRenderer
    }

    private void HideCurrentFlow() {
        _lineRenderer.materials = _material.ToArray(); // Присваиваем новый массив материалов к LineRenderer
    }

    public void UpdatePoints() {
        Transform[] PathElements = ObjectView.PathElements;
        if (PathElements == null || PathElements.Length < 2) return;

        ObjectView.LineRenderer.positionCount = PathElements.Length;
        for (int i = 0; i < PathElements.Length; i++) {
            Vector3 iVector3 = PathElements[i].position;
            _lineRenderer.SetPosition(i, iVector3);

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
            switchBox = Object.GetParentWagoClip().SwitchBox;
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
