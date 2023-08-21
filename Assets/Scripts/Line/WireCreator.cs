using System.Collections.Generic;
using UnityEngine;

public enum CreateType {
    DragDrop,
    DoubleClick,
    OneClick
}

public class WireCreator : MonoBehaviour, IService {
    public CreateType CurrentType = CreateType.OneClick;
    
    public Wire WirePrefab;
    public Contact StartContact;
    public WagoContact EndContact;

    private SwitchBoxesManager _switchBoxesManager;
    private Management _management;
    private Vector3 _mousePosition;
    private LineRenderer _lineRender;
    private Pointer _pointer;
 
    public void Init(Management management, SwitchBoxesManager switchBoxesManager, Pointer pointer) {
        _management = management;
        _switchBoxesManager = switchBoxesManager;
        _pointer = pointer;
        _lineRender = GetComponent<LineRenderer>();
    }

    private void Update() {
        if (_switchBoxesManager != null) {
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
                    SwitchBox activeSB = _switchBoxesManager.ActiveSwichBox;
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
                    SwitchBox activeSB = _switchBoxesManager.ActiveSwichBox;
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
        SwitchBox activeSwichBox = _switchBoxesManager.ActiveSwichBox;
        newWire.SwitchBox = activeSwichBox;
        newWire.transform.parent = activeSwichBox.WiresTransform.transform;
        newWire.GetComponent<SelectableObject>().Name = (activeSwichBox.Wires.Count + 1).ToString();
        newWire.ObjectView.Init(newWire);
        newWire.ObjectView.ShowName();
        newWire.ObjectView.SetColor(StartContact.Material.color);
        newWire.ObjectView.LineRenderer.numCapVertices = 50;

        StartContact.ConnectionWire = newWire;
        EndContact.ConnectionWire = newWire;
        EndContact.ConnectedContact = StartContact;
        EndContact.SetMaterial(StartContact.Material);
        EndContact.SetMaterial();
        EndContact.AddNewConnectToList();
        EndContact.Unselect();

        activeSwichBox.RemoveWagoContactFromFreeList(EndContact);
        activeSwichBox.AddNewLineFromList(newWire);

        newWire.GenerateMeshCollider();
        newWire.GenerateMeshCollider();

        ElectricFieldSettings settings = StartContact.GetElectricFieldSettings();
        if (settings.ElectricFieldMaterial != null) {
            if (newWire.ElectricFieldMovingView != null) {
                newWire.SetElectricFieldSettings(settings);                     // Настраиваем внешний вид электрическго поля на проводе

                EndContact.ParentWagoClip.SetElectricFieldSettings(settings);   // Настраиваем внешний вид электрического поля на Wago-зажиме
                if (StartContact.GetParentCompanent().Type == CompanentType.Input) {
                    EndContact.ParentWagoClip.GetElectricFieldMovingView(EndContact).SwichDirection();
                    EndContact.ParentWagoClip.GetCommomBusElectricFieldMovingView().SwichDirection();
                }
                else if (StartContact.GetParentCompanent().Type == CompanentType.Selector) {
                    EndContact.ParentWagoClip.GetElectricFieldMovingView(EndContact).SwichDirection();
                    EndContact.ParentWagoClip.GetCommomBusElectricFieldMovingView().SwichDirection();
                    newWire.ElectricFieldMovingView.SwichDirection();
                }
                //else {
                //    EndContact.SetElectricFieldMovingDirection(StartContact);
                //}
            }
            else {
                Debug.Log($"{newWire.Name} ElectricFieldMovingView не найден!");
            }
        }
        else {
            Debug.Log("WireCreator: CreateWire ошибка при получении настроек електрического поля");
        }
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
        _switchBoxesManager.ActiveSwichBox.SelectFreeWagoContacts(false);

        StartContact = null;
        EndContact = null;

        _lineRender.SetPosition(0, Vector3.zero);
        _lineRender.SetPosition(1, Vector3.zero);
        _lineRender.enabled = false;
    }
}
