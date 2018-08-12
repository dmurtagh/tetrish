using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public List<GamePieceSquare> m_SubSquares;
    public GamePieceSquare m_CenterSquare;

	// Use this for initialization
	void Start () 
    {
        m_SubSquares = new List<GamePieceSquare>(GetComponentsInChildren<GamePieceSquare>());
        foreach (var gamePieceSquare in m_SubSquares)
        {
            gamePieceSquare.OnDestroyFunction = (GamePieceSquare square) =>
            {
                m_SubSquares.Remove(square);
            };
        }
	}
}
