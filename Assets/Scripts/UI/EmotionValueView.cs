using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmotionValueView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _positiveText;
    [SerializeField] private TextMeshProUGUI _negativeText;

    [SerializeField] private Image _positiveImage;
    [SerializeField] private Image _negativeImage;

    public void PositiveValueShow(float value)
    {
        _positiveText.text = value.ToString("0");
        if (_positiveImage)
        {
            _positiveImage.fillAmount = value / 100;
        }
       
    }

    public void NegativeValueShow(float value)
    {
        _negativeText.text = value.ToString("0");
        if (_negativeImage)
        {
            _negativeImage.fillAmount = value / 100;
        } 
    }

    public void ResetValue()
    {
        PositiveValueShow(0);
        NegativeValueShow(0);
    }
}
