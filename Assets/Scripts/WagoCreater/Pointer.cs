using UnityEngine;

public class Pointer : MonoBehaviour, IService {
    [Tooltip("Статус")]
    public bool Status => _status;
    
    [Tooltip("Объект прицела")]
    [SerializeField] private Transform _aim;
    [Tooltip("Камера игрока")]
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
            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition); // Луч из камеры игрока в позицию курсора мыши на экране
                                                                           //Debug.DrawRay(ray.origin, ray.direction * 60f, Color.yellow); // Визуализация луча в сцене
            Plane plane = new Plane(Vector3.forward, Vector3.zero); // Плоскость в которой проходит игровой процесс

            // Расстояние от камеры до плоскости
            plane.Raycast(ray, out float distance); // Измеряем расстояние от камеры до плоскости с помощью созданного луча
            Vector3 point = ray.GetPoint(distance); // Точка пересечения луча и плоскости
            _aim.position = point; // Перемещаем прицел в току пересечения луча и плоскости    
        } 
    }
}
