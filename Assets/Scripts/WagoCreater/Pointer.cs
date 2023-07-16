using UnityEngine;

public class Pointer : MonoBehaviour, IService {
    [Tooltip("������")]
    public bool Status => _status;
    
    [Tooltip("������ �������")]
    [SerializeField] private Transform _aim;
    [Tooltip("������ ������")]
    [SerializeField] private Camera _playerCamera;

    private bool _status;
    public void Init() {
        
    }

    public void SetStatus(bool status) {
        _status = status;
    }

    public Vector3 GetPosition() {
        return _aim.transform.position;
    }

    void LateUpdate() {
        if (_status) {
            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition); // ��� �� ������ ������ � ������� ������� ���� �� ������
                                                                           //Debug.DrawRay(ray.origin, ray.direction * 60f, Color.yellow); // ������������ ���� � �����
            Plane plane = new Plane(Vector3.forward, Vector3.zero); // ��������� � ������� �������� ������� �������

            // ���������� �� ������ �� ���������
            plane.Raycast(ray, out float distance); // �������� ���������� �� ������ �� ��������� � ������� ���������� ����
            Vector3 point = ray.GetPoint(distance); // ����� ����������� ���� � ���������
            _aim.position = point; // ���������� ������ � ���� ����������� ���� � ���������    
        } 
    }
}
