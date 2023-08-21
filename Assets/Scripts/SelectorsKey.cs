using UnityEngine;

//[ExecuteAlways]
public class SelectorsKey : MonoBehaviour {
    [field: SerializeField] public ObjectView[] ObjectViews { get; private set; }
    [field: SerializeField] public ElectricFieldMovingView[] ElectricFieldMovingViews { get; private set; }

    [SerializeField] private Contact[] Contacts;
    [field: SerializeField] public Contact ActiveContact { get; private set; }

    [SerializeField] private Transform[] _keyPositions;
    [SerializeField] private Transform _movePoint;
    [SerializeField] private float _speed = 1f;

    [SerializeField] private AudioSource _audio;

    public ContactType GetActiveContactType { get { return ActiveContact.ContactType; } }

    [ContextMenu("SwitchKey")]
    public void SwitchKey() {
        if (ActiveContact.ContactType == ContactType.Closed) ActiveContact = Contacts[2];
        else ActiveContact = Contacts[1];
        
        SwitchSelector();
    }


    private void SwitchSelector() {
        Transform target;
        if (Vector3.Distance(_keyPositions[0].position, _movePoint.position) < 0.1f) target = _keyPositions[1];
        else target = _keyPositions[0];

        _movePoint.position = target.position;
        ObjectViews[0].UpdatePoints();
        _audio.Play();
    }
}
