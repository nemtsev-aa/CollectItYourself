using System;
using System.Collections;
using System.Linq;
using UnityEngine;
public enum ContactType {
    Line,
    Neutral,
    GroundConductor,
    Closed,
    Open,
    LineOut,
    Wago
}

public class Contact : SelectableObject {
    public ContactType ContactType => _contactType;
    
    public Material Material;
    [SerializeField] private Renderer _renderer;
    public Wire ConnectionWire;

    public Action ContactPositionChanged;
    public ContactType _contactType;
    private Color _defaultColor;
    private float _duration = 2f;
    private Companent _parentCompanent;

    public void Initialize(Companent parentCompanent) {
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

    public IEnumerator ShowEffect() {
        for (float t = 0; t < 2f; t += Time.deltaTime) {
            SetColor(new Color(Mathf.Sin(10 * t) * 0.5f + 0.5f, 0, 0, 0));
            yield return null;
        }
        SetColor(_defaultColor); // Устанавливаем конечный цвет материала после окончания анимации
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
        return _parentCompanent;
    }

    ///Написать метод/скрипт/свойство для контакта, которое будет хранить данные о цвете и направлении магнитного поля 
    ///которые будет получать следующий подключенный компанент
    public ElectricFieldSettings GetElectricFieldSettings(Contact contact) {
        ElectricFieldSettings electricFieldSettings = new ElectricFieldSettings();

        ObjectView objectView = _parentCompanent.ObjectViews.Find(x => x.Contact.ContactType == contact.ContactType);
        if (objectView != null) {
            //Debug.Log($"Искали {contact.gameObject.name} {contact.ContactType} ; Нашли {objectView.gameObject.name} {objectView.Contact.ContactType}");
            ElectricFieldMovingView electricFieldMovingView = _parentCompanent.ElectricFieldMovingViews.Find(x => x.ObjectView == objectView);
            if (electricFieldMovingView != null) {
                electricFieldSettings.Color = electricFieldMovingView.ElecticFieldMaterial.color;
                electricFieldSettings.DirectionType = electricFieldMovingView.CurrentDirection;
                electricFieldSettings.Material = electricFieldMovingView.ElecticFieldMaterial;
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
}
