using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererExample : MonoBehaviour {
    public Transform startPoint;
    public Transform endPoint;

    private LineRenderer lineRenderer;

    void Start() {
        // Получаем компонент LineRenderer для текущего объекта
        lineRenderer = GetComponent<LineRenderer>();

        // Устанавливаем цвет линии
        lineRenderer.material.color = Color.white;

        // Задаем ширину линии
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Вызываем метод для рисования линии
        DrawLine();
    }

    void DrawLine() {
        // Создаем массив точек, состоящий из четырех отрезков
        Vector3[] points = new Vector3[5];

        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        // Рассчитываем координаты промежуточных точек
        Vector3 midPoint1 = Vector3.Lerp(startPoint.position, endPoint.position, 0.25f);
        Vector3 midPoint2 = Vector3.Lerp(startPoint.position, endPoint.position, 0.5f);
        Vector3 midPoint3 = Vector3.Lerp(startPoint.position, endPoint.position, 0.75f);

        // Задаем координаты точек в массиве
        points[0] = startPoint.position;
        points[1] = midPoint1;
        points[2] = midPoint2;
        points[3] = midPoint3;
        points[4] = endPoint.position;

        // Устанавливаем количество точек для линии
        lineRenderer.positionCount = points.Length;

        // Задаем координаты каждой точки в LineRenderer
        lineRenderer.SetPositions(points);
    }
}
