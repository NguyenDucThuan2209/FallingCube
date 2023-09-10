using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Initializing,
    Playing,
    Pausing,
    End
}

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager Instance => m_instance;

    [Header("Game Properties")]
    [SerializeField] GameState m_state;
    [SerializeField] Vector2 m_vertical;
    [SerializeField] Vector2 m_horizontal;

    private int m_score;
    private int m_bestScore;

    public GameState State => m_state;
    public Vector2 Vertical => m_vertical;
    public Vector2 Horizontal => m_horizontal;

    private void Awake()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;

        CalculateScreenSize();
    }
    private void Update()
    {
        if (m_state != GameState.Playing) return;

    }

    private void ResetGameData()
    {
        
    }
    private void CalculateScreenSize()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
        var upperRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        m_vertical = new Vector2(bottomLeft.y, upperRight.y);
        m_horizontal = new Vector2(bottomLeft.x, upperRight.x);
    }
    public void ScorePoint()
    {
        m_score++;
        m_bestScore = (m_bestScore < m_score) ? m_score : m_bestScore;

        SoundManager.Instance.PlaySound("Score");
        MenuManager.Instance.SetScore(m_score, m_bestScore);
    }

    public void StartGame()
    {
        Debug.LogWarning("Start Game");

        ResetGameData();
        m_state = GameState.Playing;
    }
    public void PauseGame()
    {
        Debug.LogWarning("Pause Game");

        m_state = GameState.Pausing;
    }
    public void ResumeGame()
    {
        Debug.LogWarning("Resume Game");

        m_state = GameState.Playing;
    }
    public void EndGame()
    {
        Debug.LogWarning("End Game");

        m_state = GameState.End;

        ResetGameData();
        MenuManager.Instance.EndGame();
    }
}
