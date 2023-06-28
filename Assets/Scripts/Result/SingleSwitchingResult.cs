using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSwitchingResult : SwitchingResult
{
    public int SwitchBoxNumer;
    public string ErrorCountText;
    public string SwitchingTimeText;
    public float SwitchingTimeValue;
    public List<ConnectionData> ErrorList = new List<ConnectionData>();
}
