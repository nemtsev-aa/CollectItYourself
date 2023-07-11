using CustomEventBus;
using System.Collections.Generic;
using UnityEngine;

public class TaskDataLoader : MonoBehaviour {
    public List<ChapterData> ChapterDatas = new List<ChapterData>();
    [SerializeField] private LearningProgressManager _learningProgressManager;
    
    [Space(10)]
    [SerializeField] private ChapterPanel _chapterPanelPrfab;
    [SerializeField] private Transform _chapterPanelParent;
    public List<ChapterPanel> ChapterPanels = new List<ChapterPanel>();

    private EventBus _eventBus;

    [ContextMenu("Initialization")]
    public void Initialization() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        if (ChapterDatas.Count == 0 || _chapterPanelPrfab == null) {
            Debug.Log("Данные для инициализации модуля (обучение) не указаны!");
        } else {
            foreach (var iChapterData in ChapterDatas) {
                ChapterPanel newCahepterPanel = Instantiate(_chapterPanelPrfab);
                newCahepterPanel.ChapterData = iChapterData;
                newCahepterPanel.Initialization(_learningProgressManager);
                newCahepterPanel.transform.parent = _chapterPanelParent;
                ChapterPanels.Add(newCahepterPanel);
            }
        }
    }
}
