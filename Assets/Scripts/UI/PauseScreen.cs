using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : UIScreen
{
    [SerializeField] Text m_score;

    public void OnResumeButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");
        MenuManager.Instance.ResumeGame();
    }
    public void OnReplayButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");
        MenuManager.Instance.StartGame();
    }
    public void OnBackToMenuButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");
        MenuManager.Instance.BackToHome();
    }

    public void SetScore(int score)
    {
        m_score.text = score.ToString();
    }
}
