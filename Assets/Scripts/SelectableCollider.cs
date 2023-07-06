using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableCollider : MonoBehaviour
{
    public SelectableObject SelectableObject;
    [SerializeField] private Transform _target;
    private BoxCollider _boxCollider;

    private void Start() {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Update() {
        if (_target != null) {
            _boxCollider.transform.position = _target.position;
        }
    }
}


