using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton_DeleteCompanent : MonoBehaviour
{
    [SerializeField] private Companent Companent;

    private Button _button;

    private void Start() {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(Delete);
    }

    private void Delete() {
        Companent.Unselect();
        Companent.RemoveCompanent();
    }
}
