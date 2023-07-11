using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Dialogs {
    /// <summary>
    /// Окно меню (используется на титульной сцене)
    /// </summary>
    public class MainMenuDialog : Dialog {
        [SerializeField] private Button _learningButton;
        [SerializeField] private Button _trainingButton;
        [SerializeField] private Button _examButton;

        protected override void Awake() {
            base.Awake();
            _learningButton.onClick.AddListener(OnLearningButtonClick);
            _trainingButton.onClick.AddListener(OnTrainingButtonClick);
            _examButton.onClick.AddListener(OnExamButtonClick);
        }

        private void OnLearningButtonClick() {
            SceneManager.LoadScene(1);
            //DialogManager.ShowDialog<SelectLearningChapterDialog>();
        }

        private void OnTrainingButtonClick() {
            SceneManager.LoadScene(2);
            //DialogManager.ShowDialog<SelectTrainingTaskDialog>();
        }

        private void OnExamButtonClick() {
            SceneManager.LoadScene(3);
            //DialogManager.ShowDialog<SelectExamTaskDialog>();
        }
    }
}
