using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

[System.Serializable]
[CreateAssetMenu(fileName = nameof(SingleSwitchingResult), menuName = nameof(SingleSwitchingResult))]
public class SingleSwitchingResult : ScriptableObject {
    public string TaskName;
    public int SwitchBoxNumer;
    public string ErrorCountText;
    public string SwitchingTimeText;
    public float SwitchingTimeValue;
    public List<ConnectionData> ErrorList;
}
