using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum CreateType {
    DragDrop,
    DoubleClick,
    OneClick
}

public class WireCreator : MonoBehaviour, IService {
    public CreateType CurrentType = CreateType.OneClick;
    public SwitchBoxManager SwitchBoxManager;
    public Wire WirePrefab;
    public Contact StartContact;
    public WagoContact EndContact;

    private Management _management;
    private Vector3 _mousePosition;
    private LineRenderer _lineRender;

    public void Init() {
        _management = ServiceLocator.Current.Get<Management>();
        _lineRender = GetComponent<LineRenderer>();
    }

    private void Update() {
        if (SwitchBoxManager.ActiveSwichBox != null) {
            switch (CurrentType) {
                case CreateType.DragDrop:
                    DragDropCreation();
                    break;
                case CreateType.DoubleClick:
                    DoubleClickCreation();
                    break;
                case CreateType.OneClick:
                    OneClickCreation();
                    break;
                default:
                    break;
            }
        }
    }

    private void DragDropCreation() {
        if (Input.GetMouseButtonDown(0) && StartContact == null) {
            _lineRender.enabled = false;
        }
        else if (Input.GetMouseButton(0) && StartContact != null) {
            Vector3 mousePosition = Input.mousePosition; // �������� ������� ���������� ���� �� ������
            mousePosition.z = transform.position.z - Camera.main.transform.position.z; // ������������� z-���������� ���, ����� ������ ��������� �� ��������� x0y
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition); // ����������� ���������� ���� �� �������� � �������
            _mousePosition = new Vector3(worldPosition.x, worldPosition.y, transform.position.z); // ��������� ��������� ���� �� ��������� x0y

            _lineRender.enabled = true;
            _lineRender.SetPosition(0, StartContact.transform.position);
            _lineRender.SetPosition(1, _mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && EndContact == null) {
            ResetWireCreator();
        }
        else if (Input.GetMouseButtonUp(0) && EndContact != null) {
            CreateWire();
            ResetWireCreator();
        }
    }

    private void DoubleClickCreation() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("GetMouseButtonDown");
            if (StartContact != null) {
                if (EndContact != null) {
                    //Debug.Log("StartContact != null, EndContact != null");
                    CreateWire();
                    ResetWireCreator();
                } else {
                    //Debug.Log("StartContact != null, EndContact == null");
                    SwitchBox activeSB = SwitchBoxManager.ActiveSwichBox;
                    int freeWagoClipCount = activeSB.FindFreeWagoContacts();
                    if (freeWagoClipCount == 0) {
                        Debug.Log("��� ��������� Wago - �������!");
                    } else {
                        Debug.Log("��������� Wago-�������: " + freeWagoClipCount);
                        activeSB.SelectFreeWagoContacts(true);
                    }
                }
            }
            else {
                Debug.Log("StartContact == null");
                ResetWireCreator();
            }
        }
    }

    private void OneClickCreation() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("GetMouseButtonDown");
            if (StartContact != null) {
                if (EndContact == null) {
                    //Debug.Log("StartContact != null, EndContact == null");
                    SwitchBox activeSB = SwitchBoxManager.ActiveSwichBox;
                    int freeWagoClipCount = activeSB.FindFreeWagoContacts();
                    if (freeWagoClipCount == 0) {
                        //Debug.Log("��� ��������� Wago-�������!");
                    }
                    else {
                        WagoContact freeWagoContact = activeSB.GetFreeWagoContact();
                        if (freeWagoContact != null) {
                            //Debug.Log("��������� Wago-������� ������");
                            EndContact = freeWagoContact;
                            CreateWire();
                            ResetWireCreator();
                        } else {
                            //Debug.LogError("OneClickCreation: ��������� Wago-������� �� ������");
                        }
                    }
                }
                else {
                    //Debug.Log("StartContact == null");
                    ResetWireCreator();
                }
            }
        }
    }

    public void CreateWire() {
        Vector3 centerPosition = Vector3.Lerp(StartContact.transform.position, EndContact.transform.position, 0.5f);
        Wire newWire = Instantiate(WirePrefab, centerPosition, Quaternion.identity); // ������ ����� ���������

        switch (WirePrefab.Type) {
            case WireType.Straight:
                CreateStraightWire(newWire);
                break;
            case WireType.Poly:
                CreatePolyWire(newWire);
                break;
            case WireType.Bezier:
                CreateBezierWire(newWire);
                break;
        }

        newWire.SwitchBox = SwitchBoxManager.ActiveSwichBox;
        newWire.transform.parent = SwitchBoxManager.ActiveSwichBox.WiresTransform.transform;
        newWire.GetComponent<SelectableObject>().Name = (SwitchBoxManager.ActiveSwichBox.Wires.Count + 1).ToString();
        newWire.ObjectView.Initialization(newWire);
        newWire.ObjectView.ShowName();
        newWire.ObjectView.SetColor(StartContact.Material.color);
        newWire.ObjectView.LineRenderer.numCapVertices = 50;
        newWire.StartContact = StartContact;
        newWire.StartContact.ContactPositionChanged += newWire.SetNewPositionStartContact;
        newWire.EndContact = EndContact;
        newWire.EndContact.ContactPositionChanged += newWire.SetNewPositionEndContact;

        StartContact.ConnectionWire = newWire;
        EndContact.ConnectionWire = newWire;
        EndContact.ConnectedContact = StartContact;
        EndContact.Material = StartContact.Material;
        EndContact.SetMaterial();
        EndContact.AddNewConnectToList();
        EndContact.Unselect();
        SwitchBoxManager.ActiveSwichBox.RemoveWagoContactFromFreeList(EndContact);

        SwitchBoxManager.ActiveSwichBox.AddNewLineFromList(newWire);
        newWire.GenerateMeshCollider();
        newWire.GenerateMeshCollider();

        //StartContact = null;
        //EndContact = null;
        //_lineRender.enabled = false;
    }

    private void CreateStraightWire(Wire newWire) {
        if (StartContact != null && EndContact != null) {
            Transform[] points = new Transform[] { StartContact.transform, EndContact.transform };
            newWire.ObjectView.PathElements = points;
        }
    }

    private void CreatePolyWire(Wire newWire) {
        if (StartContact != null && EndContact != null) {
            Transform[] points = newWire.ObjectView.PathElements;
            points[0].position = StartContact.transform.position;
            points[1].position = Vector3.Lerp(StartContact.transform.position, EndContact.transform.position, 0.25f);
            points[2].position = Vector3.Lerp(StartContact.transform.position, EndContact.transform.position, 0.5f);
            points[3].position = Vector3.Lerp(StartContact.transform.position, EndContact.transform.position, 0.75f);
            points[4].position = EndContact.transform.position;
        }
    }

    private void CreateBezierWire(Wire newWire) {

    }

    private int LineVariant(Transform start, Transform end) {
        if (end.position.y > start.position.y) {
            return 1;
        }
        else if (end.position.y < start.position.y) {
            return 2;
        }
        else {
            return 3;
        }
    }

    public void ResetWireCreator() {
        if (StartContact == null) return;

        StartContact.Unselect();
        SwitchBoxManager.ActiveSwichBox.SelectFreeWagoContacts(false);

        StartContact = null;
        EndContact = null;

        _lineRender.SetPosition(0, Vector3.zero);
        _lineRender.SetPosition(1, Vector3.zero);
        _lineRender.enabled = false;
    }
}
