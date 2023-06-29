using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBoxManager : MonoBehaviour
{
    [SerializeField] private SwitchBox _switchBoxPrefab1;
    [SerializeField] private SwitchBox _switchBoxPrefab2;

    public List<SwitchBox> SwitchBoxes = new List<SwitchBox>();
    public SwitchBox ActiveSwichBox;
    
    public event Action<int> ActiveSwitchBoxChanged;
    public event Action<SwitchingResult> OnWin;
    public event Action<SwitchingResult> OnLose;

    public Stopwatch Stopwatch;

    private Management _management;

    public void SetActiveSwichBox(SwitchBox switchBox) {
        ActiveSwichBox = switchBox;
        ActiveSwitchBoxChanged?.Invoke(switchBox.SwitchBoxData.PartNumber);
    }

    public SwitchBox CreateSwichBox(SwitchBoxData switchBoxData) {
        foreach (var iSwichBox in SwitchBoxes) {
            if (iSwichBox.SwitchBoxData == switchBoxData) {
                return null;
            }
        }

        SwitchBox newSwitchBox;
        SwitchBox _switchBoxPrefab;
        if (switchBoxData.Companents.Count <= 6) {
            _switchBoxPrefab = _switchBoxPrefab1;
        } else {
            _switchBoxPrefab = _switchBoxPrefab2;
        }

        newSwitchBox = Instantiate(_switchBoxPrefab);
        newSwitchBox.gameObject.name = switchBoxData.name;
        newSwitchBox.SwitchBoxData = switchBoxData;
        newSwitchBox.transform.parent = transform;
        AddSwichBoxToList(newSwitchBox);
        SetActiveSwichBox(newSwitchBox);

        for (int i = 0; i < switchBoxData.Companents.Count; i++) {
            CompanentData data = switchBoxData.Companents[i]; // Äàííûå êîìïàíåíòà
            Companent newCompanent = Instantiate(data.Companent); // Íîâûé êîìïàíåíò
            newCompanent.SlotNumber = data.SlotNumber;
            newCompanent.Name = data.Name;
            newCompanent.ShowName();
            newCompanent.SwitchBox = newSwitchBox;
            newCompanent.transform.parent = newSwitchBox.Slots[data.SlotNumber].transform;
            newCompanent.transform.position = _switchBoxPrefab.Slots[data.SlotNumber].position;

            newSwitchBox.Companents.Add(newCompanent);

            newSwitchBox.Initialized(Stopwatch, this);
        }

        return newSwitchBox;
    }

    private void SwitchBox_OnLose(SwitchingResult switchingResult) {
        OnLose?.Invoke(switchingResult);
        GameStateManager.Instance.SetLose();
    }

    private void SwitchBox_OnWin(SwitchingResult switchingResult) {
        OnWin?.Invoke(switchingResult);
        GameStateManager.Instance.SetWin();
    }

    public void AddSwichBoxToList(SwitchBox switchBox) {
        SwitchBoxes.Add(switchBox);
    }

    public void RemoveSwichBoxFromList(SwitchBox switchBox) {
        if (SwitchBoxes.Contains(switchBox)) {
            SwitchBoxes.Remove(switchBox);
            Destroy(switchBox.gameObject);
        }
    }

    public void CheckSwichBox(int number) {
        SwitchBoxes[number-1].ÑheckingÑonnections();
    }

    public void CheckSwichBoxes() {
        int errorsCount = 0;
        float switchingsTime = 0f;

        foreach (SwitchBox iSwichBox in SwitchBoxes) {
            switchingsTime += iSwichBox.Result.SwitchingTimeValue;
            errorsCount += iSwichBox.ÑheckingÑonnections();
        }
    }

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
}
