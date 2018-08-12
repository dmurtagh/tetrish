using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public List<GamePieceSquare> m_SubSquares;
    public GamePieceSquare m_CenterSquare;

    private static int numSquaresRemoved = 1;

	// Use this for initialization
	void Start () 
    {
        m_SubSquares = new List<GamePieceSquare>(GetComponentsInChildren<GamePieceSquare>());
        foreach (var gamePieceSquare in m_SubSquares)
        {
            gamePieceSquare.OnDestroyFunction = (GamePieceSquare square) =>
            {
                m_SubSquares.Remove(square);
                numSquaresRemoved++;
                if (numSquaresRemoved == 10)
                {
                    numSquaresRemoved = 0;
                    ScoreController.Instance.Lines++;
                    ScoreController.Instance.Score += 100;
                }
            };
        }

        // Turn off gravity for a second
        foreach(var square in m_SubSquares)
        {
            square.Rigidbody2D.gravityScale = 0;
        }

        Invoke("TurnOnGravity", BoardController.Instance.m_IntroWaitTime);
	}

    void TurnOnGravity()
    {
        foreach (var square in m_SubSquares)
        {
            square.Rigidbody2D.gravityScale = 1;
        }
    }
}
