using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionButton : MonoBehaviour {
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _selectionIndicatorImage;

    private Description _description;
    public Action<Description, DescriptionButton> OnActivate;
    
    public void Init(Description description) {
        _description = description;
        _nameText.text = _description.Name;
        _iconImage.sprite = _description.Icon;
        _button.onClick.AddListener(Activate);
    }

    public void Activate() {
        OnActivate?.Invoke(_description, this);
        _selectionIndicatorImage.gameObject.SetActive(true);
    }

    public void Deactivate() {
        _selectionIndicatorImage.gameObject.SetActive(false);
    }
}
