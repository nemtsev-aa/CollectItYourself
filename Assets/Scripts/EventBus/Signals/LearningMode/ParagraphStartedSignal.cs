namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� ����� ��������
    /// </summary>
    public class ParagraphStartedSignal {

        public readonly ParagraphDescription ParagraphDescription;
        public ParagraphStartedSignal(ParagraphDescription paragraphDescription) {
            ParagraphDescription = paragraphDescription;
        }
    }
}
