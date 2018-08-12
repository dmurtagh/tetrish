using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public BoxCollider2D [] m_SubSquares;
    public GameObject m_CenterSquare;

	// Use this for initialization
	void Start () 
    {
        m_SubSquares = GetComponentsInChildren<BoxCollider2D>();
	}
}
