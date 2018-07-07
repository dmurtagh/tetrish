using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public float m_ControllerDeadzone = 0.11f;

    public bool GetJumpKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0);
    }

    public bool GetResetKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6);
    }

    public float GetLeftStickHorizontal()
    {
        float keyboardVal = Input.GetAxis("Horizontal");
        float gamepadVal = Input.GetAxis("joy_0_axis_0");

        if (Mathf.Abs(keyboardVal) > Mathf.Abs(gamepadVal))
        {
            return keyboardVal;
        }
        else
        {
            if (Mathf.Abs(gamepadVal) < m_ControllerDeadzone)
            {
                return 0f;
            }
            return gamepadVal;
        }
    }
}
