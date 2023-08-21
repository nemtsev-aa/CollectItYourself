using UnityEngine;

public class Lamp : Companent {
    [field: SerializeField] public LampState LampState { get; private set; }

    public override void Activate() {
        base.Activate();
        LampState.SetState(true);
    }

    public override void Deactivate() {
        base.Activate();
        LampState.SetState(false);
    }
}
