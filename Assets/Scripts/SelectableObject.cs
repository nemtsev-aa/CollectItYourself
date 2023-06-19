using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public GameObject SelectIndicator;
    private Vector3 _defaultScale;

    public virtual void Start() {
        SelectIndicator.SetActive(false);
        _defaultScale = transform.localScale;
    }

    public virtual void OnHover() {
        transform.localScale = _defaultScale * 1.05f;
    }

    public virtual void OnUnhover() {
        transform.localScale = _defaultScale;
    }

    public virtual void Select() {
        SelectIndicator.SetActive(true);
    }

    public virtual void Unselect() {
        SelectIndicator.SetActive(false);
    }

    public Companent GetParentCompanent() {
        return GetComponent<Companent>();
    }
}

