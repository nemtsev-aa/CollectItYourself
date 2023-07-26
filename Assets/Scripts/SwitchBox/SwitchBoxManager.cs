using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBoxManager : MonoBehaviour, IService {
    public TaskData TaskData => _taskData;
    public SwitchBox ActiveSwichBox => _activeSwichBox;
    public IEnumerable<SwitchBox> SwitchBoxes => _switchBoxes;

    [SerializeField] private SwitchBox _switchBoxPrefab1;
    [SerializeField] private SwitchBox _switchBoxPrefab2;
    [SerializeField] private CompanentConfig _companentConfig;
    
    private List<SwitchBox> _switchBoxes = new List<SwitchBox>();
    private SwitchBoxsSelectorView _switchBoxsSelectorView;
    private SwitchBox _activeSwichBox;
    private TaskData _taskData;
    private EventBus _eventBus;

    public void Init(TaskData taskData, EventBus eventBus) {
        _taskData = taskData;
        _eventBus = eventBus;
        _eventBus.Subscribe((TrainingModeStopSignal signal) => HideSwitchBoxs());
        _eventBus.Subscribe((TrainingModeStartSignal signal) => ShowSwitchBoxs());
    }

    /// <summary>
    /// ��������� ������� ������������ ����� ������������������ ���������
    /// </summary>
    /// <param name="switchBoxsSelectorView"></param>
    public void SetSwitchBoxsSelectorView(SwitchBoxsSelectorView switchBoxsSelectorView) {
        _switchBoxsSelectorView = switchBoxsSelectorView;
    }

    /// <summary>
    /// ��������� ����������������� �������
    /// </summary>
    /// <param name="switchBox"></param>
    public void SetActiveSwichBox(SwitchBox switchBox) {
        if (_activeSwichBox != null) {
            _activeSwichBox.GetTimeValue();  // ���������� ����� ���������� � �������������� �������
        }
        _activeSwichBox = switchBox;
        _activeSwichBox.SetTimeValue();      // ������������� ����� ������ ������������ �������

        _eventBus.Invoke(new ActiveSwitchBoxChangedSignal(_activeSwichBox));
    }
    
    /// <summary>
    /// �������� ����������������� ������� �� ������ �������
    /// </summary>
    public void CreateSwitchBoxs() {
        if (_taskData.SwitchBoxsData.Count > 0) {
            foreach (var iSwitchBox in _taskData.SwitchBoxsData) {
                SwitchBox newSwitchBox = CreateSwichBox(iSwitchBox);
                _switchBoxsSelectorView.CreateSwitchBoxsSelector(newSwitchBox);
            }
        } else {
            SwitchBox newSwitchBox = CreateSwichBox(_taskData.SwitchBoxsData[0]);
            _switchBoxsSelectorView.CreateSwitchBoxsSelector(newSwitchBox);
        }

        SetActiveSwichBox(_switchBoxes[0]);                           // ������������� � �������� �������� ������� ������ ���������
        _switchBoxsSelectorView.SetActiveSwitchBoxSelector(0);        // ��������� ������������� ����������������� ������� � ������ �������
    }

    /// <summary>
    /// �������� ��������� ����������������� �������
    /// </summary>
    /// <param name="switchBoxData"></param>
    /// <returns></returns>
    public SwitchBox CreateSwichBox(SwitchBoxData switchBoxData) {
        SwitchBox newSwitchBox;
        SwitchBox _switchBoxPrefab;
        if (switchBoxData.Companents.Count <= 6) {
            _switchBoxPrefab = _switchBoxPrefab1;
        } else {
            _switchBoxPrefab = _switchBoxPrefab2;
        }

        Vector3 newSwitchBoxPosition = new Vector3();
        switch (switchBoxData.PartNumber) {
            case 1:
                newSwitchBoxPosition = Vector3.zero;
                break;
            case 2:
                newSwitchBoxPosition = new Vector3(0f, -25f, 0f);
                break;
            case 3:
                newSwitchBoxPosition = new Vector3(25f, -25f, 0f);
                break;
            case 4:
                newSwitchBoxPosition = new Vector3(25f, 0f, 0f);
                break;
            default:
                break;
        }

        newSwitchBox = Instantiate(_switchBoxPrefab, newSwitchBoxPosition, Quaternion.identity);
        //newSwitchBox.gameObject.name = switchBoxData.name;
        newSwitchBox.SwitchBoxData = switchBoxData;
        newSwitchBox.TaskName = _taskData.ID.ToString();
        newSwitchBox.transform.parent = transform;
        AddSwichBoxToList(newSwitchBox);
        SetActiveSwichBox(newSwitchBox);

        for (int i = 0; i < switchBoxData.Companents.Count; i++) {
            CompanentData data = switchBoxData.Companents[i]; // ������ ����������
            Companent prefab = _companentConfig.Get(data.CompanentType, data.TaskMode, data.VersionExecution);
            if (prefab != null) {
                Companent newCompanent = Instantiate(prefab); // ����� ���������
                newCompanent.SlotNumber = data.SlotNumber;
                newCompanent.Name = data.Name;
                newCompanent.ShowName();
                newCompanent.SwitchBox = newSwitchBox;
                newCompanent.transform.parent = newSwitchBox.Slots[data.SlotNumber].transform;
                newCompanent.transform.localPosition = Vector3.zero;
                

                newSwitchBox.Companents.Add(newCompanent);
                newSwitchBox.Init();
            } else {
                Debug.LogError("������ �� ������!");
            }
        }
        return newSwitchBox;
    }

    /// <summary>
    /// ���������� ����������������� ������� � ������
    /// </summary>
    /// <param name="switchBox"></param>
    public void AddSwichBoxToList(SwitchBox switchBox) {
        _switchBoxes.Add(switchBox);
    }

    /// <summary>
    /// �������� ����������������� ������� �� ������
    /// </summary>
    /// <param name="switchBox"></param>
    public void RemoveSwichBoxFromList(SwitchBox switchBox) {
        if (_switchBoxes.Contains(switchBox)) {
            _switchBoxes.Remove(switchBox);
            Destroy(switchBox.gameObject);
        }
    }

    /// <summary>
    /// �������� ���������� �������
    /// </summary>
    /// <returns></returns>
    public GeneralSwitchingResult CheckSwichBoxes() {
        int errorsCount = 0;
        GeneralSwitchingResult generalResult;
        string currentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        string resultTaskName = _taskData.ID;
        bool resultCheckStatus;
        int resultSwitchBoxNumber;
        string resultErrorCountText;
        float resultSwitchingTimeValue = 0f;

        List<ConnectionData> resultErrorList = new List<ConnectionData>();

        if (_taskData.SwitchBoxsData.Count > 0) {
            List<SingleSwitchingResult> singleSwitchingResultList = new List<SingleSwitchingResult>();
            foreach (SwitchBox iSwichBox in SwitchBoxes) {
                Debug.Log("����������� ��: " + iSwichBox.SwitchBoxData.PartNumber);
                SingleSwitchingResult singleSwitchingResult = iSwichBox.�hecking�onnections();
                if (singleSwitchingResult != null) {
                    singleSwitchingResultList.Add(singleSwitchingResult);
                    resultErrorList.AddRange(singleSwitchingResult.ErrorList);

                    errorsCount += singleSwitchingResult.ErrorList.Count;
                    resultSwitchingTimeValue += singleSwitchingResult.SwitchingTimeValue;
                } else {
                    Debug.Log("������ � ��������� �������� ��: " + iSwichBox.SwitchBoxData.PartNumber);
                    return null;
                }
            }

            if (_taskData.Type == TaskType.Full) {
                resultSwitchBoxNumber = 0;
            } else {
                resultSwitchBoxNumber = _taskData.SwitchBoxsData[0].PartNumber;
            }
            
            if (resultErrorList.Count > 0) {
                resultCheckStatus = false;
                resultErrorCountText = errorsCount.ToString();
            } else {
                resultCheckStatus = true;
                resultErrorCountText = "-";
            }

            generalResult = GeneralSwitchingResult.CreateInstance(currentDate, _taskData, resultCheckStatus, resultSwitchBoxNumber, singleSwitchingResultList, resultErrorCountText, resultSwitchingTimeValue, resultErrorList);
            return generalResult;
        }
        return null;
    }

    /// <summary>
    /// ��������� ����������������� ������� �� �����
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public SwitchBox GetSwitchBoxByName(string name) {
        foreach (SwitchBox iBox in SwitchBoxes) {
            if (iBox.name == name) {
                return iBox;
            }
        }
        return null;
    }

    /// <summary>
    /// ��������� ����������������� ������� �� ������
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public SwitchBox GetSwitchBoxByNumber(int number) {
        foreach (SwitchBox iBox in SwitchBoxes) {
            if (iBox.SwitchBoxData.PartNumber == number) {
                return iBox;
            }
        }
        return null;
    }

    /// <summary>
    /// ������ ���� � �������� ����������������� �������
    /// </summary>
    /// <param name="status"></param>
    public void ShowCurrent(bool status) {
        _activeSwichBox.ShowCurrent(status);
    }

    /// <summary>
    /// ����� ����������������� �������
    /// </summary>
    public void ShowSwitchBoxs() {
        if (_switchBoxes.Count > 0) {
            foreach (SwitchBox iSB in SwitchBoxes) {
                iSB.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// �������� ����������������� �������
    /// </summary>
    public void HideSwitchBoxs() {
        if (_switchBoxes.Count > 0) {
            foreach (SwitchBox iSB in SwitchBoxes) {
                iSB.gameObject.SetActive(false);
            }
        }
    }

}
