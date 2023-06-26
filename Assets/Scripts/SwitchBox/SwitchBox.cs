using System.Collections.Generic;
using UnityEngine;


public class SwitchBox : MonoBehaviour {
    public SwitchBoxData SwitchBoxData;
    public bool isOpen;
    public List<Transform> Slots = new List<Transform>();

    public Transform CompanentsTransform;
    public List<Companent> Companents = new List<Companent>();
    public Transform WagoClipsTransform;
    public List<WagoClip> WagoClips = new List<WagoClip>();
    public Transform WiresTransform;
    public List<Wire> Wires = new List<Wire>();

    private List<ConnectionData> _errorConnects = new List<ConnectionData>();

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

    public void RemoveCompanent(Companent companent) {
        if (Companents.Contains(companent)) {
            Companents.Remove(companent);
            Destroy(companent.gameObject);
        }
    }
    [ContextMenu("�hecking�onnections")]
    public int �hecking�onnections() {
        if (WagoClips.Count == 0) {
            Debug.Log("����� �� �������!");
            return 100;
        }

        float errorsProcentage = 0; // ������� ������ � ������
        int allContactsCount = 0; // ����� ���������� ��������� � �������
        int allErrorsCount = 0; // ���������� ��������� ������������ � �������

        // ��������� ������ �����
        foreach (WagoClip iWagoClip in WagoClips) {
            List<ConnectionData> connectionsResult = iWagoClip.Connections; // ������ � ������������ ����������� � �� ���������
            if (iWagoClip.Connections.Count > 0 ) {
                List<ConnectionData> connectionsAnswer = FindConnectionInAnswer(iWagoClip.Connections[0]); // ������������ ����� Wago-������ � ������ �� ������� ������������� �������� 
                if (connectionsAnswer != null) {
                    allContactsCount += connectionsAnswer.Count; // ���������� ��������� � ����������� Wago-�����e
                    List<ConnectionData> errorConnectsList = CompareLists(connectionsResult, connectionsAnswer); // ������ ��������� �����������

                    if (errorConnectsList.Count > 0) {
                        int errorCount = errorConnectsList.Count; // ���������� ������ � ����������� Wago-�����e
                        _errorConnects.AddRange(errorConnectsList); // ����� ������ ��������� �����������
                    }
                }
            }
        }

        allErrorsCount = _errorConnects.Count; // ����� ���������� ������
        if (allErrorsCount > 0) {
            ShowErrorConnections();
            errorsProcentage = (allErrorsCount / allContactsCount) * 100;
            Debug.Log("������ � ������: " + allErrorsCount + "/" + allContactsCount);
        } else {
            Debug.Log("������ ������!");
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

    public void ShowErrorConnections() {
        if (_errorConnects.Count == 0) return;
        
        foreach (ConnectionData iConnection in _errorConnects) {
            Contact errorContact = FindContactInConpanents(iConnection);

            if (errorContact) {
                errorContact.StartBlink();
                errorContact.Select();
            }
        }  
    }

    private Contact FindContactInConpanents(ConnectionData connection) {
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
}
