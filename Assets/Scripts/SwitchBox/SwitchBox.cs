using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBox : MonoBehaviour
{
    public bool isOpen;
    public string Name;
    public int Number;
    public List<WagoClip> Wagos = new List<WagoClip>();
    public List<Companent> Companents = new List<Companent>();
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

    public void RemoveCompanent(Companent companent) {
        if (Companents.Contains(companent)) {
            Companents.Remove(companent);
            Destroy(companent.gameObject);
        }
    }
}
