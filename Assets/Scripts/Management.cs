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
    WagoContactSelected
}

public class Management : MonoBehaviour {
    public Camera Camera;
    public SelectableObject Hovered;
    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();
    public SelectionState CurrentSelectionState;

    [Header("WireCreator")]
    public WireCreator WireCreator;

    public SwitchBoxData SwitchBoxData;
    public SwitchBoxManager SwitchBoxManager;
    public ActionState ActionState;

    private bool _isOverUI;

    [ContextMenu("ShowTask")]
    public void ShowTask(Task selectionTask) {
        Debug.Log("Management_ShowTask");
        SwitchBoxData = selectionTask.TaskData[0].SwitchBoxsData;
        SwitchBox newBox = SwitchBoxManager.CreateSwichBox(SwitchBoxData);

        ActionState.ShowSwitchBox(newBox);
    }

    [ContextMenu("HideTask")]
    public void ClearTask(Task hideTask) {
        SwitchBoxData = null;
        SwitchBoxManager.RemoveSwichBoxFromList(SwitchBoxManager.ActiveSwichBox);
    }

    void Update() {
        _isOverUI = EventSystem.current.IsPointerOverGameObject();

        Ray ray = Camera.ScreenPointToRay(Input.mousePosition); // ��� �� ������ � ����� ������������ ������� ���� �� ������
        Debug.DrawLine(ray.origin, ray.direction * 10f, Color.red); // ������������ ����

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
                }
                else {
                    Hovered = hitSelectable;
                    Hovered.OnHover();
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
            if (Hovered) {
                UnselectAll();
                if (Hovered is Companent) {
                    Select(Hovered);
                }
                else if (Hovered is WagoContact) {
                    WagoContact iContact = Hovered.GetComponent<WagoContact>();
                    if (iContact.ConnectionWire == null) {
                        Select(Hovered);
                    }
                    else {
                        SwitchBoxManager.ActiveSwichBox.RemoveLineToList(iContact.ConnectionWire);
                        iContact.RemoveConnectFromList();
                        iContact.ResetMaterial();
                    }
                }
                else if (Hovered is Contact) {
                    if (Hovered.GetComponent<Contact>().ConnectionWire == null) {
                        WireCreator.StartContact = Hovered.GetComponent<Contact>();
                        Select(Hovered);
                    }
                    else {
                        Debug.Log("������� �����!");
                    }
                }
                else if (Hovered is WagoClip) {
                    Select(Hovered);
                }
                else if (Hovered is PrincipalSchemeCompanent) {
                    if (Hovered.GetComponent<PrincipalSchemeCompanent>().IsSelected) {
                        Unselect(Hovered);
                    }
                    else {
                        Select(Hovered);
                    }
                }
            }
            else {
                UnselectAll();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (ListOfSelected.Count == 0) return;
            if (Hovered) {
                SelectableObject selectedObject = ListOfSelected[0]; // ���������� �������
                if (selectedObject is Contact) {
                    if (Hovered is WagoContact && WireCreator.StartContact != null) {
                        WireCreator.EndContact = Hovered.GetComponent<WagoContact>();
                    } else {
                        Select(Hovered);
                    }
                }
            }
        }
    }

    //void CreatingFrame() {

    //    if (Input.GetMouseButtonDown(0)) {
    //        _frameStart = Input.mousePosition; // ��������� ��������� ��������� ����
    //    }

    //    if (Input.GetMouseButton(0)) {

    //        _frameEnd = Input.mousePosition; // ��������� �������� ��������� ���� �� ����� ���� ������ LeftMouse ������

    //        Vector2 min = Vector2.Min(_frameStart, _frameEnd);
    //        Vector2 max = Vector2.Max(_frameStart, _frameEnd);
    //        Vector2 size = max - min; // ������ ���������� �������

    //        if (size.magnitude > 10) { // ���������� � ��������� ������������ �������� �����

    //            FrameImage.enabled = true; // ���������� �����
    //            FrameImage.rectTransform.anchoredPosition = min; // ��������� �����
    //            FrameImage.rectTransform.sizeDelta = size;  // ������� �����

    //            Rect rect = new Rect(min, size); // ������������� ������� ��� ��������� ��������

    //            UnselectAll(); // ������� ��������� �� ���� ��������
    //            Companent[] allCompanent = FindObjectsOfType<Companent>(); // ������ ���� ������ �� �����
    //            for (int i = 0; i < allCompanent.Length; i++) {
    //                Vector2 screenPosition = Camera.WorldToScreenPoint(allCompanent[i].transform.position); // ���������� ������� �������� �� ��������� ������
    //                if (rect.Contains(screenPosition)) {
    //                    Select(allCompanent[i]); // �������� �������, ����������� ������ �����
    //                }
    //            }
    //            CurrentSelectionState = SelectionState.Frame;
    //        }
    //    }
    //}

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
        if (ListOfSelected.Count == 0) return;

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
