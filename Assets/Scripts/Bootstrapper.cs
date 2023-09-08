using CustomEventBus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour {
    [SerializeField] private ApplicationController _applicationController;                  // �������� ��������� ����������
    [SerializeField] private SavesManager _savesManager;                                    // �������� ����������
    private EventBus _eventBus;

    private void Awake() {
        // ������������� ���������� ��������
        ServiceLocator.Initialize();
        _eventBus = new EventBus();
        
        RegisterServices();
        Init();
        SceneManager.LoadScene(1);
    }

    private void RegisterServices() {
        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register(_applicationController);
        ServiceLocator.Current.Register(_savesManager);

    }

    private void Init() {
        _applicationController.Init(_eventBus);
        _savesManager.Init();
    }
}