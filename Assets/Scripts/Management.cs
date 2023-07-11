using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] private Camera _camera;
    private List<SelectableObject> _listOfSelected = new List<SelectableObject>();
    private SelectionState _currentSelectionState;
    private WireCreator _wireCreator;
    private SelectableObject _hovered;
    private bool _isOverUI;

    public void Init() {
        _wireCreator = ServiceLocator.Current.Get<WireCreator>();
    }

    void Update() {
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
        else {
            UnhowerCurrent();
        }

        if (Input.GetMouseButtonDown(0)) {
            if (_hovered) {
                if (_hovered.TryGetComponent(out WirePoint wirePoint)) {
                    Select(_hovered);
                } else {
                    UnselectAll();
                }
                
                if (_hovered is Companent) {
                    Select(_hovered);
                }
                else if (_hovered is WagoContact) {
                    WagoContact wagoContact = _hovered.GetComponent<WagoContact>();
                    if (wagoContact.ConnectionWire == null) {
                        _wireCreator.EndContact = wagoContact;
                    } else {
                        if (_wireCreator.StartContact == null) {
                            Wire wire = wagoContact.ConnectionWire;
                            ServiceLocator.Current.Get<SwitchBoxManager>().ActiveSwichBox.RemoveLineToList(wire);
                            
                        } else {
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
                else if (_hovered is WagoClip) {
                    Select(_hovered);
                }
                else if (_hovered is WirePoint) {
                    Select(_hovered);
                }
                else if (_hovered is PrincipalSchemeCompanent) {
                    if (_hovered.GetComponent<PrincipalSchemeCompanent>().IsSelected) {
                        Unselect(_hovered);
                    }
                    else {
                        Select(_hovered);
                    }
                } else {
                    Select(_hovered);
                }
            }
            else {
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

    //void CreatingFrame() {

    //    if (Input.GetMouseButtonDown(0)) {
    //        _frameStart = Input.mousePosition; // Фиксируем начальное положение мыши
    //    }

    //    if (Input.GetMouseButton(0)) {

    //        _frameEnd = Input.mousePosition; // Обновляем конечное положение мыши всё время пока кнопка LeftMouse нажата

    //        Vector2 min = Vector2.Min(_frameStart, _frameEnd);
    //        Vector2 max = Vector2.Max(_frameStart, _frameEnd);
    //        Vector2 size = max - min; // Размер выделенной области

    //        if (size.magnitude > 10) { // Убеждаемся в намерении пользователя рисовать рамку

    //            FrameImage.enabled = true; // Отображаем рамку
    //            FrameImage.rectTransform.anchoredPosition = min; // Положение рамки
    //            FrameImage.rectTransform.sizeDelta = size;  // Размеры рамки

    //            Rect rect = new Rect(min, size); // Прямоугольная область для выделения объектов

    //            UnselectAll(); // Снимаем выделение со всех объектов
    //            Companent[] allCompanent = FindObjectsOfType<Companent>(); // Массив всех юнитов на сцене
    //            for (int i = 0; i < allCompanent.Length; i++) {
    //                Vector2 screenPosition = Camera.WorldToScreenPoint(allCompanent[i].transform.position); // Проецируем позиции объектов на плоскость экрана
    //                if (rect.Contains(screenPosition)) {
    //                    Select(allCompanent[i]); // Выделяем объекты, находящиеся внутри рамки
    //                }
    //            }
    //            CurrentSelectionState = SelectionState.Frame;
    //        }
    //    }
    //}

    void Select(SelectableObject selectableObject) {
        if (!_listOfSelected.Contains(selectableObject)) {
            _listOfSelected.Add(selectableObject);
            selectableObject.Select();
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
}
