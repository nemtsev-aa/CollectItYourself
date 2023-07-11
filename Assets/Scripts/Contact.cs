using System;
using System.Collections;
using UnityEngine;
public enum ContactType {
    Line,
    Neutral,
    GroundConductor,
    Closed,
    Open,
    LineOut,
    Wago
}

public class Contact : SelectableObject {
    public ContactType ContactType;
    public Material Material;
    [SerializeField] private Renderer _renderer;
    public Wire ConnectionWire;

    public Action ContactPositionChanged;

    private Color _defaultColor;
    private float _duration = 2f;
    private Companent _parentCompanent;

    public void Initialize(Companent parentCompanent) {
        _parentCompanent = parentCompanent;
    }

    public override void OnHover() {
        //transform.localScale = Vector3.one * 1.5f;
    }

    [ContextMenu("StartBlink")]
    public void StartBlink() {
        _defaultColor = Material.color;
        StartCoroutine(ShowEffect());
    }

    public IEnumerator ShowEffect() {
        for (float t = 0; t < 2f; t += Time.deltaTime) {
            SetColor(new Color(Mathf.Sin(10 * t) * 0.5f + 0.5f, 0, 0, 0));
            yield return null;
        }
        SetColor(_defaultColor); // Устанавливаем конечный цвет материала после окончания анимации
    }

    public void SetColor(Color newColor) {
        if (gameObject != null) {
            for (int m = 0; m < _renderer.materials.Length; m++) {
                _renderer.materials[m].SetColor("_BaseColor", newColor);
            }
        }
    }

    public override void OnMouseDrag() {
        base.OnMouseDrag();
        ContactPositionChanged?.Invoke();
    }

    public override Companent GetParentCompanent() {
        return _parentCompanent;
    }

    public virtual void ResetContact() {
        ContactPositionChanged -= ConnectionWire.SetNewPositionEndContact;
        ConnectionWire = null;
    }
}
