using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : UIScreen
{
    [SerializeField] Text m_scoreText;
    [SerializeField] Text m_highScoreText;

    public void SetScoreText(int score, int highScore)
    {
        m_scoreText.text = "SCORE " + score;
        m_highScoreText.text = "BEST " + highScore;
    }

    public void OnHomeButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");
        MenuManager.Instance.BackToHome();
    }
    public void OnReplayButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");
        MenuManager.Instance.StartGame();
    }
}
