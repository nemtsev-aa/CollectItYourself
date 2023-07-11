using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WagoClipsDragElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _mainRenderer;
    //[SerializeField] private SpriteRenderer _mainRenderer;

    private Sprite _mainSprite;
    private Transform _defaultParentTransform;
    private Transform _dragParentTransform;
    private int _siblingIndex;
    private WagoClipData _currentData;
    private WagoCreator _wagoCreator;
    private WagoClip _newWagoClip;
    private Vector3 _defaultPosition;

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

    private void Start() {
        _defaultPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        transform.SetParent(DragParentTransform);
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;;
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(DefaultParentTransform);
        transform.SetSiblingIndex(SiblingIndex);

        transform.position = _defaultPosition;

        _newWagoClip = _wagoCreator.CreateWago(CurrentData);
    }
}
