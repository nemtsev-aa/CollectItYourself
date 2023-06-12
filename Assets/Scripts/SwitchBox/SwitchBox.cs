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
}
