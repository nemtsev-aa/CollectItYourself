using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SelectionState {
    ClipsSelected,
    Frame,
    Other,
    CompanentSelected,
    ContactSelected,
    WagoContactSelected,
    WireSelected
}


public class Management : MonoBehaviour, IService {
    public bool IsOverUI => _isOverUI;
    [SerializeField] private Camera _camera;

    private List<SelectableObject> _listOfSelected = new List<SelectableObject>();
    private SelectionState _currentSelectionState;
    private WireCreator _wireCreator;
    private Pointer _pointer;
    private SelectableObject _hovered;
    private bool _isOverUI;
    private TrainingModeController _trainingModeController;

    public void Init() {
        _wireCreator = ServiceLocator.Current.Get<WireCreator>();
        _pointer = ServiceLocator.Current.Get<Pointer>();
        _trainingModeController = ServiceLocator.Current.Get<TrainingModeController>();
    }

    void Update() {

        if (EventSystem.current == null) {
            // Создаем новый объект EventSystem
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
        }
        _isOverUI = EventSystem.current.IsPointerOverGameObject();

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition); // Луч из камеры в точку расположения курсора мыши на экране
        Debug.DrawLine(ray.origin, ray.direction * 10f, Color.red); // Визуализация луча

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            SelectableCollider selectable = hit.collider.GetComponent<SelectableCollider>();
            if (selectable) {
                SelectableObject hitSelectable = selectable.SelectableObject;
                if (_hovered) {
                    if (_hovered != hitSelectable) {
                        _hovered.OnUnhover();
                        _hovered = hitSelectable;
                        _hovered.OnHover();
                    }
                }
                else {
                    _hovered = hitSelectable;
                    _hovered.OnHover();
                }
            }
            else {
                UnhowerCurrent();
            }
        }
        else UnhowerCurrent();

        if (Input.GetMouseButtonDown(0)) {
            if (_hovered) {
                if (_hovered.TryGetComponent(out WirePoint wirePoint)) Select(_hovered);
                else UnselectAll();

                if (_hovered is WagoContact) {
                    WagoContact wagoContact = _hovered.GetComponent<WagoContact>();
                    if (wagoContact.ConnectionWire == null) {
                        _wireCreator.EndContact = wagoContact;
                    }
                    else {
                        if (_wireCreator.StartContact == null) {
                            Wire wire = wagoContact.ConnectionWire;
                            ServiceLocator.Current.Get<SwitchBoxesManager>().ActiveSwichBox.RemoveLineToList(wire);
                            _pointer.Disconnect();
                        }
                        else {
                            Debug.Log("Wago-контакт занят!");
                        }
                    }
                }
                else if (_hovered is Contact) {
                    Contact contact = _hovered.GetComponent<Contact>();
                    if (contact.ConnectionWire == null) {
                        _wireCreator.StartContact = contact;

                        Select(contact);
                    } else {
                        Debug.Log("Контакт занят!");
                    }
                }
                else if (_hovered is PrincipalSchemeCompanent) {
                    if (_hovered.GetComponent<PrincipalSchemeCompanent>().IsSelected) Unselect(_hovered);
                    else Select(_hovered);
                }
            } else {
                UnselectAll();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (_listOfSelected.Count == 0) return;
            if (_hovered) {
                SelectableObject selectedObject = _listOfSelected[0]; // Выделенный контакт
                if (selectedObject is Contact) {
                    if (_hovered is WagoContact && _wireCreator.StartContact != null) {
                        _wireCreator.EndContact = _hovered.GetComponent<WagoContact>();
                    } else {
                        Select(_hovered);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            if (_hovered) {
                if (_hovered.TryGetComponent(out WirePoint wirePoint)) Select(_hovered);
                else UnselectAll();
                
                if (_hovered is Companent) Select(_hovered);
                else if (_hovered is WagoContact) {
                    WagoContact wagoContact = _hovered.GetComponent<WagoContact>();
                    if (wagoContact.ConnectionWire == null) {
                        _wireCreator.EndContact = wagoContact;
                    } else {
                        if (_wireCreator.StartContact == null) {
                            Wire wire = wagoContact.ConnectionWire;
                            ServiceLocator.Current.Get<SwitchBoxesManager>().ActiveSwichBox.RemoveLineToList(wire);
                            _pointer.Disconnect();
                        } else {
                            Debug.Log("Wago-контакт занят!");
                        }
                    }
                } else if (_hovered is Contact) {
                    Contact contact = _hovered.GetComponent<Contact>();
                    if (contact.ConnectionWire == null) {
                        _wireCreator.StartContact = contact;
                        Select(contact);
                    } else {
                        Debug.Log("Контакт занят!");
                    }
                } else if (_hovered is PrincipalSchemeCompanent) {
                    if (_hovered.GetComponent<PrincipalSchemeCompanent>().IsSelected) Unselect(_hovered);
                    else Select(_hovered);
                } else {
                    Select(_hovered);
                }
            } else {
                UnselectAll();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (_listOfSelected.Count == 0) return;
            if (_hovered) {
                SelectableObject selectedObject = _listOfSelected[0]; // Выделенный контакт
                if (selectedObject is Contact) {
                    if (_hovered is WagoContact && _wireCreator.StartContact != null) {
                        _wireCreator.EndContact = _hovered.GetComponent<WagoContact>();
                    } else {
                        Select(_hovered);
                    }
                }
            }
        }
    }

    void Select(SelectableObject selectableObject) {
        if (!_listOfSelected.Contains(selectableObject)) {
            _listOfSelected.Add(selectableObject);
            if (_trainingModeController.CurrentStatus == TrainingModeStatus.Demonstration) {
                if (selectableObject is Companent) {
                    selectableObject.GetComponent<Companent>().Action();
                }  
            } else {
                selectableObject.Select();
            }  
        }
    }

    public void Unselect(SelectableObject selectableObject) {
        Debug.Log("Unselect");
        if (_listOfSelected.Contains(selectableObject)) {
            _listOfSelected.Remove(selectableObject);
        }
        selectableObject.Unselect();
    }

    void UnselectAll() {
        if (_listOfSelected.Count == 0) return;
        foreach (var iSelected in _listOfSelected) {
            iSelected.Unselect();
        }
        _listOfSelected.Clear();
        _currentSelectionState = SelectionState.Other;
    }

    private void UnhowerCurrent() {
        if (_hovered) {
            _hovered.OnUnhover();
            _hovered = null;
        }
    }

    public WagoClip GetSelectionWagoClip() {
        if (_listOfSelected.Count > 0 && _listOfSelected[0] is WagoClip) {
            return (WagoClip)_listOfSelected[0];
        }
        else {
            return null;
        }
    }
}
