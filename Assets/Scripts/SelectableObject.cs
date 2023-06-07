using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public GameObject SelectIndicator;

    public virtual void Start() {
        SelectIndicator.SetActive(false);
    }

    public virtual void OnHover() {
        transform.localScale = Vector3.one * 1.05f;
    }

    public virtual void OnUnhover() {
        transform.localScale = Vector3.one;
    }

    public virtual void Select() {
        SelectIndicator.SetActive(true);
    }

    public virtual void Unselect() {
        SelectIndicator.SetActive(false);
    }
}

