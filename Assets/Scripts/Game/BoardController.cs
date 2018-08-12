using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : Singleton<BoardController> 
{
    public GameObject [] m_PiecePrefabs;
    public GameObject m_BoardRoot;
    public GameObject m_RowCheckerPrefab;
    public LayerMask m_BlocksLayerMask;

    public BoxCollider2D m_GameOverChecker;

    public float m_TimeBetweenSpawns = 5.0f;
    public float m_IntroWaitTime = 1.0f;

    [Header("For Debugging")]
    public bool m_DropPieces = true;  // Kill switch
    public bool m_SnapPositions = true;

    [Tooltip("-1 for infinite pieces")]
    public int numPiecesToSpawn = -1;

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

    public void ResetAll()
    {
        // Reset all the pieces
        foreach (var piece in m_GamePieces)
        {
            foreach (GamePieceSquare gpc in piece.m_SubSquares)
            {
                Destroy(gpc.gameObject);
            }
        }

        m_GamePieces.Clear();
    }

    private void Update()
    {
        m_GamePieces.RemoveAll(IsDestroyedPredicate);
        CheckForLineCompletion();

        if (m_SnapPositions)
        {
            SnapPositions();
        }

        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        Collider2D[] overlappingColliders = new Collider2D[kNumColumns];
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.SetLayerMask(m_BlocksLayerMask);
        filter2D.useLayerMask = true;
        int numCollisions = m_GameOverChecker.OverlapCollider(filter2D, overlappingColliders);
        if (numCollisions > 4)
        {
            ScoreController.Instance.GameOver = true;
        }
    }

    private static bool IsDestroyedPredicate(GamePiece obj)
    {
        return obj.m_SubSquares.Count == 0;
    }

    private void SnapPositions()
    {
        int positionIndex = Random.Range(0, kNumColumns - 2);
        RectTransform rectTransform = m_BoardRoot.transform as RectTransform;
        float stepX = rectTransform.sizeDelta.x / kNumColumns;
        float stepY = rectTransform.sizeDelta.y / kNumRows;

        foreach (var gamePiece in m_GamePieces)
        {
            foreach (var gamePieceSquare in gamePiece.m_SubSquares)
            {
                gamePieceSquare.SetMovedThisFrame(gamePieceSquare.Rigidbody2D.velocity.SqrMagnitude() > 0.05f);

                if (gamePieceSquare.IsStationary())
                {
                    // Get the position of the square in board space
                    Vector3 positionInBoardSpace = m_BoardRoot.transform.InverseTransformPoint(gamePieceSquare.transform.position);
                    int column = (int)(positionInBoardSpace.x / stepX);
                    int row = (int)(positionInBoardSpace.y / stepY);
                    Vector3 newPositionInBoardSpace = new Vector3(stepX * (column + 0.5f), stepY * (row + 0.5f), positionInBoardSpace.z);

                    float sqrMagnitude = (newPositionInBoardSpace - positionInBoardSpace).sqrMagnitude;
                    if (sqrMagnitude > 0.001f) // Figured out the constant empirically
                    {
                        gamePieceSquare.SnapPosition(m_BoardRoot.transform.TransformPoint(newPositionInBoardSpace));
                    }
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
            // Modify timeBetweenSpawns using the player level
            float modifier = Mathf.Clamp(ScoreController.Instance.Level - 1f, 0f, 3.5f);
            float timeBetweenSpawns = m_TimeBetweenSpawns - modifier;
            timeBetweenSpawns = Mathf.Clamp(timeBetweenSpawns, 0.1f, m_TimeBetweenSpawns);

            yield return new WaitForSeconds(timeBetweenSpawns);
            if (m_DropPieces)
            {
                if (numPiecesToSpawn != -1)
                {
                    numPiecesToSpawn--;
                    if (numPiecesToSpawn == -1)
                    {
                        break;
                    }
                }

                if (!ScoreController.Instance.GameOver)
                {
                    SpawnPiece();
                }
            }
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

        ScoreController.Instance.Score += 10;

        m_GamePieces.Add(gamePiece);
    }
}
