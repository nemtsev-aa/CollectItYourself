using CustomEventBus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBoxManager : MonoBehaviour, IService {
    public SwitchBox ActiveSwichBox => _activeSwichBox;
    public IEnumerable<SwitchBox> SwitchBoxes => _switchBoxes;

    [SerializeField] private SwitchBox _switchBoxPrefab1;
    [SerializeField] private SwitchBox _switchBoxPrefab2;
    [SerializeField] private CompanentConfig _companentConfig;
    
    private List<SwitchBox> _switchBoxes = new List<SwitchBox>();

    private SwitchBox _activeSwichBox;
    private TaskData _taskData;
    private EventBus _eventBus;

    public void Init(TaskData taskData) {
        _taskData = taskData;
        
    }

    public void SetActiveSwichBox(SwitchBox switchBox) {
        _activeSwichBox = switchBox;
    }

    public void SetActiveSwichBox(int number) {
        if (_activeSwichBox != null) {
            _activeSwichBox.GetTimeValue();  // Записываем время коммутации в деактивируемую коробку
        }
        _activeSwichBox = _switchBoxes[number - 1];
        _activeSwichBox.SetTimeValue();      // Устанавливаем время сборки активируемой коробки
    }

    public void CreateSwitchBoxs() {
        if (_taskData.SwitchBoxsData.Count > 0) {
            foreach (var iSwitchBox in _taskData.SwitchBoxsData) {
                CreateSwichBox(iSwitchBox);
            }
        } else {
            CreateSwichBox(_taskData.SwitchBoxsData[0]);
        }
    }

    public SwitchBox CreateSwichBox(SwitchBoxData switchBoxData) {
        SwitchBox newSwitchBox;
        SwitchBox _switchBoxPrefab;
        if (switchBoxData.Companents.Count <= 6) {
            _switchBoxPrefab = _switchBoxPrefab1;
        } else {
            _switchBoxPrefab = _switchBoxPrefab2;
        }

        newSwitchBox = Instantiate(_switchBoxPrefab);
        //newSwitchBox.gameObject.name = switchBoxData.name;
        newSwitchBox.SwitchBoxData = switchBoxData;
        newSwitchBox.TaskName = _taskData.ID.ToString();
        newSwitchBox.transform.parent = transform;
        AddSwichBoxToList(newSwitchBox);
        SetActiveSwichBox(newSwitchBox);

        for (int i = 0; i < switchBoxData.Companents.Count; i++) {
            CompanentData data = switchBoxData.Companents[i]; // Данные компанента
            Companent prefab = _companentConfig.Get(data.CompanentType, data.TaskMode, data.VersionExecution);
            Companent newCompanent = Instantiate(prefab); // Новый компанент
            newCompanent.SlotNumber = data.SlotNumber;
            newCompanent.Name = data.Name;
            newCompanent.ShowName();
            newCompanent.SwitchBox = newSwitchBox;
            newCompanent.transform.parent = newSwitchBox.Slots[data.SlotNumber].transform;
            newCompanent.transform.position = _switchBoxPrefab.Slots[data.SlotNumber].position;

            newSwitchBox.Companents.Add(newCompanent);
            newSwitchBox.Init();
        }

        return newSwitchBox;
    }

    public void AddSwichBoxToList(SwitchBox switchBox) {
        _switchBoxes.Add(switchBox);
    }

    public void RemoveSwichBoxFromList(SwitchBox switchBox) {
        if (_switchBoxes.Contains(switchBox)) {
            _switchBoxes.Remove(switchBox);
            Destroy(switchBox.gameObject);
        }
    }

    public void CheckSwichBox(int number) {
        _switchBoxes[number-1].СheckingСonnections();
    }

    public void CheckSwichBoxes() {
        int errorsCount = 0;
        GeneralSwitchingResult generalResult;
        string resultTaskName = _taskData.ID;
        bool resultCheckStatus;
        int resultSwitchBoxNumer;
        string resultErrorCountText;
        float resultSwitchingTimeValue = 0f;

        List<ConnectionData> resultErrorList = new List<ConnectionData>();

        if (_taskData.SwitchBoxsData.Count > 0) {
            List<SingleSwitchingResult> singleSwitchingResultList = new List<SingleSwitchingResult>();

            foreach (SwitchBox iSwichBox in SwitchBoxes) {
                SingleSwitchingResult singleSwitchingResult = iSwichBox.СheckingСonnections();
                singleSwitchingResultList.Add(singleSwitchingResult);
                resultErrorList.AddRange(singleSwitchingResult.ErrorList);

                errorsCount += int.Parse(singleSwitchingResult.ErrorCountText);
                resultSwitchingTimeValue += singleSwitchingResult.SwitchingTimeValue;
            }

            if (resultErrorList.Count > 0) {
                resultCheckStatus = false;
                resultSwitchBoxNumer = 0;
                resultErrorCountText = errorsCount.ToString();
            } else {
                resultCheckStatus = true;
                resultSwitchBoxNumer = 0;
                resultErrorCountText = "-";
            }

            generalResult = GeneralSwitchingResult.CreateInstance(_taskData, resultCheckStatus, resultSwitchBoxNumer, singleSwitchingResultList, resultErrorCountText, resultSwitchingTimeValue, resultErrorList);
        }      
    }


    //ShowErrorConnections();


    public SwitchBox GetSwitchBoxByName(string name) {
        foreach (SwitchBox iBox in SwitchBoxes) {
            if (iBox.name == name) {
                return iBox;
            }
        }
        return null;
    }

    public SwitchBox GetSwitchBoxByNumber(int number) {
        foreach (SwitchBox iBox in SwitchBoxes) {
            if (iBox.SwitchBoxData.PartNumber == number) {
                return iBox;
            }
        }
        return null;
    }

    public void ShowCurrent(bool status) {
        _activeSwichBox.ShowCurrent(status);
    }

    public void ShowSwitchBoxs() {
        if (_switchBoxes.Count > 0) {
            foreach (SwitchBox iSB in SwitchBoxes) {
                iSB.gameObject.SetActive(true);
            }
        }
    }

    public void HideSwitchBoxs() {
        if (_switchBoxes.Count > 0) {
            foreach (SwitchBox iSB in SwitchBoxes) {
                iSB.gameObject.SetActive(false);
            }
        }
    }

}
