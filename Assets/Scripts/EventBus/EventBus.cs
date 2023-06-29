using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus 
{
    private EventBus() {

    }

    private static EventBus _instance;

    public static EventBus Instance {
        get {
            if (_instance == null) _instance = new EventBus();
            return _instance;
        }
    }

    // ���������� �������� ���������
    public event Action<int> ProgressValueChanget;

    // �������� ����������������� �������
    public Action<SingleSwitchingResult> SingleCorrectChecked;
    public Action<SingleSwitchingResult> SingleIncorrectChecked;

    public Action<SwitchingResult> CorrectChecked;
    public Action<SwitchingResult> IncorrectChecked;
} 
