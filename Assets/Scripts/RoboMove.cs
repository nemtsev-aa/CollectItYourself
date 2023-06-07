using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoboMove : MonoBehaviour {

    public GameObject HitPoint;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private bool _isMoving;

    private Ray _ray;
    private RaycastHit _hit;

    private void Update() {
        //ManualControl();
        if (Input.GetKeyDown(KeyCode.Space)) {
            AutoControl();
        }
    }

    void AutoControl() {

        while (!ScanFreeRight()) {
            Filling();
            MoveUp();
        }

        while (ScanFreeRight()) {
            MoveUp();
        }

        while (!ScanFreeRight() && ScanFreeUp()) {
            Filling();
            MoveUp();
        }

        while (!ScanFreeUp()) {
            Filling();
            MoveLeft();
        }

        while (ScanFreeUp()) {
            MoveLeft();
        }

        while (!ScanFreeUp()) {
            Filling();
            MoveLeft();
        }
    }

    void ManualControl() {
        if (Input.GetKeyDown(KeyCode.W)) {
            if (ScanFreeUp()) {
                MoveUp();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            if (ScanFreeDown()) {
                MoveDown();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            if (ScanFreeLeft()) {
                MoveLeft();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            if (ScanFreeRight()) {
                MoveRight();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            Filling();
        }
    }

    private void MoveUp() {
        if (_isMoving) return;
        _startPosition = transform.position;
        _targetPosition = transform.localPosition + Vector3.forward * 1f;

        StartCoroutine(MoveToTarget());
    }

    private void MoveDown() {
        if (_isMoving) return;
        _startPosition = transform.position;
        _targetPosition = transform.localPosition - Vector3.forward * 1f;

        StartCoroutine(MoveToTarget());
    }

    private void MoveRight() {
        if (_isMoving) return;
        _startPosition = transform.position;
        _targetPosition = transform.localPosition + Vector3.right * 1f;

        StartCoroutine(MoveToTarget());
    }

    private void MoveLeft() {
        if (_isMoving) return;
        _startPosition = transform.position;
        _targetPosition = transform.localPosition - Vector3.right * 1f;

        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget() {
        _isMoving = true;
        for (float i = 0; i < 1; i += Time.deltaTime) {
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, i);
            yield return null;
        }
        transform.localPosition = _targetPosition;
        _isMoving = false;
    }


    private bool ScanFreeUp() {
        _ray = new Ray(transform.position, Vector3.forward * 1f);
        if (Physics.Raycast(_ray, out _hit, 1f)) {
            //Debug.DrawRay(_ray.origin, _ray.direction * 1f, Color.blue); 
            return false;
        }
        //Debug.Log("FreeUp");
        return true;
    }

    private bool ScanFreeDown() {
        _ray = new Ray(transform.position, -Vector3.forward * 1f);
        if (Physics.Raycast(_ray, out _hit, 1f)) {
            //Debug.DrawRay(_ray.origin, _ray.direction * 1f, Color.blue); 
            return false;
        }
        //Debug.Log("FreeDown");
        return true;
    }

    private bool ScanFreeRight() {
        _ray = new Ray(transform.position, Vector3.right * 1f);
        if (Physics.Raycast(_ray, out _hit, 1f)) {
            //Debug.DrawRay(_ray.origin, _ray.direction * 1f, Color.blue); 
            return false;
        }
        //Debug.Log("FreeRight");
        return true;
    }

    private bool ScanFreeLeft() {
        _ray = new Ray(transform.position, Vector3.left * 1f);
        if (Physics.Raycast(_ray, out _hit, 1f)) {
            //Debug.DrawRay(_ray.origin, _ray.direction * 1f, Color.blue); 
            return false;
        }
        //Debug.Log("FreeLeft");
        return true;
    }

    private void Filling() {
        _ray = new Ray(transform.position, Vector3.down * 1f);
        if (Physics.Raycast(_ray, out _hit, 1f)) {
            //Debug.DrawRay(_ray.origin, _ray.direction * 1f, Color.blue); 
            _hit.transform.GetComponent<Renderer>().material.color = Color.green;
            Debug.Log(_hit.transform.parent.gameObject.name + " " + _hit.transform.gameObject.name + " залита");
        }
    }

}

