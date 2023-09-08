/*
https://oxmond.com/how-to-build-a-video-player-with-scrub-control-in-unity/
*/
using UnityEngine;
public class Knob : MonoBehaviour {
    [SerializeField] private MyVideoPlayer _videoPlayer;
    
    void OnMouseDown() => _videoPlayer.KnobOnPressDown();
    void OnMouseUp() => _videoPlayer.KnobOnRelease();
    void OnMouseDrag() => _videoPlayer.KnobOnDrag();
}
