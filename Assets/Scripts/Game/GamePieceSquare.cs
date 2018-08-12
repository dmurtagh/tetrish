using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceSquare : MonoBehaviour 
{
    private bool m_FullRowDetectedThisFrame = false;
    private int m_numFullRowFrames = 0;

    public void SetFullRowDetected()
    {
        m_FullRowDetectedThisFrame = true;
    }

    void Update()
    {
        if (m_FullRowDetectedThisFrame)
        {
            m_numFullRowFrames++;
        }
        else
        {
            m_numFullRowFrames = 0;
        }
        m_FullRowDetectedThisFrame = false;

        if (m_numFullRowFrames > 15)
        {
            Destroy(gameObject);
        }
    }
}
