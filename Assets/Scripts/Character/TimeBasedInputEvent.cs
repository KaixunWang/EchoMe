using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 基于时间的输入事件记录
[System.Serializable]
public class TimeBasedInputEvent
{
    public float timestamp;
    public InputType inputType;
    public bool isPressed; // true for keydown, false for keyup
    public Vector2 position; // 记录位置用于同步
    
    public TimeBasedInputEvent(float time, InputType type, bool pressed, Vector2 pos)
    {
        timestamp = time;
        inputType = type;
        isPressed = pressed;
        position = pos;
    }
}
