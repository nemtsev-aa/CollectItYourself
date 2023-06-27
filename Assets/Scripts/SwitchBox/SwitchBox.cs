using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBox : MonoBehaviour {
    [Header("Switching Parametrs")]
    public Dictionary<string, ConnectionData> ErrorConnects = new();
    [field: SerializeField] public bool isOpen { get; private set; }
    [field: SerializeField] public SwitchingResult Result = new SwitchingResult();

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

    public event Action<SwitchingResult> OnWin;
    public event Action<SwitchingResult> OnLose;

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

    }

    public void GetTimeValue() {

    }

    #region СheckingСonnections
    [ContextMenu("СheckingСonnections")]
    public int СheckingСonnections() {
        if (WagoClips.Count == 0) {
            Debug.Log("Схема не собрана!");
            return 100;
        }

        float errorsProcentage = 0; // Процент ошибок в сборке
        int allContactsCount = 0; // Общее количество контактов в коробке
        int allErrorsCount; // Количество контактов подключенных с ошибкой

        // Проверяем каждый зажим
        foreach (WagoClip iWagoClip in WagoClips) {
            List<ConnectionData> connectionsResult = iWagoClip.Connections; // Данные о подключенных компанентах и их контактах
            if (iWagoClip.Connections.Count > 0) {
                List<ConnectionData> connectionsAnswer = FindConnectionInAnswer(iWagoClip.Connections[0]); // Осуществляем поиск Wago-зажима в ответе по первому подключенному контакту 
                if (connectionsAnswer != null) {
                    allContactsCount += connectionsAnswer.Count; // Количество контактов в проверяемом Wago-зажимe
                    List<ConnectionData> errorConnectsList = CompareLists(connectionsResult, connectionsAnswer); // Список ошибочных подключений
                    int errorCount = errorConnectsList.Count; // Количество ошибок в проверяемом Wago-зажимe
                    if (ErrorConnects.Count >= 0) { // Словарь с найденными ошибками не пуст
                        foreach (ConnectionData iError in errorConnectsList) {
                            string newError = iError.CompanentName.ToString() + "_" + iError.ContactType.ToString();
                            if (!ErrorConnects.ContainsKey(newError)) {
                                ErrorConnects.Add(newError, iError); // Пополняем словарь ошибочных подключений новой записью
                            }
                        }
                    }
                }
            }
        }

        allErrorsCount = ErrorConnects.Count; // Общее количество ошибок
        if (allErrorsCount > 0) {
            ShowErrorConnections();
            errorsProcentage = (allErrorsCount / allContactsCount) * 100;
            Debug.Log("Ошибок в сборке: " + allErrorsCount + "/" + allContactsCount);
            OnLose?.Invoke(Result);
        }
        else {
            Debug.Log("Верная сборка!");
            OnWin?.Invoke(Result);
        }

        if (Result.SwitchingTimeValue != 0) {
            Result.TaskName = SwitchBoxData.Task.Name;
            Result.SwitchBoxNumber = SwitchBoxData.PartNumber;
            Result.ErrorCount = ErrorConnects.Count;

            if (ErrorConnects.Count > 0) {
                if (!CreateErrorsList()) {
                    Debug.Log("Ошибка переноса списка ошибок из словаря!" + this.name);
                }
            }
        }
        return (int)errorsProcentage;
    }
    
    private List<ConnectionData> FindConnectionInAnswer(ConnectionData connectionData) {
        Answer answer = SwitchBoxData.Answer; // Данные верного подключения
        List<AnswerData> answerDatas = answer.AnswerDataList; // Список Wago-зажимов

        foreach (AnswerData iWagoAnswer in answerDatas) {
            List<ConnectionData> connectionDatas = iWagoAnswer.Connections; // Список данных о подключениях к Wago-зажиму
            if (connectionDatas.Contains(connectionData)) { // Верное подключение содержит искомый компанент и контакт
                return connectionDatas;
            }
        }
        return null;
    }

    private List<ConnectionData> CompareLists(List<ConnectionData> connectionsResult, List<ConnectionData> connectionsAnswer) {
        if (connectionsResult.Count != connectionsAnswer.Count) {
            Debug.Log("Списки имеют различную длину");
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

        //if (ErrorConnects.Count != Result.ErrorList.Count) {
        //    return false;
        //} else {
        //    return true;
        //}

        return ErrorConnects.Count != Result.ErrorList.Count ? true : false;
    }

    #endregion


}
