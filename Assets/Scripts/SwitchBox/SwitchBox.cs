using CustomEventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwitchBox : MonoBehaviour {
    [Header("Switching Parametrs")]
    public Dictionary<string, ConnectionData> ErrorConnects = new();
    public int Number { get { return SwitchBoxData.PartNumber; } private set { } }
    [field: SerializeField] public bool isOpen { get; private set; }
    [field: SerializeField] public SingleSwitchingResult Result;

    [Header("Data")]
    public SwitchBoxData SwitchBoxData;
    public List<Transform> Slots = new List<Transform>();
    [SerializeField] private Transform CompanentsTransform;
    public List<Companent> Companents = new List<Companent>();
    public string TaskName;
    [Header("Clips")]
    public Transform WagoClipsTransform;
    public List<WagoClip> WagoClips = new List<WagoClip>();
    public WagoClip ActiveWagoClip;
    [Header("Wires")]
    public Transform WiresTransform;
    public List<Wire> Wires = new List<Wire>();

    private Stopwatch _stopwatch;
    private List<WagoContact> _freeWagoContacts = new List<WagoContact>();

    public event Action<SingleSwitchingResult> SingleIncorrectChecked;
    public event Action<bool> OnShowCurrent;
    public Action<bool> OnConnectionsCountChanged;
        
    private EventBus _eventBus;

    public void Init() {
        _stopwatch = ServiceLocator.Current.Get<Stopwatch>();
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    #region CreatingAndEditingLists
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

    public void RemoveLineToList(Wire wire) {
        wire.StartContact.ResetContact();
        wire.EndContact.ResetContact();

        Wires.Remove(wire);
        Destroy(wire.gameObject);
    }

    public void RemoveCompanent(Companent companent) {
        if (Companents.Contains(companent)) {
            Companents.Remove(companent);
            Destroy(companent.gameObject);
        }
    }
    #endregion

    #region WorkingWithTime
    public void SetTimeValue() {
        if (!Result) return;
        _stopwatch.SetTimeValue(Result.BuldingTime);
    }

    public void GetTimeValue() {
        if (!Result) return;
        Result.SetSwitchingTimeValue(_stopwatch.GetTimeValue());
    }
    #endregion

    #region VisualEffects
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

    [ContextMenu("StartCurrent")]
    public void StartCurrent() {
        OnShowCurrent?.Invoke(true);
    }

    [ContextMenu("StopCurrent")]
    public void StopCurrent() {
        OnShowCurrent?.Invoke(false);
    }

    public void ShowCurrent(bool status) {
        OnShowCurrent?.Invoke(status);
    }
    #endregion

    #region WorkingWithWagoContacts
    ///  Функции для работы с контактами Wago-зажимов
    public int FindFreeWagoContacts() {
        //Debug.Log("Поиск свободных Wago-зажимов!");
        foreach (WagoClip iWagoClip in WagoClips) {
            foreach (WagoContact iContact in iWagoClip.WagoContacts) {
                if (!iContact.ConnectionStatus) {
                    if (!_freeWagoContacts.Contains(iContact)) {
                        _freeWagoContacts.Add(iContact);
                    }
                }
            }
        }
        return _freeWagoContacts.Count;
    }

    public WagoContact GetFreeWagoContact() {
        // Приоритет на подключение установлен для контактов активного Wago-зажима
        foreach (WagoContact iWagoContact in ActiveWagoClip.WagoContacts) {
            if (iWagoContact.ConnectedContact == null) {
                return iWagoContact;
            }
        }
        // Если активный Wago-зажима не имеет свободных контактов, подключаем по очереди добавления Wago-зажимов в сборку
        return _freeWagoContacts[0];
    }

    public void SelectFreeWagoContacts(bool status) {
        //Debug.Log("SelectFreeWagoContacts: " + status);
        if (_freeWagoContacts.Count > 0) {
            foreach (WagoContact iWagoContact in _freeWagoContacts) {
                if (status) {
                    iWagoContact.Select();
                }
                else {
                    iWagoContact.Unselect();
                }
            }
        }
    }

    public void RemoveWagoContactFromFreeList(WagoContact wagoContact) {
        if (_freeWagoContacts.Contains(wagoContact)) {
            _freeWagoContacts.Remove(wagoContact);
        }
    }
    #endregion

    #region СheckingСonnections
    //[ContextMenu("Сheck")]
    //public void Сheck() {
    //    if (СheckingСonnections() != null) {
    //        Debug.Log(TaskName + " True");
    //    } else {
    //        Debug.Log(TaskName + " False");
    //    }
    //}

    /// <summary>
    /// Проверка сборки отдельной распределительной коробки 
    /// </summary>
    /// <returns></returns>
    public SingleSwitchingResult СheckingСonnections() {
        if (WagoClips.Count == 0) {
            Debug.Log("Схема не собрана!");
            return null;
        }
                
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
                    if (errorConnectsList != null) {
                        int errorCount = errorConnectsList.Count; // Количество ошибок в проверяемом Wago-зажимe
                        if (ErrorConnects.Count >= 0) { // Словарь с найденными ошибками не пуст
                            foreach (ConnectionData iError in errorConnectsList) {
                                string newError = iError.CompanentName.ToString() + "_" + iError.ContactType.ToString();
                                if (!ErrorConnects.ContainsKey(newError)) {
                                    ErrorConnects.Add(newError, iError); // Пополняем словарь ошибочных подключений новой записью
                                }
                            }
                        }
                    } else {
                        return null;
                    }
                }
            }
        }

        allErrorsCount = ErrorConnects.Count; // Общее количество ошибок

        foreach (string iKey in ErrorConnects.Keys) {
            Debug.Log(iKey);
        }
       
        string resultTaskName = TaskName;
        bool resultCheckStatus;
        int resultSwitchBoxNumer = SwitchBoxData.PartNumber;
        string resultErrorCountText = allErrorsCount + "/" + allContactsCount;
        
        List<ConnectionData> resultErrorList = new List<ConnectionData>();
        if (ErrorConnects.Count > 0) {
            resultErrorList = CreateErrorsList();
            if (resultErrorList == null) {
                Debug.LogError("Ошибка переноса списка ошибок из словаря!" + this.name);
            }
        }

        float resultSwitchingTimeValue = _stopwatch.GetTimeValue();
        

        if (allErrorsCount > 0) {
            //errorsProcentage = (allErrorsCount / allContactsCount) * 100;
            Debug.Log("Ошибок в сборке: " + allErrorsCount + "/" + allContactsCount);
            resultCheckStatus = false;
        } else {
            Debug.Log("Верная сборка!");
            resultCheckStatus = true;
        }

        Result = SingleSwitchingResult.CreateInstance(resultTaskName, resultCheckStatus, resultSwitchBoxNumer,
                                           resultErrorCountText, resultSwitchingTimeValue, resultErrorList);
        return Result;
    }

    private List<ConnectionData> FindConnectionInAnswer(ConnectionData connectionData) {
       
        TaskData taskData = ServiceLocator.Current.Get<TaskController>().CurrentTaskData;
        Answer answer;                                                                      // Данные верного подключения
        if (taskData.Type == TaskType.Full) {
            answer = taskData.Answers[SwitchBoxData.PartNumber - 1]; 
        } else {
            answer = taskData.Answers[0]; 
        }
        
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
            Debug.Log(gameObject.name + " cписки имеют различную длину: " + connectionsResult.Count + "|" + connectionsAnswer.Count);
            return null;
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

    private List<ConnectionData> CreateErrorsList() {
        List<ConnectionData> resultErrorList = new List<ConnectionData>();
        foreach (KeyValuePair<string, ConnectionData> iConnection in ErrorConnects) {
            if (!resultErrorList.Contains(iConnection.Value)) {
                resultErrorList.Add(iConnection.Value);
            }
        }
        return ErrorConnects.Count != resultErrorList.Count ? null : resultErrorList;
    }

    public int GetConnectionsCount() {
        int connectionsCoint = 0;
        foreach (var iWagoClip in WagoClips) {
            connectionsCoint += iWagoClip.Connections.Count;
        }
        return connectionsCoint;
    }
    #endregion


    public Companent GetComponentByConnectionData(ConnectionData connectionData) {
        Companent companent = Companents.Find(x => x.Type == connectionData.CompanentType && x.Name == connectionData.CompanentName);

        return companent;
    }


}
