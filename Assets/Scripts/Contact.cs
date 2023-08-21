using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ContactType {
    Line,
    Neutral,
    GroundConductor,
    Closed,
    Open,
    LineOut,
    Wago,
    Key_Line,
    Key_Open,
    Key_Close
}

public class Contact : SelectableObject {
    public ContactType ContactType =>_contactType;
    public Material Material => _material;
    public Wire ConnectionWire;

    public Action ContactPositionChanged;

    [SerializeField] private ContactType _contactType;
    [SerializeField] private Material _material;
    [SerializeField] private Renderer _renderer;

    private Color _defaultColor;
    private float _duration = 2f;
    private Companent _parentCompanent;

    public void Init(Companent parentCompanent) {
        _parentCompanent = parentCompanent;
    }

    public override void OnHover() {
        //transform.localScale = Vector3.one * 1.5f;
    }

    [ContextMenu("StartBlink")]
    public void StartBlink() {
        _defaultColor = Material.color;
        StartCoroutine(ShowEffect());
    }

    private IEnumerator ShowEffect() {
        for (float t = 0; t < 2f; t += Time.deltaTime) {
            SetColor(new Color(Mathf.Sin(10 * t) * 0.5f + 0.5f, 0, 0, 0));
            yield return null;
        }
        SetColor(_defaultColor); // Устанавливаем конечный цвет материала после окончания анимации
    }

    public void SetMaterial(Material material) {
        _material = material;
    }

    public void SetColor(Color newColor) {
        if (gameObject != null) {
            for (int m = 0; m < _renderer.materials.Length; m++) {
                _renderer.materials[m].SetColor("_BaseColor", newColor);
            }
        }
    }

    public override void OnMouseDrag() {
        base.OnMouseDrag();
        ContactPositionChanged?.Invoke();
    }

    public override Companent GetParentCompanent() {
        if (_parentCompanent != null) {
            return _parentCompanent;
        } else {
            return null;
        }
    }

    /// <summary>
    /// Запрос списка компанентов к которым подключен контакт
    /// </summary>
    /// <returns></returns>
    public List<Companent> GetConnectionCompanents() {
        List<Companent> companents = new List<Companent>();
        if (ConnectionWire != null) {
            WagoClip wagoClip = ConnectionWire.GetParentWagoClip();
            foreach (WagoContact iWagoContact in wagoClip.WagoContacts) {
                if (iWagoContact.ConnectionStatus == true) {
                    companents.Add(iWagoContact.GetConnectionCompanent());
                }
            }
        }

        return companents;
    }

    //Данные о цвете и направлении магнитного поля 
    // которые будет получать следующий подключенный компанент
    public ElectricFieldSettings GetElectricFieldSettings() {
        ElectricFieldSettings electricFieldSettings = new ElectricFieldSettings();

        ObjectView objectView = _parentCompanent.ObjectViews.Find(x => x.Contact.ContactType == this.ContactType);
        if (objectView != null) {
            //Debug.Log($"Искали {contact.gameObject.name} {contact.ContactType} ; Нашли {objectView.gameObject.name} {objectView.Contact.ContactType}");
            ElectricFieldMovingView electricFieldMovingView = _parentCompanent.ElectricFieldMovingViews.Find(x => x.ObjectView == objectView);
            if (electricFieldMovingView != null) {
                electricFieldSettings.Color = electricFieldMovingView.FieldMaterials[1].color;
                electricFieldSettings.DirectionType = electricFieldMovingView.CurrentDirection;
                electricFieldSettings.ElectricFieldMaterial = electricFieldMovingView.FieldMaterials[1];
                electricFieldSettings.BackFieldMaterial = electricFieldMovingView.FieldMaterials[0];
                electricFieldSettings.Contact = this;
                return electricFieldSettings;
            } else {
                Debug.LogError("GetElectricFieldSettings: electricFieldMovingView не найден!");
            }
        } else {
            Debug.LogError("GetElectricFieldSettings: objectView не найден!");
        }
        return electricFieldSettings;
    }

    public virtual void ResetContact() {
        ContactPositionChanged -= ConnectionWire.SetNewPositionEndContact;
        ConnectionWire = null;
    }

    public List<Material> GetElectricFieldMaterials() {
        ObjectView objectView = _parentCompanent.ObjectViews.Find(x => x.Contact.ContactType == this.ContactType);
        if (objectView != null) {
            ElectricFieldMovingView electricFieldMovingView = _parentCompanent.ElectricFieldMovingViews.Find(x => x.ObjectView == objectView);
            if (electricFieldMovingView != null) {
                return electricFieldMovingView.FieldMaterials;
            }
        }
        return null;
    }
}
