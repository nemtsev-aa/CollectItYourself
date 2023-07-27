using EPOOutline;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WagoClipsDragElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private Image _mainRenderer;
    //[SerializeField] private SpriteRenderer _mainRenderer;
    [SerializeField] private Image _outlinable;
    public bool CreationZone;

    private Sprite _mainSprite;
    private Transform _defaultParentTransform;
    private Transform _dragParentTransform;
    private int _siblingIndex;
    private WagoClipData _currentData;
    private WagoCreator _wagoCreator;
    private WagoClip _newWagoClip;
    private Vector3 _defaultPosition;

    private Vector3 _startDragPosition;
    private float _distantion;

    public Sprite MainSprite {
        get { return _mainSprite; }
        set {
            if (value != null) {
                _mainSprite = value;
                _mainRenderer.sprite = value;
            }
        }
    }
    public Transform DefaultParentTransform {
        get { return _defaultParentTransform; }
        set {
            if (value != null) {
                _defaultParentTransform = value;
            }
        }
    }
    public Transform DragParentTransform {
        get { return _dragParentTransform; }
        set {
            if (value != null) {
                _dragParentTransform = value;
            }
        }
    }
    public int SiblingIndex {
        get { return _siblingIndex; }
        set {
            if (value > 0) {
                _siblingIndex = value;
            }
        }
    }
    public WagoClipData CurrentData {
        get { return _currentData; }
        set {
            _currentData = value;
        }
    }
    public WagoCreator WagoCreator {
        get { return _wagoCreator; }
        set {
            _wagoCreator = value;
        }
    }

    private bool This_is_UI;

    private void Start() {
        _defaultPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        transform.SetParent(DragParentTransform);
        _outlinable.enabled = false;
        _startDragPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
        _distantion = Vector3.Distance(_startDragPosition, transform.position);
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(DefaultParentTransform);
        transform.SetSiblingIndex(SiblingIndex);

        transform.position = _defaultPosition;
        if (CheckEndDragPoint(eventData)) {
            _newWagoClip = _wagoCreator.CreateWago(_currentData);
        }
    }

    private bool CheckEndDragPoint(PointerEventData eventData) {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        if (raycastResults.Count > 0) {
            foreach (var go in raycastResults) {
                Debug.Log(go.gameObject.name);
                if (go.gameObject.GetComponent<WagoClipsDragPanel>() != null) {
                    Debug.Log("Не покинул зону создания");
                    return false;
                }
            }
        }
        return true;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _outlinable.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        _outlinable.enabled = false;
    }
}
