using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagoClipsDragPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] _dragWagoClipPrefabs;
    [SerializeField] private Transform _scrollViewContent;

    private void Start() {
        for (int i = 0; i < _dragWagoClipPrefabs.Length; i++) {
            var dragObject = Instantiate(_dragWagoClipPrefabs[i], _scrollViewContent);
            // Инициализация элемента
        }
    }
}
