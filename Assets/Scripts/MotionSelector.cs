using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSelector : Companent {
    [field: SerializeField] public MotionSensorState MotionSensorState { get; private set; }

    public override void Activate() {
        base.Activate();
        MotionSensorState.SetState(MotionSensorStateType.Show);
    }

    public override void Deactivate() {
        base.Activate();
        MotionSensorState.SetState(MotionSensorStateType.Hide);
    }

    public override void Action() {
        base.Action();
        MotionSensorState.SetState(MotionSensorStateType.Blink);
    }

}
