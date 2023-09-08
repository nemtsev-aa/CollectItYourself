/*
https://oxmond.com/how-to-build-a-video-player-with-scrub-control-in-unity/
*/

using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.UI;

public class MyVideoPlayer : MonoBehaviour {
    [SerializeField] private GameObject cinemaPlane;
    [Header("Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _pauseButton;
    [Header("Progress Bar")]
    [SerializeField] private GameObject _knob;
    [SerializeField] private GameObject _pbValue;
    [SerializeField] private GameObject _pbBackground;

    private float _maxKnobX;
    private float _minKnobX;
    private float newKnobY;
    private float _simpleKnobValue;
    private float _pbWidth;
    private bool _knobIsDragging;
    private bool _videoIsJumping = false;
    private bool _videoIsPlaying = false;
    private VideoPlayer _videoPlayer;
    private RectTransform _pbValueRt;

    public void Init (VideoClip videoClip) {
        newKnobY = _knob.transform.localPosition.y;
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.frame = (long)100;
        _videoPlayer.clip = videoClip;

        _pauseButton.gameObject.SetActive(false);
        _pauseButton.onClick.AddListener(BtnPlayVideo);
        _playButton.gameObject.SetActive(true);
        _playButton.onClick.AddListener(BtnPlayVideo);

        _pbValueRt = _pbValue.GetComponent<RectTransform>();
        _pbWidth = _pbBackground.GetComponent<RectTransform>().sizeDelta.x;
    }

    private void Update() {
        if (!_knobIsDragging && !_videoIsJumping) {
            if (_videoPlayer.frameCount > 0) {
                float progress = (float)_videoPlayer.frame / _videoPlayer.frameCount;
                float offsetX = _pbWidth * progress;

                _pbValueRt.sizeDelta = new Vector2(offsetX, _pbValueRt.sizeDelta.y);
                _knob.transform.localPosition = new Vector2(_pbValueRt.localPosition.x + offsetX, _knob.transform.localPosition.y);
            }
        }
    }

    public void KnobOnPressDown() {
        VideoStop();
        _minKnobX = _pbValue.transform.localPosition.x;
        _maxKnobX = _minKnobX + _pbWidth;
    }

    public void KnobOnRelease() {
        _knobIsDragging = false;
        CalcKnobSimpleValue();
        VideoPlay();
        VideoJump();
        StartCoroutine(DelayedSetVideoIsJumpingToFalse());
    }

    IEnumerator DelayedSetVideoIsJumpingToFalse() {
        yield return new WaitForSeconds(0.3f);
        SetVideoIsJumpingToFalse();
    }

    public void KnobOnDrag() {
        _knobIsDragging = true;
        _videoIsJumping = true;

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _knob.transform.position = new Vector2(curPosition.x, curPosition.y);

        float newKnobX = _knob.transform.localPosition.x;
        
        if (newKnobX > _maxKnobX) { newKnobX = _maxKnobX; }
        if (newKnobX < _minKnobX) { newKnobX = _minKnobX; }

        _knob.transform.localPosition = new Vector2(newKnobX, newKnobY);
        CalcKnobSimpleValue();
        _pbValueRt.sizeDelta = new Vector2(_simpleKnobValue * _pbWidth, _pbValueRt.sizeDelta.y);
    }

    private void CalcKnobSimpleValue() {
        float maxKnobValue = _maxKnobX - _minKnobX;
        float knobValue = _knob.transform.localPosition.x - _minKnobX;
        _simpleKnobValue = knobValue / maxKnobValue;
    }

    private void SetVideoIsJumpingToFalse() {
        _videoIsJumping = false;
    }

    private void VideoJump() {
        var frame = _videoPlayer.frameCount * _simpleKnobValue;
        _videoPlayer.frame = (long)frame;
    }

    private void BtnPlayVideo() {
        if (_videoIsPlaying) VideoStop();
        else VideoPlay();
    }

    private void VideoStop() {
        _videoIsPlaying = false;
        _videoPlayer.Pause();
        _pauseButton.gameObject.SetActive(false);
        _playButton.gameObject.SetActive(true);
    }

    private void VideoPlay() {
        _videoIsPlaying = true;
        _videoPlayer.Play();
        _pauseButton.gameObject.SetActive(true);
        _playButton.gameObject.SetActive(false);
    }
}
