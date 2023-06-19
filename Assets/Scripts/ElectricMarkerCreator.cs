using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricMarkerCreator : MonoBehaviour
{
    [SerializeField] private MoveAlongLine _markerPrefab;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _delay = 0.5f;
    [SerializeField] private int _markCount = 10;
    [SerializeField] private float _markSpeed = 2f;

    [SerializeField] private List<MoveAlongLine> _markerList = new ();

    private float _time;
    private int _markCurrentCount = 0;

    private void Update() {
        if (_markCurrentCount < _markCount) {
            _time += Time.deltaTime;
            if (_time >= _delay) {
                _time = 0;
                MoveAlongLine newMark = Instantiate(_markerPrefab, new Vector3(_line.GetPosition(0).x, _line.GetPosition(0).y, _line.GetPosition(0).z), Quaternion.Euler(new Vector3(90, 0, 0)));
                newMark.Line = _line;
                newMark.Speed = _markSpeed;
                _markerList.Add(newMark);
                _markCurrentCount++;
            }
        }
    }
}
