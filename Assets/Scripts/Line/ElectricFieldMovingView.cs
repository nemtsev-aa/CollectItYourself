using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldState {
    Filling,
    Show,
    Hide,
}
public struct ElectricFieldSettings {
    public Material BackFieldMaterial;
    public Material ElectricFieldMaterial;
    public Color Color;
    public DirectionType DirectionType;
    public Contact Contact;
}

public class ElectricFieldMovingView : MonoBehaviour {
    public SelectableObject Object;
    public ObjectView ObjectView;
    public FieldState CurrentState => _currentState;
    public DirectionType CurrentDirection => _currentDirection;
    public float FillingValue { get { return _fillingValue; } private set { } }
    public List<Material> FieldMaterials => _fieldMaterials;

    [SerializeField] private FieldState _currentState = FieldState.Hide;
    [SerializeField] private DirectionType _currentDirection = DirectionType.Positive;
    [SerializeField] private Color _backColor;
    [SerializeField] private Color _electicFieldColor;
    
    [SerializeField] private float _fillingValue = 1f;
    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private List<Material> _fieldMaterials;
    [SerializeField] private Material _backMaterial;
    [SerializeField] private Material _electicFieldMaterial;
    
    private float _defaulMoveSpeed;
    private float _time;
    
    // Инициализация элемента в момент создания
    public void Initialization() {
        if (_backMaterial != null && _electicFieldMaterial != null) {
            _backMaterial = Instantiate(_backMaterial);
            _electicFieldMaterial = Instantiate(_electicFieldMaterial);
            _fieldMaterials = new List<Material> { _backMaterial, _electicFieldMaterial }; // Создаем новый массив материалов
        
            _defaulMoveSpeed = _electicFieldMaterial.GetFloat("_Speed");
            if (_defaulMoveSpeed == 0f) {
                Debug.Log($"{Object.name} скорость электрического поля 0!");
            }

            _backMaterial.SetColor("_BaseColor", _backColor);
            _electicFieldMaterial.SetColor("_Color", _electicFieldColor);

            _lineRenderer.materials = _fieldMaterials.ToArray();
        }

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

        SetDirection(_currentDirection);
        UpdatePoints();
        SetState(FieldState.Hide);
    }

    public void SetObject(SelectableObject selectableObject) {
        Object = selectableObject;
        Initialization();
    }

    public void SetMaterials(List<Material> fieldMaterials) {
        // Создаём копии материалов источника для дальнейшей независимой настройки и управления
        _fieldMaterials = new List<Material>() { Instantiate(fieldMaterials[0]), Instantiate(fieldMaterials[1]) };
        if (_fieldMaterials.Count > 0) {
            _fieldMaterials.Clear();
            _fieldMaterials.AddRange(fieldMaterials.ToArray());
        }
    }

    [ContextMenu("SetColor")]
    public void SetColor(Color electicFieldColor) {
        _electicFieldColor = electicFieldColor;
        _electicFieldMaterial.SetColor("_Color", _electicFieldColor);
        //_lineRenderer.material = _electicFieldMaterial;

        //Debug.Log($"Цвет установлен! {_lineRenderer.materials[0].color}");
    }
    
    [ContextMenu("SwichDirection")]
    public void SwichDirection() {
        _currentDirection = _currentDirection == DirectionType.Positive ? DirectionType.Negative : DirectionType.Positive;
        SetDirection(_currentDirection);
    }

    public void SetDirection(DirectionType direction) {
        _currentDirection = direction;
        _defaulMoveSpeed = _fieldMaterials[1].GetFloat("_Speed");
        
        float newSpeed = 0f;
        if (direction == DirectionType.Negative) {
            newSpeed = -_defaulMoveSpeed;
        } else {
            newSpeed = _defaulMoveSpeed;
        }
        SetCurrentFlow(newSpeed);
    }

    public void SetCurrentFlow(float newSpeed) {
        //Debug.Log($"{Object.Name} swichDirection: new speed {newSpeed}");
        _fillingValue = (-1) * (newSpeed / _defaulMoveSpeed);
        _lineRenderer.material.SetFloat("_Speed", newSpeed);
    }

    private void HideCurrentFlow() {
        List<Material> materials = new List<Material>();
        _lineRenderer.materials = materials.ToArray();      // Удаляем материалы у LineRenderer
    }

    #region StateManagment
    public void SetStatus(bool status) {
        if (status) {
            if (_currentState == FieldState.Hide) {
                SetState(FieldState.Filling); 
            }
            else if (_currentState == FieldState.Filling) {
                SetState(FieldState.Show);
            }
        } else {
            SetState(FieldState.Hide);
        }
    }

    private void SetState(FieldState state) {
        _currentState = state;

        switch (_currentState) {
            case FieldState.Filling:
                ShowField_Filling();
                break;
            case FieldState.Show:
                ShowField_Current();
                break;
            case FieldState.Hide:
                HideField();
                break;
            default:
                break;
        }
    }

    private void ShowField_Filling() {
        _lineRenderer.enabled = true;
        _lineRenderer.textureMode = LineTextureMode.Stretch;
        _lineRenderer.material = _fieldMaterials[0];
    }

    private void ShowField_Current() {
        ObjectView.LineRenderer.enabled = true;
        _lineRenderer.enabled = true;
        _lineRenderer.textureMode = LineTextureMode.Tile;
        _lineRenderer.material = _fieldMaterials[1];
        SetDirection(_currentDirection);
    }

    private void HideField() {
        _lineRenderer.enabled = false;
        HideCurrentFlow();
    }
    #endregion

    [ContextMenu("Demonstration")]
    public void StartDemonstration() {
        SetState(FieldState.Filling);
        StartCoroutine(Demonstration());
    }

    public IEnumerator Demonstration() {
        SetState(FieldState.Filling);

        float duration = 0.2f;    // Длительность изменения (в секундах)
        float elapsed = 0f;     // Прошедшее время

        float startValue = _fillingValue;
        float currentValue = _fillingValue;
        float endValue = 0f;

        while (elapsed < duration || currentValue != 0f) {
            float t = elapsed / duration;
            currentValue = Mathf.Lerp(startValue, endValue, t);
            _lineRenderer.material.SetFloat("_FillingValue", currentValue);
            
            elapsed += Time.deltaTime; // Прирост времени
            yield return null; // Переход на следующий кадр
        }

        ObjectView.LineRenderer.enabled = false;
        SetState(FieldState.Show);
    }

    public void UpdatePoints() {
        Transform[] PathElements = ObjectView.PathElements;
        if (PathElements == null || PathElements.Length < 2) return;

        _lineRenderer.positionCount = PathElements.Length;
        for (int i = 0; i < PathElements.Length; i++) {
            _lineRenderer.SetPosition(i, PathElements[i].position);
        }
    }

    public bool GetParentContact(ContactType contactType) {
        if (ObjectView.Contact.ContactType == contactType) {
            return true;
        }
        return false;
    }

    public bool GetParentContact(Contact contact) {
        if (ObjectView.Contact == contact) {
            return true;
        }
        return false;
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
