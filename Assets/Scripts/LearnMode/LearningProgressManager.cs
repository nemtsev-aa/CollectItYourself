using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningProgressManager : MonoBehaviour
{
    [SerializeField] private LearningProgressView _learningProgressView;
    [field: SerializeField] public int CurrentExpValue { get; private set; }
    public List<ChapterData> ChapterDatas = new List<ChapterData>();
    
    [Space(10)]
    [SerializeField] private ChapterPanel _chapterPanelPrfab;
    [SerializeField] private Transform _chapterPanelParent;
    public List<ChapterPanel> ChapterPanels = new List<ChapterPanel>();

    public event Action<int> ProgressValueChanget;

    private int _fullExpAmount;

    public void Initialization() {
        if (ChapterDatas.Count == 0 || _chapterPanelPrfab == null) {
            Debug.Log("������ ��� ������������� ������ (��������) �� �������!");
        }
        else {
            foreach (var iChapterData in ChapterDatas) {
                _fullExpAmount += iChapterData.ExpAmountToComplete; // ��������� ����� ���������� ����� � ������ 

                ChapterPanel newCahepterPanel = Instantiate(_chapterPanelPrfab);
                newCahepterPanel.ChapterData = iChapterData;
                newCahepterPanel.Initialization(this);
                newCahepterPanel.transform.parent = _chapterPanelParent;
                newCahepterPanel.OnChapterComplite += ChapterComplite;
                ChapterPanels.Add(newCahepterPanel);
            }
        }  
    }

    private void ChapterComplite(ChapterData chapterData) {
        CurrentExpValue += chapterData.ExpAmountToComplete;
        int _currentProgress = (CurrentExpValue / _fullExpAmount) * 100;
        ProgressValueChanget?.Invoke(_currentProgress);
    }

    private void OnDisable() {
        foreach (var iChapterPanel in ChapterPanels) {
            if (iChapterPanel.ChapterData == null) {
                Debug.Log("������ �� �������!");
            }
            else {
                iChapterPanel.OnChapterComplite -= ChapterComplite;
            }
        }
    }
}
