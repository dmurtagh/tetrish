using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettablePosition : AResettable
{
    Vector2 m_StartPosition;
    Vector2 m_StartingVelocity;
    float m_StartingRotation;
    float m_StartingAngularVelocity;
    Rigidbody2D m_Rigidbody2d;

    // Use this for initialization
    void Start ()
    {
        m_Rigidbody2d = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        m_StartPosition = m_Rigidbody2d.position;
        m_StartingVelocity = m_Rigidbody2d.velocity;
        m_StartingRotation = m_Rigidbody2d.rotation;
        m_StartingAngularVelocity = m_Rigidbody2d.angularVelocity;
    }
	
	// Update is called once per frame
	public override void Reset ()
    {
        m_Rigidbody2d.position = m_StartPosition;
        m_Rigidbody2d.velocity = m_StartingVelocity;
        m_Rigidbody2d.rotation = m_StartingRotation;
        m_Rigidbody2d.angularVelocity = m_StartingAngularVelocity;
    }
}
