using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private bool _wasSet; // ������� ������ ��������� ���������

    public virtual void Init(GameStateManager gameStateManager)
    {
        
    }

    public virtual void EnterFirstTime()
    {

    }

    public virtual void Enter()
    {
        if (!_wasSet) // ���� ��������� ������������ �������
        {
            EnterFirstTime();
            _wasSet = true;
        }
    }

    public virtual void Exit()
    {

    }
}
