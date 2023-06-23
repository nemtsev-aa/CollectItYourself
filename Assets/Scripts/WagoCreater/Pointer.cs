using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [Tooltip("Объект прицела")]
    public Transform Aim;
    [Tooltip("Камера игрока")]
    [SerializeField] private Camera _playerCamera;

    void LateUpdate() {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition); // Луч из камеры игрока в позицию курсора мыши на экране
        Debug.DrawRay(ray.origin, ray.direction * 60f, Color.yellow); // Визуализация луча в сцене
        Plane plane = new Plane(-Vector3.up, Vector3.zero); // Плоскость в которой проходит игровой процесс

        // Расстояние от камеры до плоскости
        plane.Raycast(ray, out float distance); // Измеряем расстояние от камеры до плоскости с помощью созданного луча
        Vector3 point = ray.GetPoint(distance); // Точка пересечения луча и плоскости
        Aim.position = point; // Перемещаем прицел в току пересечения луча и плоскости     
    }
}
