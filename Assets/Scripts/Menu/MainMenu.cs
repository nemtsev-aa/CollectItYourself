using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Tooltip("Кнопка - Обучение")]
    [SerializeField] private Button _learningButton;
    [Tooltip("Кнопка - Тренировка")]
    [SerializeField] private Button _trainingButton;
    [Tooltip("Кнопка - Экзамен")]
    [SerializeField] private Button _examButton;

    private void Start()
    {
        _learningButton.onClick.AddListener(StartLearning);
        _trainingButton.onClick.AddListener(StartTraining);
        _examButton.onClick.AddListener(StartExam);
    }

    private void StartExam() {
        SceneManager.LoadScene(3);
    }

    private void StartTraining() {
        SceneManager.LoadScene(2);
    }

    private void StartLearning()
    {
        SceneManager.LoadScene(1); 
    }
}
