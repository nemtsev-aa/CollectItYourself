using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct WagoClipData {
    public WagoType WagoType;
    public Sprite Sprite;
    public GameObject Prefab;
}

public class WagoClipsDragPanel : MonoBehaviour, IPointerEnterHandler {
    [SerializeField] private GameObject _dragWagoClipPrefab;
    [SerializeField] private List<WagoClipData> _dragWago = new ();
    [SerializeField] private Transform _scrollViewContent;
    [SerializeField] private WagoCreator _wagoCreator;
    [SerializeField] private Management _management;
    
    public void Init(WagoCreator wagoCreator) {
        _wagoCreator = wagoCreator;
        _management = ServiceLocator.Current.Get<Management>();

        for (int i = 0; i < _dragWago.Count; i++) {
            var dragObject = Instantiate(_dragWagoClipPrefab, _scrollViewContent);

            var script = dragObject.GetComponent<WagoClipsDragElement>();
            script.CreationZone = true;
            script.MainSprite = _dragWago[i].Sprite;
            script.CurrentData = _dragWago[i];
            script.WagoCreator = _wagoCreator;
            script.DefaultParentTransform = _scrollViewContent;
            script.DragParentTransform = transform;
            script.SiblingIndex = i;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        WagoClip wagoClip = _management.GetSelectionWagoClip();
        if (Input.GetMouseButton(0) && wagoClip != null) {
            wagoClip.DeleteClip();
        } 
    }
}
