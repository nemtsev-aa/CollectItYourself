using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private Pointer _pointer;
 
    public void Init(Management management) {
        _management = management;
        _lineRender = GetComponent<LineRenderer>();
        _pointer = ServiceLocator.Current.Get<Pointer>();
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
            _mousePosition = ServiceLocator.Current.Get<Pointer>().GetPosition();
           
            _lineRender.enabled = true;
            _lineRender.SetPosition(0, StartContact.transform.position);
            _lineRender.SetPosition(1, _mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && EndContact == null) {
            ResetWireCreator();
        }
        else if (Input.GetMouseButtonUp(0) && EndContact != null) {
            _pointer.Connect();
            CreateWire();
            ResetWireCreator();
        }
    }

    private void DoubleClickCreation() {
        if (Input.GetMouseButtonDown(0)) {
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
                        Debug.Log("Нет свободных Wago - зажимов!");
                    } else {
                        Debug.Log("Свободных Wago-зажимов: " + freeWagoClipCount);
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
                        //Debug.Log("Нет свободных Wago-зажимов!");
                    }
                    else {
                        WagoContact freeWagoContact = activeSB.GetFreeWagoContact();
                        if (freeWagoContact != null) {
                            //Debug.Log("Свободный Wago-контакт найден");
                            EndContact = freeWagoContact;
                            CreateWire();
                            ResetWireCreator();
                        } else {
                            //Debug.LogError("OneClickCreation: Свободный Wago-контакт не найден");
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
        Wire newWire = Instantiate(WirePrefab, centerPosition, Quaternion.identity); // Создан новый проводник
        newWire.StartContact = StartContact;
        newWire.StartContact.ContactPositionChanged += newWire.SetNewPositionStartContact;
        newWire.EndContact = EndContact;
        newWire.EndContact.ContactPositionChanged += newWire.SetNewPositionEndContact;
        
        switch (WirePrefab.Type) {
            case WireType.Straight:
                CreateStraightWire(newWire);
                break;
            case WireType.Poly:
                CreatePolyWire(newWire);
                break;
            case WireType.Bezier:
                CreateBezierWire((BezierWire)newWire);
                break;
        }

        newWire.SwitchBox = SwitchBoxManager.ActiveSwichBox;
        newWire.transform.parent = SwitchBoxManager.ActiveSwichBox.WiresTransform.transform;
        newWire.GetComponent<SelectableObject>().Name = (SwitchBoxManager.ActiveSwichBox.Wires.Count + 1).ToString();
        newWire.ObjectView.Initialization(newWire);
        newWire.ObjectView.ShowName();
        newWire.ObjectView.SetColor(StartContact.Material.color);
        newWire.ObjectView.LineRenderer.numCapVertices = 50;
        

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

    private void CreateBezierWire(BezierWire newWire) {
        if (StartContact != null && EndContact != null) {
            newWire.Init();


            //int _pointsCount = 20;
            //Transform pointsParent = gameObject.transform;
            //foreach (Transform child in newWire.ObjectView.transform) {
            //    if (child.name == "Points") pointsParent = child;
            //}

            //Transform[] points = new Transform[_pointsCount];
            //for (int i = 0; i < _pointsCount; i++) {
            //    Transform newPoint = new GameObject().transform;
            //    newPoint.gameObject.name = "Point" + i;
            //    newPoint.parent = pointsParent;
            //    points[i] = newPoint; 
            //}

            //Transform _startPoint = StartContact.transform;
            //Transform _endPoint = EndContact.transform;

            //Vector3[] _points = new Vector3[_pointsCount];
            //_points[0] = new Vector3(_startPoint.position.x, _startPoint.position.y, 0f);
            //_points[_pointsCount - 1] = new Vector3(_endPoint.position.x, _endPoint.position.y, 0f);

            //float _distance = Vector3.Distance(_startPoint.position, _endPoint.position);

            //Vector3 _point1 = _startPoint.position + _startPoint.right * (_distance / 2f);
            ////_point1 = new Vector3(_point1.x, _point1.y, 0f);
            ////Debug.Log($"_point1 {_point1}");
            //GameObject point1 = new GameObject();
            //point1.transform.position = _point1;

            //Vector3 _point2 = _endPoint.position + _endPoint.right * (_distance / 2f);
            ////_point2 = new Vector3(_point2.x, _point2.y, 0f);
            ////Debug.Log($"_point2 {_point2}");
            //GameObject point2 = new GameObject();
            //point2.transform.position = _point2;

            //for (int i = 1; i < _pointsCount - 1; i++) {
            //    _points[i] = Bezier.GetPoint(_points[0], _point1, _point2, _points[_pointsCount - 1], (float)i / _pointsCount);
            //}

            //for (int i = 0; i < _pointsCount; i++) {
            //    //Debug.Log($"{i} {_points[i]}");
            //    points[i].position = _points[i];
            //}
            //newWire.ObjectView.PathElements = points;
        }
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
