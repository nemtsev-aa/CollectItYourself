using UnityEngine;
using DG.Tweening;
using CustomEventBus.Signals;
using CustomEventBus;

public class Pointer : MonoBehaviour, IService, IDisposable {
    [Tooltip("Статус")]
    public bool Status => _status;
   
    [Tooltip("Объект прицела")]
    [SerializeField] private Transform _aim;
    [Tooltip("Камера игрока")]
    [SerializeField] private Camera _mainCamera;
    [Header("View")]
    [Tooltip("Точка")]
    [SerializeField] private SpriteRenderer _point;
    [Tooltip("Обводка")]
    [SerializeField] private SpriteRenderer _ring;

    private bool _status;
    private EventBus _eventBus;

    public void Init(EventBus eventBus) {
        _eventBus = eventBus;
        _eventBus.Subscribe((TrainingModeStopSignal signal) => SetStatus(false));
        _eventBus.Subscribe((TrainingModeStartSignal signal) => SetStatus(true));
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
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition); // Луч из камеры игрока в позицию курсора мыши на экране
            //Debug.DrawRay(ray.origin, ray.direction * 60f, Color.yellow); // Визуализация луча в сцене
            Plane plane = new Plane(Vector3.forward, Vector3.zero); // Плоскость в которой проходит игровой процесс

            // Расстояние от камеры до плоскости
            plane.Raycast(ray, out float distance); // Измеряем расстояние от камеры до плоскости с помощью созданного луча
            Vector3 point = ray.GetPoint(distance); // Точка пересечения луча и плоскости
            _aim.transform.position = point; // Перемещаем прицел в току пересечения луча и плоскости  
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
        // Сохраняем исходный размер объекта
        Vector3 originalScale = transform.localScale;

        // Изменяем размер объекта на 50% с помощью DOTween
        transform.DOScale(originalScale * newScale, duration)
            .OnComplete(() => {
                // По завершению изменения размера, плавно возвращаем объект в исходное состояние
                transform.DOScale(originalScale, duration);
            });
    }

    public void Dispose() {
        _eventBus.Unsubscribe((TrainingModeStopSignal signal) => SetStatus(false));
        _eventBus.Unsubscribe((TrainingModeStartSignal signal) => SetStatus(true));
    }
}
