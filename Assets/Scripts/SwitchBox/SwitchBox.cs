using System.Collections.Generic;
using UnityEngine;


public class SwitchBox : MonoBehaviour {
    public SwitchBoxData SwitchBoxData;
    public bool isOpen;
    public List<Transform> Slots = new List<Transform>();
    public List<WagoClip> Wagos = new List<WagoClip>();
    public List<Wire> Lines = new List<Wire>();

    public void AddNewWagoClipToList(WagoClip wago) {
        Wagos.Add(wago);
    }

    public void RemoveWagoClipFromList(WagoClip wago) {
        if (Wagos.Contains(wago)) {
            Wagos.Remove(wago);
            Destroy(wago.gameObject);
        }
    }

    public void AddNewLineFromList(Wire line) {
        Lines.Add(line);
    }

    public void RemoveCompanent(CompanentData companentData) {    
        if (SwitchBoxData.Companents.Contains(companentData)) {
            SwitchBoxData.Companents.Remove(companentData);
            Destroy(companentData.Companent.gameObject);
        }
    }
}
