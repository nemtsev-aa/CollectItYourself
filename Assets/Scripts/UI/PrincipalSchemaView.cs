using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrincipalSchemaView : MonoBehaviour
{
    [Tooltip("������� - �������� �������")]
    [SerializeField] private TextMeshProUGUI _name;
    [Tooltip("����������� - �����")]
    [SerializeField] private Image _image;
    [Tooltip("������ �����������")]
    [SerializeField] private TimeView _timeView;

    public void Show(SwitchBox switchBox) {
        Debug.Log("PrincipalSchemaView_Show");
        _name.text = switchBox.SwitchBoxData.Task.Name;
        _image.sprite = switchBox.SwitchBoxData.Task.TaskData[switchBox.SwitchBoxData.PartNumber-1].PrincipalShemas;
    }
}
