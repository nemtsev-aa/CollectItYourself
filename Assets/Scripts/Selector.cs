using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Companent {
    [field: SerializeField] public SelectorsKey SelectorsKey { get; private set; }

    public override void Init() {
        base.Init();

        foreach (ObjectView objectView in SelectorsKey.ObjectViews) {
            Debug.Log($"Selector Init: ObjectView {objectView.name}");
            objectView.Init(this);
            objectView.UpdatePoints();
        }

        foreach (ElectricFieldMovingView electricView in SelectorsKey.ElectricFieldMovingViews) {
            electricView.SetObject(this);
            electricView.UpdatePoints();
        }

    }

    public override void Activate() {
        base.Activate();
    }

    public override void Action() {
        SelectorsKey.SwitchKey();
    }
}
