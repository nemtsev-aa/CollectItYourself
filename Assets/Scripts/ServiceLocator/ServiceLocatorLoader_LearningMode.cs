using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class ServiceLocatorLoader_LearningMode : MonoBehaviour
{
    [SerializeField] private GUIHolder _guiHolder;
    
    private EventBus _eventBus;
    private LearningProgressManager _learningProgressManager;
    private GoldController _goldController;
    private ScoreController _scoreController;

    private ITaskLoader _levelLoader;

    private List<IDisposable> _disposables = new List<IDisposable>();
}
