using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private bool _wasSet; // Отметка первой активации состояния

    public virtual void Init(GameStateManager gameStateManager)
    {
        
    }

    public virtual void EnterFirstTime()
    {

    }

    public virtual void Enter()
    {
        if (!_wasSet) // Если состояние активируется впервые
        {
            EnterFirstTime();
            _wasSet = true;
        }
    }

    public virtual void Exit()
    {

    }
}
