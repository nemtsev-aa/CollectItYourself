using UnityEngine;

public class Pointer : MonoBehaviour, IService {
    [Tooltip("������ �������")]
    public Transform Aim;
    [Tooltip("������ ������")]
    [SerializeField] private Camera _playerCamera;

    public void Init() {
        
    }

    void LateUpdate() {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition); // ��� �� ������ ������ � ������� ������� ���� �� ������
        //Debug.DrawRay(ray.origin, ray.direction * 60f, Color.yellow); // ������������ ���� � �����
        Plane plane = new Plane(Vector3.forward, Vector3.zero); // ��������� � ������� �������� ������� �������

        // ���������� �� ������ �� ���������
        plane.Raycast(ray, out float distance); // �������� ���������� �� ������ �� ��������� � ������� ���������� ����
        Vector3 point = ray.GetPoint(distance); // ����� ����������� ���� � ���������
        Aim.position = point; // ���������� ������ � ���� ����������� ���� � ���������     
    }
}
