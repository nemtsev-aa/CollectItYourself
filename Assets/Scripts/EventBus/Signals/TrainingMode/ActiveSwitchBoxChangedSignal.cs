
namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал об изменении активной распределительной коробки
    /// </summary>
    public class ActiveSwitchBoxChangedSignal {
        public readonly SwitchBox SwitchBox;
        public ActiveSwitchBoxChangedSignal(SwitchBox switchBox) {
            SwitchBox = switchBox;
        }
    }
}
