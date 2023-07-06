using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererExample : MonoBehaviour {
    public Transform startPoint;
    public Transform endPoint;

    private LineRenderer lineRenderer;

    void Start() {
        // �������� ��������� LineRenderer ��� �������� �������
        lineRenderer = GetComponent<LineRenderer>();

        // ������������� ���� �����
        lineRenderer.material.color = Color.white;

        // ������ ������ �����
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // �������� ����� ��� ��������� �����
        DrawLine();
    }

    void DrawLine() {
        // ������� ������ �����, ��������� �� ������� ��������
        Vector3[] points = new Vector3[5];

        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        // ������������ ���������� ������������� �����
        Vector3 midPoint1 = Vector3.Lerp(startPoint.position, endPoint.position, 0.25f);
        Vector3 midPoint2 = Vector3.Lerp(startPoint.position, endPoint.position, 0.5f);
        Vector3 midPoint3 = Vector3.Lerp(startPoint.position, endPoint.position, 0.75f);

        // ������ ���������� ����� � �������
        points[0] = startPoint.position;
        points[1] = midPoint1;
        points[2] = midPoint2;
        points[3] = midPoint3;
        points[4] = endPoint.position;

        // ������������� ���������� ����� ��� �����
        lineRenderer.positionCount = points.Length;

        // ������ ���������� ������ ����� � LineRenderer
        lineRenderer.SetPositions(points);
    }
}
