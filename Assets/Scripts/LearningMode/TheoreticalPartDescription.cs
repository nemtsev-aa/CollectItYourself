using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = nameof(TheoreticalPartDescription), menuName = "LearningMode/" + nameof(TheoreticalPartDescription))]
[System.Serializable]
public class TheoreticalPartDescription : ParagraphDescription {
    public VideoClip TheoreticalVideoClip;
}