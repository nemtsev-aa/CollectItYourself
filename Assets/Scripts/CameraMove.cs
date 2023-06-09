using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour {
    public Camera RaycastCamera;
    [Space(10)]
    [Tooltip("���������� ������ ��� ����������� ������� � ���� ������")]
    public bool MoveWhileEdgeScreen;
    [Tooltip("������ ���� ��� ����������� ������")]
    public float SideBorderSize = 20f;
    [Tooltip("�������� ������")]
    public float MoveSpeed = 10f;

    private Vector3 _startPoint;
    private Vector3 _cameraStartPosition;
    private Plane _plane;

    private bool _isOverUI;
    private void Start() {
        _plane = new Plane(Vector3.forward, Vector3.zero);
    }

    private void Update() {
        _isOverUI = EventSystem.current.IsPointerOverGameObject();

        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.direction * 10f, Color.blue); // ������������ ����

        float distance;
        _plane.Raycast(ray, out distance);
        
        Vector3 point = ray.GetPoint(distance);

        if (Input.GetMouseButtonDown(2)) {
            _startPoint = point;
            _cameraStartPosition = transform.localPosition;
        }

        if (Input.GetMouseButton(2)) {
            Vector3 offset = point - _startPoint;
            transform.localPosition = _cameraStartPosition - offset;
        }

        if (!_isOverUI) {
            transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
            RaycastCamera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
        }
    }

    void LateUpdate() {

        if (MoveWhileEdgeScreen) {
            Vector2 mousePos = Input.mousePosition; // ��������� �������
            mousePos.x /= Screen.width; // �������������� ��������� ������� ������������ ������ ����
            mousePos.y /= Screen.height; // ������������ ��������� ������� ������������ ������ ����

            Vector2 delta = mousePos - new Vector2(0.5f, 0.5f); // ��������� ��������� ����

            float sideBorder = Mathf.Min(Screen.width, Screen.height) / 20f; //������ ����� �� ����� ������

            float xDist = Screen.width * (0.5f - Mathf.Abs(delta.x)); // ������������ �� �������� ��������� ������� �� ������������ ������ ������
            float yDist = Screen.height * (0.5f - Mathf.Abs(delta.y)); // ���������� �� �������� ��������� ������� �� �������������� ������ ������

            if (xDist < sideBorder || yDist < sideBorder) {
                delta = delta.normalized;
                delta *= Mathf.Clamp01(1 - Mathf.Min(xDist, yDist) / sideBorder);

                transform.Translate(delta * MoveSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}
