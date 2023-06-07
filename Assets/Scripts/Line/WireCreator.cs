using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireCreator : MonoBehaviour {
    public SwitchBox SwitchBox;

    public Wire WirePrefab;
    public Contact StartContact;
    public WagoContact EndContact;
    
    public Pointer Pointer;
    private Vector3 _mousePosition;
    private LineRenderer lineRender;

    private void Start() {
        lineRender = GetComponent<LineRenderer>();
    }


    private void Update() {
        if (Input.GetMouseButtonDown(0) && StartContact != null) {
            //Debug.Log("Start");
            //Debug.Log("Start: " + StartContact.transform.position);
            lineRender.enabled = true;
            lineRender.SetPosition(0, StartContact.transform.position);
        }
        else if (Input.GetMouseButtonUp(0) && EndContact != null) {          
            //Debug.Log("End: " + EndContact.transform.position);
            lineRender.SetPosition(1, EndContact.transform.position);
            CreateWire();
        }
        else if (Input.GetMouseButton(0) && StartContact != null) {
            //Debug.Log("Move");
            _mousePosition = Pointer.Aim.position;
            _mousePosition.y = 0;

            lineRender.SetPosition(0, StartContact.transform.position);
            lineRender.SetPosition(1, _mousePosition);
        }
        //else if (Input.GetMouseButtonUp(0) && EndContact == null) {
        //    Debug.Log("Выделение снято");
        //    StartContact = null;
        //    EndContact = null;
        //}
    }

    public void CreateWire() {
        
        Wire newWire = Instantiate(WirePrefab); // Создан новый проводник
        newWire.SwitchBox = SwitchBox;
        newWire.transform.parent = SwitchBox.transform;
        newWire.StartContact = StartContact;
        newWire.EndContact = EndContact;
        newWire.LineRenderer.material = StartContact.Material;
        newWire.LineRenderer.useWorldSpace = false;
        newWire.LineRenderer.numCapVertices = 50;

        switch (WirePrefab.Type) {
            case WireType.Straight:
                CreateStraightWire(newWire);
                break;
            case WireType.Poly:

                break;
            case WireType.Bezier:

                break;
        }

        EndContact.ConnectedContact = StartContact;
        EndContact.Material = StartContact.Material;
        EndContact.SetMaterial();
        EndContact.ConnectionWire = newWire;

        SwitchBox.AddNewLineFromList(newWire);

        StartContact = null;
        EndContact = null;
        lineRender.enabled = false;
    }

    private void CreateStraightWire(Wire newWire) {
        Vector3[] points = new Vector3[] { StartContact.transform.position, EndContact.transform.position };
        AddPointFromLineRenderer(newWire, points);
    }

    private void CreatePolyWire() {

    }

    private void CreateBezierWire() {

    }

    private void AddPointFromLineRenderer(Wire newWire, Vector3[] points) {
        newWire.LineRenderer.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++) {
            newWire.LineRenderer.SetPosition(i, points[i]);
            //Debug.Log("iPoint: " + points[i]);
        }
    }
}
