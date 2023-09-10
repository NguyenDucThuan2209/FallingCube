using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : UIScreen
{
    [SerializeField] Text m_bestScore;

    public void OnStartGameButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");

        MenuManager.Instance.StartGame();
    }
    public void OnPrivacyAndPolicyButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");

        MenuManager.Instance.OpenPrivacyAndPolicy();
    }

    public void SetHighScore(int highScore)
    {
        m_bestScore.text = "BEST " + highScore;
    }
}
