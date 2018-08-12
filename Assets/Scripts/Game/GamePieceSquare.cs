using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceSquare : MonoBehaviour 
{
    public System.Action<GamePieceSquare> OnDestroyFunction = null;
    public Vector3 StartingLocalPosition
    {
        get;
        private set;
    }

    public Rigidbody2D Rigidbody2D
    {
        get;
        set;
    }

    // For calculating if this square is part of a full row
    private bool m_FullRowDetectedThisFrame = false;
    private int m_numFullRowFrames = 0;

    // for calculating motion
    private bool m_MovedThisFrame = false;
    private int m_NumFramesWithoutMovement = 0;

    public void SetFullRowDetected()
    {
        m_FullRowDetectedThisFrame = true;
    }

    public void SetMovedThisFrame(bool value)
    {
        m_MovedThisFrame = value;
    }

    public bool IsStationary()
    {
        return m_NumFramesWithoutMovement > 30;
    }

    public void SnapPosition(Vector2 position)
    {
        Rigidbody2D.MovePosition(position);
        m_NumFramesWithoutMovement = 0;
    }

    private void Awake()
	{
        Rigidbody2D = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        StartingLocalPosition = gameObject.transform.localPosition;
	}

    void Update()
    {
        // Calculate full row made
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
            if (OnDestroyFunction != null)
            {
                OnDestroyFunction(this);
            }
            Destroy(gameObject);
        }

        // Calculate movement
        if (m_MovedThisFrame)
        {
            m_NumFramesWithoutMovement = 0;
        }
        else 
        {
            m_NumFramesWithoutMovement++;
        }
    }
}
