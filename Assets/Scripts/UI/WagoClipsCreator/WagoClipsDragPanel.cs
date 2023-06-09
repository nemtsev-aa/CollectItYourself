using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WagoClipData {
    public WagoType WagoType;
    public Sprite Sprite;
    public GameObject Prefab;
}

public class WagoClipsDragPanel : MonoBehaviour
{
    [SerializeField] private GameObject _dragWagoClipPrefab;
    [SerializeField] private List<WagoClipData> _dragWago = new ();
    [SerializeField] private Transform _scrollViewContent;
    [SerializeField] private WagoCreator _wagoCreator;

    private void Start() {
        for (int i = 0; i < _dragWago.Count; i++) {
            var dragObject = Instantiate(_dragWagoClipPrefab, _scrollViewContent);

            var script = dragObject.GetComponent<WagoClipsDragElement>();
            script.MainSprite = _dragWago[i].Sprite;
            script.CurrentData = _dragWago[i];
            script.WagoCreator = _wagoCreator;
            script.DefaultParentTransform = _scrollViewContent;
            script.DragParentTransform = transform;
            script.SiblingIndex = i;
        }
    }
}
