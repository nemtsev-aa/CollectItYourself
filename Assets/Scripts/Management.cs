using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SelectionState {
    ClipsSelected,
    Frame,
    Other,
    CompanentSelected,
    ContactSelected
}

public class Management : MonoBehaviour {
    public Camera Camera;
    public SelectableObject Hovered;
    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();
    public SelectionState CurrentSelectionState;

    [Header("Frame")]
    public Image FrameImage;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;

    [Header("WireCreator")]
    public WireCreator WireCreator;

    private bool _isOverUI;

    public SwitchBoxData SwitchBoxData;
    public SwitchBoxManager SwitchBoxManager;

    [ContextMenu("Test")]
    public void Test() {
        SwitchBoxManager.CreateSwichBox(SwitchBoxData);
    }

    void Update() {
        _isOverUI = EventSystem.current.IsPointerOverGameObject();

        Ray ray = Camera.ScreenPointToRay(Input.mousePosition); // Луч из камеры в точку расположения курсора мыши на экране
        Debug.DrawLine(ray.origin, ray.direction * 10f, Color.red); // Визуализация луча

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            SelectableCollider selectable = hit.collider.GetComponent<SelectableCollider>();
            if (selectable) {
                SelectableObject hitSelectable = selectable.SelectableObject;
                if (Hovered) {
                    if (Hovered != hitSelectable) {
                        Hovered.OnUnhover();
                        Hovered = hitSelectable;
                        Hovered.OnHover();
                    }
                } else {
                    Hovered = hitSelectable;
                    Hovered.OnHover();
                }
            } else {
                UnhowerCurrent();
            }
        } else {
            UnhowerCurrent();
        }

        if (Input.GetMouseButtonDown(0)) {
            if (Hovered) {
                if (Input.GetKey(KeyCode.LeftControl)) { // Добавляем объект в группу сочитанием клавиш LeftCtr+LeftMouse
                    Select(Hovered);
                } else if (Input.GetKey(KeyCode.LeftAlt)) { // Удаляем объект из группы сочитанием клавиш LeftAlt+LeftMouse
                    Unselect(Hovered);
                }
            } else {
                if (!_isOverUI) {
                    if (ListOfSelected.Count == 1) {
                        Unselect(ListOfSelected[0]);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (Hovered) {
                if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftAlt)) {
                    UnselectAll();
                    Select(Hovered);    // Выделяем одиночный объект
                }
            }
        }

        if (CurrentSelectionState == SelectionState.ClipsSelected) {
            if (Input.GetMouseButtonDown(0)) {
                if (hit.collider) {
                    //if (hit.collider.tag == "Clips" && !_isOverUI) {
                        //foreach (var iSelectedItem in ListOfSelected) {
                        //     // Задаём пункт назначения для перемещения юнитов
                        //}
                    //}
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) { // Очищаем список выделенных объектов нажатием на RightMouse
            UnselectAll();
        }

        if (Input.GetMouseButtonUp(0)) {
            FrameImage.enabled = false; // Скрываем рамку
            if (ListOfSelected.Count == 0) {
                CurrentSelectionState = SelectionState.Other;
            }
            else if (ListOfSelected.Count == 1) {
                GameObject selectedObject = ListOfSelected[0].gameObject;
                if (selectedObject.GetComponent<Companent>()) {
                    //Debug.Log("Выделен компанент");
                    CurrentSelectionState = SelectionState.CompanentSelected;
                } else if (selectedObject.TryGetComponent(out WagoContact wagoContact)) {
                    Debug.Log("Выделен Wago-контакт");
                    CurrentSelectionState = SelectionState.ContactSelected;
                    WireCreator.EndContact = wagoContact;
                } else if (selectedObject.TryGetComponent(out Contact contact)) {
                    Debug.Log("Выделен контакт");
                    CurrentSelectionState = SelectionState.ContactSelected;
                    WireCreator.StartContact = contact;
                }
            } else {
                if (ListOfSelected.Count > 1) {
                    //CurrentSelectionState = SelectionState.ClipsSelected;
                }
            }
        }
    }

    void CreatingFrame() {

        if (Input.GetMouseButtonDown(0)) {
            _frameStart = Input.mousePosition; // Фиксируем начальное положение мыши
        }

        if (Input.GetMouseButton(0)) {

            _frameEnd = Input.mousePosition; // Обновляем конечное положение мыши всё время пока кнопка LeftMouse нажата

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);
            Vector2 size = max - min; // Размер выделенной области

            if (size.magnitude > 10) { // Убеждаемся в намерении пользователя рисовать рамку

                FrameImage.enabled = true; // Отображаем рамку
                FrameImage.rectTransform.anchoredPosition = min; // Положение рамки
                FrameImage.rectTransform.sizeDelta = size;  // Размеры рамки

                Rect rect = new Rect(min, size); // Прямоугольная область для выделения объектов

                UnselectAll(); // Снимаем выделение со всех объектов
                Companent[] allCompanent = FindObjectsOfType<Companent>(); // Массив всех юнитов на сцене
                for (int i = 0; i < allCompanent.Length; i++) {
                    Vector2 screenPosition = Camera.WorldToScreenPoint(allCompanent[i].transform.position); // Проецируем позиции объектов на плоскость экрана
                    if (rect.Contains(screenPosition)) {
                        Select(allCompanent[i]); // Выделяем объекты, находящиеся внутри рамки
                    }
                }
                CurrentSelectionState = SelectionState.Frame;
            }
        }
    }

    void Select(SelectableObject selectableObject) {
        if (!ListOfSelected.Contains(selectableObject)) {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    public void Unselect(SelectableObject selectableObject) {
        if (ListOfSelected.Contains(selectableObject)) {
            ListOfSelected.Remove(selectableObject);
        }
        selectableObject.Unselect();
    }

    void UnselectAll() {
        foreach (var iSelected in ListOfSelected) {
            iSelected.Unselect();
        }
        ListOfSelected.Clear();
        CurrentSelectionState = SelectionState.Other;
    }

    private void UnhowerCurrent() {
        if (Hovered) {
            Hovered.OnUnhover();
            Hovered = null;
        }
    }
}
