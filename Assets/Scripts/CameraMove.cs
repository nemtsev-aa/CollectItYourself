using UnityEngine;

public class CameraMove : MonoBehaviour {
    public Camera RaycastCamera;
    [Space(10)]
    [Tooltip("Перемещать камеру при приближении курсора к краю экрана")]
    public bool MoveWhileEdgeScreen;
    [Tooltip("Ширина зоны для перемещения камеры")]
    public float SideBorderSize = 20f;
    [Tooltip("Скорость камеры")]
    public float MoveSpeed = 10f;

    private Vector3 _startPoint;
    private Vector3 _cameraStartPosition;
    private Plane _plane;

    private void Start() {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update() {
        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);

        if (Input.GetMouseButtonDown(2)) {
            _startPoint = point;
            _cameraStartPosition = transform.position;
        }

        if (Input.GetMouseButton(2)) {
            Vector3 offset = point - _startPoint;
            transform.position = _cameraStartPosition - offset;
        }

        transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
        RaycastCamera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
    }

    void LateUpdate() {

        if (MoveWhileEdgeScreen) {
            Vector2 mousePos = Input.mousePosition; // Положение курсора
            mousePos.x /= Screen.width; // Горизонтальное положение курсора относительно ширины окна
            mousePos.y /= Screen.height; // Вертикальное положение курсора относительно высоты окна

            Vector2 delta = mousePos - new Vector2(0.5f, 0.5f); // Изменение положения мыши

            float sideBorder = Mathf.Min(Screen.width, Screen.height) / 20f; //Размер рамки по краям экрана

            float xDist = Screen.width * (0.5f - Mathf.Abs(delta.x)); // Расстояниедо от текущего положения курсора до вертикальных границ экрана
            float yDist = Screen.height * (0.5f - Mathf.Abs(delta.y)); // Расстояние от текущего положения курсора до горизонтильных границ экрана

            if (xDist < sideBorder || yDist < sideBorder) {
                delta = delta.normalized;
                delta *= Mathf.Clamp01(1 - Mathf.Min(xDist, yDist) / sideBorder);

                transform.Translate(delta * MoveSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}
