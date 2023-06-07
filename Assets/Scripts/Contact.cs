using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ContactType {
    Line,
    Neutral,
    GroundConductor,
    Closed,
    Open
}

public class Contact : SelectableObject
{
    /// <summary>
    /// Тип контакта
    /// </summary>
    public ContactType CurrentContactType;
    public Material Material;

    public override void OnHover() {
        transform.localScale = Vector3.one * 1.5f;
    }
}
