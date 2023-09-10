using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameScreen : UIScreen
{
    [SerializeField] Text m_scoreText;

    public void SetScoreText(int score)
    {
        m_scoreText.text = "BOX " + score;
    }
    public void OnPauseButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");
        MenuManager.Instance.PauseGame();
    }
}
