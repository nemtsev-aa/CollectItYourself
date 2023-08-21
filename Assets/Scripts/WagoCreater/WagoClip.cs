using System;
using System.Collections.Generic;
using UnityEngine;

public enum WagoType {
    WagoD_2,
    WagoD_3,
    WagoD_5,
    WagoU_2,
    WagoU_3,
    WagoU_5
}

public class WagoClip : Clips {
    [Header("Parameters")]
    public SwitchBox ParentSwitchBox;
    public WagoType WagoType;
    public List<WagoContact> WagoContacts = new List<WagoContact>();
    public List<ConnectionData> Connections = new List<ConnectionData>();

    [Header("View")]
    public List<ObjectView> ObjectViews = new List<ObjectView>();
    public List<ElectricFieldMovingView> ElectricFieldMovingViews = new List<ElectricFieldMovingView>();

    // Инициализация элемента в момент создания
    [ContextMenu("Initialization")]
    public void Initialization() {
        foreach (ObjectView objectView in ObjectViews) {
            objectView.Init(this);
        }

        foreach (ElectricFieldMovingView electricView in ElectricFieldMovingViews) {
            electricView.SetObject(this);
        }
    }

    public void GetConnectionsCount(bool status) {
       
    }

    #region Managment
    public override void Start() {
        base.Start();
    }

    public override void OnHover() {
        foreach (ObjectView objectView in ObjectViews) {
            objectView.OnHover(IsSelected);
        }
    }

    public override void OnUnhover() {
        foreach (ObjectView objectView in ObjectViews) {
            objectView.OnUnhover(IsSelected);
        }
    }

    public override void Select() {
        base.Select();
        foreach (ObjectView objectView in ObjectViews) {
            objectView.Select();
        }

        foreach (WagoContact iWagoContact in WagoContacts) {
            Wire wire = iWagoContact.ConnectionWire;
            if (wire != null) {
                wire.LineSelect();
            }
        }

        ParentSwitchBox.ActiveWagoClip = this;
    }

    public override void Unselect() {
        base.Unselect();
        foreach (ObjectView objectView in ObjectViews) {
            objectView.Unselect();
        }

        foreach (WagoContact iWagoContact in WagoContacts) {
            Wire wire = iWagoContact.ConnectionWire;
            if (wire != null) {
                wire.Unselect();
            }
        }
    }

    public override void OnMouseDrag() {
        base.OnMouseDrag();
        foreach (var iObjView in ObjectViews) {
            iObjView.UpdatePoints();
        }

        UpdateLocationEndContact();
    }

    public override void OnMouseUp() {
        base.OnMouseUp();
        foreach (WagoContact iWagoContact in WagoContacts) {
            Wire wire = iWagoContact.ConnectionWire;
            if (wire != null) {
                wire.GenerateMeshCollider();
            }
        }
    }
    #endregion

    public void UpdateLocationEndContact() {
        for (int i = 0; i < WagoContacts.Count; i++) {
            WagoContact iWagoContact = WagoContacts[i];
            iWagoContact.ContactPositionChanged?.Invoke();
        }

        foreach (var iView in ElectricFieldMovingViews) {
            iView.UpdatePoints();
        }
    }

    [ContextMenu("ShowParentCompanent")]
    public void ShowParentCompanent() {
        foreach (var iConnection in Connections) {
            Debug.Log(iConnection.CompanentName + "_" + iConnection.ContactType);
        }
    }

    /// <summary>
    /// Список подключенных компанентов с контактами нужного типа
    /// </summary>
    /// <param name="contactType"></param>
    public List<Companent> GetParentCompanents(ContactType contactType) {
        List<Companent> companents = new List<Companent>();
        foreach (WagoContact iWagoContact in WagoContacts) {
            Companent companent = iWagoContact.GetConnectionCompanent(); // Компанент подключенный к Wago-зажиму
            if (companent != null) {
                Contact contact = companent.GetContactByType(contactType);
                if (contact != null) {
                    companents.Add(companent);
                } else {
                    Debug.Log($"Контакт подключенный к {companent} не найден!");
                }
            } else {
                Debug.Log($"Компанент подключенный к {iWagoContact} не найден!");
            } 
        }
        return companents;
    }

    public void DeleteClip() {
        Debug.Log("DeleteClip");
        ParentSwitchBox.RemoveWagoClipFromList(this);
        Destroy(gameObject);
    }

    public ElectricFieldMovingView GetElectricFieldMovingView(WagoContact wagoContact) {
        if (wagoContact == null) {
            return ElectricFieldMovingViews[ElectricFieldMovingViews.Count-1];
        } else {
            foreach (var iElectricFieldView in ElectricFieldMovingViews) {
                if (iElectricFieldView.GetParentContact(wagoContact)) {
                    return iElectricFieldView;
                }
            }
        }
        return null;
    }

    public ElectricFieldMovingView GetCommomBusElectricFieldMovingView() {
        return ElectricFieldMovingViews[ElectricFieldMovingViews.Count-1];
    }

    public void SetElectricFieldSettings(ElectricFieldSettings settings) {
        foreach (var iField in ElectricFieldMovingViews) {
            iField.SetMaterials(new List<Material>() { settings.BackFieldMaterial, settings.ElectricFieldMaterial});
            iField.SetDirection(DirectionType.Negative);
        }
    }

    /// <summary>
    /// Корректировка направления движения магнитного поля в контактах Wago-зажима
    /// </summary>
    public void CheckFieldMovingDirection() {
        for (int i = 0; i < ElectricFieldMovingViews.Count-1; i++) {
            var iField = ElectricFieldMovingViews[i];
            var wire = iField.ObjectView.Contact.ConnectionWire;
            if (wire != null) {
                var Companent = wire.StartContact.GetParentCompanent();
                if (Companent != null) {
                    if (Companent.Type == CompanentType.Input) {
                        if (wire.StartContact.ContactType == ContactType.Neutral || wire.StartContact.ContactType == ContactType.GroundConductor) {
                            iField.SetDirection(DirectionType.Negative);
                        } else {
                            iField.SetDirection(DirectionType.Positive);
                        }
                    } else if (Companent.Type == CompanentType.Selector) {
                        if (wire.StartContact.ContactType == ContactType.Open || wire.StartContact.ContactType == ContactType.Closed) {
                            iField.SetDirection(DirectionType.Positive);
                        } else {
                            iField.SetDirection(DirectionType.Negative);
                        }
                    } else {
                        if (wire.StartContact.ContactType == ContactType.Neutral || wire.StartContact.ContactType == ContactType.GroundConductor) {
                            iField.SetDirection(DirectionType.Positive);
                        }
                        else {
                            iField.SetDirection(DirectionType.Negative);
                        }
                    }
                }
            } else {
                if (i == ElectricFieldMovingViews.Count - 1) {
                    iField.SetDirection(DirectionType.Positive);
                } else {
                    iField.SetDirection(DirectionType.Negative);
                }
            }
        }
    }

    //public void SetElectricFieldMaterials(List<Material> fieldMaterial) {
    //    foreach (var iField in ElectricFieldMovingViews) {
    //        iField.SetMaterials(fieldMaterial);
    //    }
    //}
}
