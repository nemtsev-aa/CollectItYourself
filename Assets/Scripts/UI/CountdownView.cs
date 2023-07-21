using DG.Tweening;
using System.Collections;
using TMPro;
using UI.Dialogs;
using UnityEngine;
using UnityEngine.UI;

public class CountdownView : MonoBehaviour {
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] int duration = 3;
    [SerializeField] private TextMeshProUGUI _timeValueText;

    private CountdownDialog _countdownDialog;
    private TrainingSwitchingDialog _trainingSwitchingDialog;
    private Coroutine _countdown;

    public void Init(CountdownDialog countdownDialog) {
        _countdownDialog = countdownDialog;
        _cancelButton.onClick.AddListener(ReturnFromSelectionDialog);
        _startButton.onClick.AddListener(CountdownComplete);
        _countdown = StartCoroutine(nameof(Countdown));
    }

    private IEnumerator Countdown() {
        for (int i = duration; i >= 0; i--) {
            _timeValueText.text = i.ToString();
            // —оздаем последовательную анимацию с помощью DOTween
            Sequence countdownSequence = DOTween.Sequence().SetEase(Ease.Linear);
            // ƒобавл€ем анимацию дл€ каждого числа обратного отсчета
            countdownSequence.Append(_timeValueText.transform.DOScale(Vector3.one, 0.5f));
            countdownSequence.Append(_timeValueText.transform.DOScale(Vector3.zero, 0.5f));
            // ”станавливаем продолжительность всей анимации
            countdownSequence.Play();

            yield return new WaitForSeconds(1);
        }
        CountdownComplete();
    }

    private void CountdownComplete() {
        StopCoroutine(_countdown);
        _countdownDialog.OnCountdownFinish.Invoke(true);
        _countdownDialog.Hide();
    }

    private void ReturnFromSelectionDialog() {
        StopCoroutine(_countdown);
        _countdownDialog.OnCountdownFinish.Invoke(false);
        _countdownDialog.Hide();
    }
}
