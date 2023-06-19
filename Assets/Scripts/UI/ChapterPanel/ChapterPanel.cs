using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPanel : MonoBehaviour
{
    public ChapterData ChapterData;

    [SerializeField] private TextMeshProUGUI _chepterTitle;
    [SerializeField] private Image _chepterIcon;
    [SerializeField] private Image _progressValueImage;
    [SerializeField] private TextMeshProUGUI _progressValueText;
    [SerializeField] private TextMeshProUGUI _chepterDescription;
    [SerializeField] private TextMeshProUGUI _expAmount;
}
