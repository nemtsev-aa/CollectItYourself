using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingResult 
{
    public string TaskName { get; set; }
    public int SwitchBoxNumber { get; set; }
    public int ErrorCount { get; set; }
    public List<ConnectionData> ErrorList { get; set; }
    public float SwitchingTimeValue { get; set; }

}
