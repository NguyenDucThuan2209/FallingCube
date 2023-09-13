using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : UIScreen
{
    [SerializeField] Text m_scoreText;

    public void SetScoreText(int score)
    {
        m_scoreText.text = score.ToString();
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
