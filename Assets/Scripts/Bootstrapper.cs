using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameStateManager _gameStateManager;

    private void Awake() {
        _gameStateManager.Init();
    }
}
