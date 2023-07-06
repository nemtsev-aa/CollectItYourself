using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum CreateType {
    DragDrop,
    DoubleClick,
    OneClick
}

public class WireCreator : MonoBehaviour {
    public CreateType CurrentType = CreateType.OneClick;
    public SwitchBoxManager SwitchBoxManager;
    public Wire WirePrefab;
    public Contact StartContact;
    public WagoContact EndContact;

    private Management _management;
    private Vector3 _mousePosition;
    private LineRenderer _lineRender;
    private Dictionary<string, WagoContact> freeWagoContacts = new Dictionary<string, WagoContact>();

    public void Initialize(Management management) {
        _management = management;
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
            Vector3 mousePosition = Input.mousePosition; // Получаем текущие координаты мыши на экране
            mousePosition.z = transform.position.z - Camera.main.transform.position.z; // Устанавливаем z-координату так, чтобы объект оставался на плоскости x0y
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition); // Преобразуем координаты мыши из экранных в мировые
            _mousePosition = new Vector3(worldPosition.x, worldPosition.y, transform.position.z); // Фиксируем положение мыши на плоскости x0y

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
                    if (FindFreeWagoContactsFromActiveSwitchBox() == 0) {
                        Debug.Log("Нет свободных Wago - зажимов!");
                    }
                    else {
                        Debug.Log("Свободных Wago-зажимов: " + freeWagoContacts.Count);
                        SelectFreeWagoContacts(true);
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
                    if (FindFreeWagoContactsFromActiveSwitchBox() == 0) {
                        Debug.Log("Нет свободных Wago-зажимов!");
                    }
                    else {
                        WagoContact freeWagoContact = GetFreeWagoContact();
                        if (freeWagoContact != null) {
                            Debug.Log("Свободный Wago-контакт найден");
                            EndContact = freeWagoContact;
                            CreateWire();
                            ResetWireCreator();
                        } else {
                            Debug.LogError("OneClickCreation: Свободный Wago-контакт не найден");
                        }
                    }
                }
                else {
                    Debug.Log("StartContact == null");
                    ResetWireCreator();
                }
            }
        }
    }

    [ContextMenu("FindFreeWagoContactsFromActiveSwitchBox")]
    public int FindFreeWagoContactsFromActiveSwitchBox() {
        //Debug.Log("Поиск свободных Wago-зажимов!");
        SwitchBox activeSB = SwitchBoxManager.ActiveSwichBox;
        foreach (WagoClip iWagoClip in activeSB.WagoClips) {
            foreach (WagoContact iContact in iWagoClip.WagoContacts) {
                if (!iContact.GetConnectionStatus()) {
                    if (!freeWagoContacts.ContainsKey(iWagoClip.Name + "_" + iContact.Name)) {
                        freeWagoContacts.Add(iWagoClip.Name + "_" + iContact.Name, iContact);
                    }
                }
            }
        }
        return freeWagoContacts.Count;
    }

    public WagoContact GetFreeWagoContact() {
        SwitchBox activeSB = SwitchBoxManager.ActiveSwichBox;
        WagoClip activeWC = activeSB.ActiveWagoClip;
        // Приоритет на подключение установлен для контактов активного Wago-зажима
        foreach (WagoContact iWagoContact in activeWC.WagoContacts) {
            if (iWagoContact.ConnectedContact == null) {
                return iWagoContact;
            } 
        }
        // Если активный Wago-зажима не имеет свободных контактов, подключаем по очереди добавления Wago-зажимов в сборку
        return freeWagoContacts.Values.First();
    }

    public List<WagoContact> CreateFreeWagoContactsList(WagoClip wagoClip) {
        return null;
    }

    private void SelectFreeWagoContacts(bool status) {
        Debug.Log("SelectFreeWagoContacts: " + status);
        if (freeWagoContacts.Count > 0) {
            foreach (WagoContact iWagoContact in freeWagoContacts.Values) {
                if (status) {
                    iWagoContact.Select();
                }
                else {
                    iWagoContact.Unselect();
                }
            }
        }
    }

    public void CreateWire() {
        Vector3 centerPosition = Vector3.Lerp(StartContact.transform.position, EndContact.transform.position, 0.5f);
        Wire newWire = Instantiate(WirePrefab, centerPosition, Quaternion.identity); // Создан новый проводник

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
        //newWire.StartContact.ContactPositionChanged += newWire.SetNewPositionStartContact;
        newWire.EndContact = EndContact;
        //newWire.EndContact.ContactPositionChanged += newWire.SetNewPositionEndContact;

        StartContact.ConnectionWire = newWire;
        EndContact.ConnectionWire = newWire;
        EndContact.ConnectedContact = StartContact;
        EndContact.Material = StartContact.Material;
        EndContact.SetMaterial();
        EndContact.AddNewConnectToList();
        EndContact.Unselect();
        freeWagoContacts.Remove(EndContact.ParentWagoClip.Name + "_" + EndContact.Name);

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
        SelectFreeWagoContacts(false);

        StartContact = null;
        EndContact = null;

        _lineRender.SetPosition(0, Vector3.zero);
        _lineRender.SetPosition(1, Vector3.zero);
        _lineRender.enabled = false;
    }
}
