using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceSquare : MonoBehaviour 
{
    public System.Action<GamePieceSquare> OnDestroyFunction = null;

    public Rigidbody2D m_Rigidbody2D;
    private bool m_FullRowDetectedThisFrame = false;
    private int m_numFullRowFrames = 0;

	private void Start()
	{
        m_Rigidbody2D = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
	}

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
            if (OnDestroyFunction != null)
            {
                OnDestroyFunction(this);
            }
            Destroy(gameObject);
        }
    }
}
