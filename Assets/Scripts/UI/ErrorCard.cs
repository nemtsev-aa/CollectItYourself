using System;
using TMPro;
using UnityEngine;

public class ErrorCard : MonoBehaviour {
    public BacklitButton Button => _button;
    public Contact Contact => _contact;

    [SerializeField] private BacklitButton _button;
    [SerializeField] private TextMeshProUGUI _errorIndexText;

    public Action<ErrorCard> OnErrorCardChanged;

    private Contact _contact;

    public void Init(Contact contact, int index) {
        _contact = contact;
        _errorIndexText.text = index.ToString();
        _button.Button.onClick.AddListener(ShowErrorContact);
    }

    private void ShowErrorContact() {
        _button.Status = true;
        OnErrorCardChanged?.Invoke(this);
    }
}
