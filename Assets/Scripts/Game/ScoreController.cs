using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : Singleton<ScoreController> 
{
    public TextMesh m_ScoreText;
    public TextMesh m_ScoreShadow;
    public TextMesh m_LevelText;
    public TextMesh m_LevelShadow;
    public TextMesh m_LinesText;
    public TextMesh m_LinesShadow;

    public GameObject m_GameOverFrame;
    public TextMesh m_GameOverScoreText;
    public TextMesh m_GameOverScoreShadow;
    public TextMesh m_GameOverLevelText;
    public TextMesh m_GameOverLevelShadow;
    public TextMesh m_GameOverLinesText;
    public TextMesh m_GameOverLinesShadow;


    public bool GameOver
    {
        get
        {
            return m_GameOver;
        }
        set
        {
            m_GameOver = value;
            UpdateText();
            m_Player.SetActive(!m_GameOver);
        }
    }

    public int Score
    {
        get
        {
            return m_Score;
        }
        set
        {
            m_Score = value;
            UpdateText();

            Level = (m_Score / 250) + 1;
        }
    }

    public int Level
    {
        get
        {
            return m_Level;
        }
        set
        {
            m_Level = value;
            UpdateText();
        }
    }

    public int Lines
    {
        get
        {
            return m_Lines;
        }
        set
        {
            m_Lines = value;
            UpdateText();
        }
    }

    private int m_Score = 0;
    private int m_Lines = 0;
    private int m_Level = 1;
    private bool m_GameOver = false;

    private GameObject m_Player;

    // Use this for initialization
    void Start()
    {
        m_Player = GameObject.Find("Player");
        UpdateText();
    }

	private void Update()
	{
        if (m_GameOver && InputManager.Instance.GetJumpKeyDown())
        {
            // Restart the game
            BoardController.Instance.ResetAll();
            Score = 0;
            Lines = 0;
            Level = 1;
            GameOver = false;
        }
	}

	void UpdateText()
    {
        m_ScoreText.text    = m_Score.ToString();
        m_ScoreShadow.text  = m_Score.ToString();
        m_LevelText.text    = m_Level.ToString();
        m_LevelShadow.text  = m_Level.ToString();
        m_LinesText.text    = m_Lines.ToString();
        m_LinesShadow.text  = m_Lines.ToString();

        m_GameOverFrame.SetActive(m_GameOver);
        if (m_GameOver)
        {
            m_GameOverScoreText.text = m_Score.ToString();
            m_GameOverScoreShadow.text = m_Score.ToString();
            m_GameOverLevelText.text = m_Level.ToString();
            m_GameOverLevelShadow.text = m_Level.ToString();
            m_GameOverLinesText.text = m_Lines.ToString();
            m_GameOverLinesShadow.text = m_Lines.ToString();
        }
	}
}
