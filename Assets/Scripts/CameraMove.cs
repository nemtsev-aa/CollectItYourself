using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

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
    private Vector3 _defaultPosition;
    private Plane _plane;
    private EventBus _eventBus;

    private bool _isOverUI;
    private void Start() {
        _defaultPosition = transform.position;
        _plane = new Plane(Vector3.forward, Vector3.zero);
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<ActiveSwitchBoxChangedSignal>(MoveToActiveSwitchBox);  // Перемещение к активной распределительной коробке
    }

    private void Update() {
        _isOverUI = EventSystem.current.IsPointerOverGameObject();

        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.direction * 10f, Color.blue); // Визуализация луча

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

    private void MoveToActiveSwitchBox(ActiveSwitchBoxChangedSignal signal) {
        Vector3 activeSwitchBoxPosition = signal.SwitchBox.transform.localPosition;
        float zPosition = 0f; ;
        if (signal.SwitchBox.SwitchBoxData.PartNumber == 3) {
            zPosition = _defaultPosition.z - 3f;
        } else {
            zPosition = _defaultPosition.z;
        }
        Vector3 newCameraPosition = new Vector3(activeSwitchBoxPosition.x, activeSwitchBoxPosition.y, zPosition);
        transform.DOMove(newCameraPosition, 0.35f).SetEase(Ease.OutFlash);
    }
}
