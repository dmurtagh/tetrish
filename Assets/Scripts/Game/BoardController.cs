using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour 
{
    public GameObject [] m_PiecePrefabs;
    public GameObject m_BoardRoot;
    public GameObject m_RowCheckerPrefab;
    public LayerMask m_BlocksLayerMask;

    private List<GamePiece> m_GamePieces;
    private int kNumColumns = 10;
    private int kNumRows = 20;
    private List<BoxCollider2D> m_RowCheckers;

	// Use this for initialization
	void Start () 
    {
        m_GamePieces = new List<GamePiece>();
        m_RowCheckers = new List<BoxCollider2D>();
        RectTransform rectTransform = m_BoardRoot.transform as RectTransform;
        Vector3 position = rectTransform.position;
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        float step = rectTransform.sizeDelta.y / kNumRows;

        for (int i = 0; i < kNumRows; i++)
        {
            GameObject newGameObject = Instantiate(m_RowCheckerPrefab, new Vector3(0, position.y + step * i, position.z), Quaternion.identity, m_BoardRoot.transform);
            m_RowCheckers.Add(newGameObject.GetComponent(typeof(BoxCollider2D)) as BoxCollider2D);
        }
        StartCoroutine(SpawnerCoroutine());
	}

    private void Update()
    {
        m_GamePieces.RemoveAll(IsDestroyedPredicate);
        CheckForLineCompletion();
        CorrectPositions();
    }

    private static bool IsDestroyedPredicate(GamePiece obj)
    {
        return obj.m_SubSquares.Count == 0;
    }

    /**
     * Correct the positions of all pieces which aren't moving
     */
    private void CorrectPositions()
    {
        foreach (var gamePiece in m_GamePieces)
        {
            foreach (var gamePieceSquare in gamePiece.m_SubSquares)
            {
                Debug.LogError("Velocity = " + gamePieceSquare.m_Rigidbody2D.velocity.SqrMagnitude().ToString());
                if (gamePieceSquare.m_Rigidbody2D.velocity.SqrMagnitude() < 1f)
                {
                    
                }
            }
        }
    }

    private void CheckForLineCompletion()
    {
        Collider2D[] overlappingColliders = new Collider2D[kNumColumns];
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.SetLayerMask(m_BlocksLayerMask);
        filter2D.useLayerMask = true;
        foreach (BoxCollider2D rowChecker in m_RowCheckers)
        {
            int numCollisions = rowChecker.OverlapCollider(filter2D, overlappingColliders);
            if (numCollisions == kNumColumns)
            {
                // Check that all the z-rotations are less than 10-degress
                bool allBlocksAligned = true;
                foreach(var square in overlappingColliders)
                {
                    float kCutoff = 10f; // Allow 10 degrees either side of upright;
                    if ((square.gameObject.transform.eulerAngles.z + kCutoff) % 90f > kCutoff * 2)
                    {
                        allBlocksAligned = false;
                        break;
                    }
                }
                if (allBlocksAligned == false)
                {
                    continue;
                }

                foreach(var square in overlappingColliders)
                {
                    GamePieceSquare gamePieceSquare = square.gameObject.GetComponent(typeof(GamePieceSquare)) as GamePieceSquare; 
                    gamePieceSquare.SetFullRowDetected();
                }
            }
        }
	}

	private IEnumerator SpawnerCoroutine()
    {
        for (;;)
        {
            yield return new WaitForSeconds(3.0f);
            SpawnPiece();
        }
    }

    private void SpawnPiece()
    {
        int index = Random.Range(0, m_PiecePrefabs.Length);
        GameObject prefab = m_PiecePrefabs[index];
        GameObject newGameObject = Instantiate(prefab, m_BoardRoot.transform);
        GamePiece gamePiece = newGameObject.GetComponent(typeof(GamePiece)) as GamePiece;

        // Pick a random rotation and position
        int rotationIndex = Random.Range(0, 4);

        // Rotate around center square
        gamePiece.transform.RotateAround(gamePiece.m_CenterSquare.transform.position, 
                                         new Vector3(0, 0, 1)/*Rotate around the z axis*/, 
                                         rotationIndex * 90f);

        int positionIndex = Random.Range(0, kNumColumns-2);
        RectTransform rectTransform = m_BoardRoot.transform as RectTransform;
        float step = rectTransform.sizeDelta.x / kNumColumns;
        newGameObject.transform.position = new Vector3(
            newGameObject.transform.position.x + (step * positionIndex),
            newGameObject.transform.position.y,
            newGameObject.transform.position.z
        );

        m_GamePieces.Add(gamePiece);
    }
}
