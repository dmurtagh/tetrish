using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Key mappings from http://wiki.unity3d.com/index.php/Xbox360Controller
 */
public class InputManager : Singleton<InputManager>
{
    public float m_ControllerDeadzone = 0.11f;

    public bool GetJumpKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Space) || 
                    // A button (button 0 on windows, 16 on mac)
                    Input.GetKeyDown(KeyCode.Joystick1Button0) || 
                    Input.GetKeyDown(KeyCode.Joystick1Button16);  
    }

    public bool GetResetKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Escape) || 
                    // LT Button (button 6 on windows, 10 on mac)
                    Input.GetKeyDown(KeyCode.Joystick1Button4) ||
                    Input.GetKeyDown(KeyCode.Joystick1Button13);
    }

    public bool GetNextTrackKeyDown()
    {
        return Input.GetKeyDown(KeyCode.N) || 
                    // Left bumper (button 4 on windows, 13 on mac)
                    Input.GetKeyDown(KeyCode.Joystick1Button4) || 
                    Input.GetKeyDown(KeyCode.Joystick1Button13);
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
