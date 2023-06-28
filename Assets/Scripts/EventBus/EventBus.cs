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

    // Проверка распределительной каробки
    public Action<SwitchingResult> CorrectChecked;
    public Action<SwitchingResult> IncorrectChecked;

} 
