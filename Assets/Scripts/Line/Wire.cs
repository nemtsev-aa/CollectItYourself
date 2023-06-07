using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WireType {
    Straight,
    Poly,
    Bezier
}
public class Wire : MonoBehaviour {
    public SwitchBox SwitchBox;

    public WireType Type;
    public Contact StartContact;
    public Contact EndContact;

    public LineRenderer LineRenderer;
}
