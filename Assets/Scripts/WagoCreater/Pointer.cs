using UnityEngine;
using DG.Tweening;

public class Pointer : MonoBehaviour, IService {
    [Tooltip("������")]
    public bool Status => _status;
   
    [Tooltip("������ �������")]
    [SerializeField] private Transform _aim;
    [Tooltip("������ ������")]
    [SerializeField] private Camera _mainCamera;
    [Header("View")]
    [Tooltip("�����")]
    [SerializeField] private SpriteRenderer _point;
    [Tooltip("�������")]
    [SerializeField] private SpriteRenderer _ring;

    private bool _status;
    
    public void Init() {
        
    }

    public void SetStatus(bool status) {
        _status = status;
        Cursor.visible = !status;
    }

    public Vector3 GetPosition() {
        return _aim.transform.position;
    }

    void LateUpdate() {
        _aim.gameObject.SetActive(_status);
        if (_status) {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition); // ��� �� ������ ������ � ������� ������� ���� �� ������
            //Debug.DrawRay(ray.origin, ray.direction * 60f, Color.yellow); // ������������ ���� � �����
            Plane plane = new Plane(Vector3.forward, Vector3.zero); // ��������� � ������� �������� ������� �������

            // ���������� �� ������ �� ���������
            plane.Raycast(ray, out float distance); // �������� ���������� �� ������ �� ��������� � ������� ���������� ����
            Vector3 point = ray.GetPoint(distance); // ����� ����������� ���� � ���������
            _aim.transform.position = point; // ���������� ������ � ���� ����������� ���� � ���������  
        } 
    }

    public void Connect() {
        Transformation(_ring.transform, 0.5f);
    }

    public void Disconnect() {
        Transformation(_ring.transform, 1.5f);
    }

    private void Transformation(Transform transform, float newScale) {
        float duration = 0.3f;
        // ��������� �������� ������ �������
        Vector3 originalScale = transform.localScale;

        // �������� ������ ������� �� 50% � ������� DOTween
        transform.DOScale(originalScale * newScale, duration)
            .OnComplete(() => {
                // �� ���������� ��������� �������, ������ ���������� ������ � �������� ���������
                transform.DOScale(originalScale, duration);
            });
    }
}
