
namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ �� ��������� �������� ����������������� �������
    /// </summary>
    public class ActiveSwitchBoxChangedSignal {
        public readonly SwitchBox SwitchBox;
        public ActiveSwitchBoxChangedSignal(SwitchBox switchBox) {
            SwitchBox = switchBox;
        }
    }
}
