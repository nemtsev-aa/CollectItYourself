using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [Tooltip("������ �������")]
    public Transform Aim;
    [Tooltip("������ ������")]
    [SerializeField] private Camera _playerCamera;

    void LateUpdate() {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition); // ��� �� ������ ������ � ������� ������� ���� �� ������
        Debug.DrawRay(ray.origin, ray.direction * 60f, Color.yellow); // ������������ ���� � �����
        Plane plane = new Plane(-Vector3.up, Vector3.zero); // ��������� � ������� �������� ������� �������

        // ���������� �� ������ �� ���������
        plane.Raycast(ray, out float distance); // �������� ���������� �� ������ �� ��������� � ������� ���������� ����
        Vector3 point = ray.GetPoint(distance); // ����� ����������� ���� � ���������
        Aim.position = point; // ���������� ������ � ���� ����������� ���� � ���������     
    }
}
