using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBoxManager : MonoBehaviour
{
    [SerializeField] private SwitchBox _switchBoxPrefab1;
    [SerializeField] private SwitchBox _switchBoxPrefab2;

    public List<SwitchBox> SwitchBoxes = new List<SwitchBox>();
    public SwitchBox ActiveSwichBox;


    public void SetActiveSwichBox(SwitchBox switchBox) {
        ActiveSwichBox = switchBox;
    }

    public void CreateSwichBox(SwitchBoxData switchBoxData) {
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
        AddSwichBoxFromList(newSwitchBox);

        for (int i = 0; i < switchBoxData.Companents.Count; i++) {
            CompanentData data = switchBoxData.Companents[i]; // Данные компанента
            Companent newCompanent = Instantiate(data.Companent); // Новый компанент
            newCompanent.Name = data.Name;
            newCompanent.ShowName();
            newCompanent.SwitchBox = newSwitchBox;
            newCompanent.transform.parent = newSwitchBox.Slots[data.SlotNumber].transform;
            newCompanent.transform.position = _switchBoxPrefab.Slots[data.SlotNumber].position;
        }
    }

    public void AddSwichBoxFromList(SwitchBox switchBox) {
        SwitchBoxes.Add(switchBox);
    }
}
