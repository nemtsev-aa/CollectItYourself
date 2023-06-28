using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBox : MonoBehaviour {
    [Header("Switching Parametrs")]
    public Dictionary<string, ConnectionData> ErrorConnects = new();
    [field: SerializeField] public bool isOpen { get; private set; }
    [field: SerializeField] public SingleSwitchingResult Result = new SingleSwitchingResult();

    [Header("Data")]
    public SwitchBoxData SwitchBoxData;
    public List<Transform> Slots = new List<Transform>();
    [SerializeField] private Transform CompanentsTransform;
    public List<Companent> Companents = new List<Companent>();
    [Header("Clips")]
    public Transform WagoClipsTransform;
    public List<WagoClip> WagoClips = new List<WagoClip>();
    [Header("Wires")]
    public Transform WiresTransform;
    public List<Wire> Wires = new List<Wire>();

    private Stopwatch _stopwatch;

    public void Initialized(Stopwatch stopwatch) {
        _stopwatch = stopwatch;
    }

    public void AddNewWagoClipToList(WagoClip wago) {
        WagoClips.Add(wago);
    }

    public void RemoveWagoClipFromList(WagoClip wago) {
        if (WagoClips.Contains(wago)) {
            WagoClips.Remove(wago);
            Destroy(wago.gameObject);
        }
    }

    public void AddNewLineFromList(Wire line) {
        Wires.Add(line);
    }

    public void RemoveLineToList(Wire line) {
        Wires.Remove(line);
        line.StartContact.ConnectionWire = null;
        Destroy(line.gameObject);
    }

    public void RemoveCompanent(Companent companent) {
        if (Companents.Contains(companent)) {
            Companents.Remove(companent);
            Destroy(companent.gameObject);
        }
    }

    public void ShowErrorConnections() {
        if (ErrorConnects.Count == 0) return;

        foreach (KeyValuePair<string, ConnectionData> iConnection in ErrorConnects) {
            Contact errorContact = FindContactInCompanents(iConnection.Value);

            if (errorContact) {
                errorContact.StartBlink();
                errorContact.Select();
            }
        }
    }

    public void SetTimeValue() {
        _stopwatch.SetTimeValue(Result.SwitchingTimeValue);
    }

    public void GetTimeValue() {
        Result.SwitchingTimeValue = _stopwatch.GetTimeValue();
    }

    #region �hecking�onnections
    [ContextMenu("�hecking�onnections")]
    public int �hecking�onnections() {
        if (WagoClips.Count == 0) {
            Debug.Log("����� �� �������!");
            return 100;
        }

        _stopwatch.SetStatus(false);

        float errorsProcentage = 0; // ������� ������ � ������
        int allContactsCount = 0; // ����� ���������� ��������� � �������
        int allErrorsCount; // ���������� ��������� ������������ � �������

        // ��������� ������ �����
        foreach (WagoClip iWagoClip in WagoClips) {
            List<ConnectionData> connectionsResult = iWagoClip.Connections; // ������ � ������������ ����������� � �� ���������
            if (iWagoClip.Connections.Count > 0) {
                List<ConnectionData> connectionsAnswer = FindConnectionInAnswer(iWagoClip.Connections[0]); // ������������ ����� Wago-������ � ������ �� ������� ������������� �������� 
                if (connectionsAnswer != null) {
                    allContactsCount += connectionsAnswer.Count; // ���������� ��������� � ����������� Wago-�����e
                    List<ConnectionData> errorConnectsList = CompareLists(connectionsResult, connectionsAnswer); // ������ ��������� �����������
                    int errorCount = errorConnectsList.Count; // ���������� ������ � ����������� Wago-�����e
                    if (ErrorConnects.Count >= 0) { // ������� � ���������� �������� �� ����
                        foreach (ConnectionData iError in errorConnectsList) {
                            string newError = iError.CompanentName.ToString() + "_" + iError.ContactType.ToString();
                            if (!ErrorConnects.ContainsKey(newError)) {
                                ErrorConnects.Add(newError, iError); // ��������� ������� ��������� ����������� ����� �������
                            }
                        }
                    }
                }
            }
        }

        allErrorsCount = ErrorConnects.Count; // ����� ���������� ������

        Result.TaskName = SwitchBoxData.Task.Name;
        Result.SwitchBoxNumer = SwitchBoxData.PartNumber;
        Result.ErrorCountText = allErrorsCount + "/" + allContactsCount;

        if (ErrorConnects.Count > 0) {
            if (!CreateErrorsList()) {
                Debug.Log("������ �������� ������ ������ �� �������!" + this.name);
            }
        }

        Result.SwitchingTimeValue = _stopwatch.GetTimeValue();
        Result.SwitchingTimeText = _stopwatch.GetTimeText();

        gameObject.SetActive(false);

        if (allErrorsCount > 0) {
            ShowErrorConnections();
            errorsProcentage = (allErrorsCount / allContactsCount) * 100;
            Debug.Log("������ � ������: " + allErrorsCount + "/" + allContactsCount);
            EventBus.Instance.IncorrectChecked?.Invoke(Result);
        }
        else {
            Debug.Log("������ ������!");
            EventBus.Instance.CorrectChecked?.Invoke(Result);
        }
       
        return (int)errorsProcentage;
    }

    private List<ConnectionData> FindConnectionInAnswer(ConnectionData connectionData) {
        Answer answer = SwitchBoxData.Answer; // ������ ������� �����������
        List<AnswerData> answerDatas = answer.AnswerDataList; // ������ Wago-�������

        foreach (AnswerData iWagoAnswer in answerDatas) {
            List<ConnectionData> connectionDatas = iWagoAnswer.Connections; // ������ ������ � ������������ � Wago-������
            if (connectionDatas.Contains(connectionData)) { // ������ ����������� �������� ������� ��������� � �������
                return connectionDatas;
            }
        }
        return null;
    }

    private List<ConnectionData> CompareLists(List<ConnectionData> connectionsResult, List<ConnectionData> connectionsAnswer) {
        if (connectionsResult.Count != connectionsAnswer.Count) {
            Debug.Log("������ ����� ��������� �����");
        }

        connectionsResult.Sort((x, y) => x.CompanentName.CompareTo(y.CompanentName));
        connectionsAnswer.Sort((x, y) => x.CompanentName.CompareTo(y.CompanentName));

        List<ConnectionData> errorConnectionList = new();
        for (int i = 0; i < connectionsResult.Count; i++) {
            if (connectionsResult[i].CompanentName != connectionsAnswer[i].CompanentName ||
                connectionsResult[i].CompanentType != connectionsAnswer[i].CompanentType ||
                connectionsResult[i].ContactType != connectionsAnswer[i].ContactType) {
                errorConnectionList.Add(connectionsResult[i]);
            }
        }

        return errorConnectionList;
    }

    private Contact FindContactInCompanents(ConnectionData connection) {
        foreach (Companent iCompanent in Companents) {
            if (iCompanent.Name == connection.CompanentName && iCompanent.Type == connection.CompanentType) {
                //Debug.Log(connection.CompanentName);

                foreach (Contact iContact in iCompanent.Contacts) {
                    //Debug.Log(connection.ContactType);
                    if (iContact.ContactType == connection.ContactType) {
                        //Debug.Log(connection.ContactType);
                        return iContact;
                    }
                }
            }
        }
        return null;
    }

    private bool CreateErrorsList() {
        foreach (KeyValuePair<string, ConnectionData> iConnection in ErrorConnects) {
            if (!Result.ErrorList.Contains(iConnection.Value)) {
                Result.ErrorList.Add(iConnection.Value);
            }
        }
        return ErrorConnects.Count != Result.ErrorList.Count ? true : false;
    }

    #endregion


}
