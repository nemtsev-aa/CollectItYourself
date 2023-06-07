using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    public Camera Camera;
    private LineRenderer line;
    private Vector3 mousePos;
    public Material material;
    private int currentLines = 0;
    public Pointer Pointer;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (line == null) {
                createLine();
            }

            Debug.Log("Start");
            
            line.SetPosition(0, Pointer.Aim.transform.position);
            line.SetPosition(1, Pointer.Aim.transform.position);
        } else if (Input.GetMouseButtonUp(0) && line) {
            Debug.Log("End");
           
            line.SetPosition(1, Pointer.Aim.transform.position);
            line = null;
            currentLines++;

        } else if (Input.GetMouseButton(0) && line) {
            Debug.Log("Move");
           
            line.SetPosition(1, Pointer.Aim.transform.position);
        }
    }

    private void createLine() {
        line = new GameObject("Line" + currentLines).AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.material = material;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.useWorldSpace = false;
        line.numCapVertices = 50;
    }
}
