using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private static MenuManager m_instance;
    public static MenuManager Instance => m_instance;

    [SerializeField] MenuScreen m_menuScreen;
    [SerializeField] PauseScreen m_pauseScreen;
    [SerializeField] IngameScreen m_ingameScreen;
    [SerializeField] EndGameScreen m_endGameScreen;
    [Space]
    [SerializeField] Image m_menuBackground;
    [SerializeField] Image m_ingameBackground;

    private const string POLICY_LINK = "https://doc-hosting.flycricket.io/falling-cube-privacy-policy/fdc42be0-94b5-49bb-a547-25af555fd5f3/privacy";
    private void Awake()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        m_instance = this;
    }
    
    private void HideAllScreen()
    {
        m_menuScreen.HideScreen();
        m_pauseScreen.HideScreen();
        m_ingameScreen.HideScreen();
        m_endGameScreen.HideScreen();
    }

    public void StartGame()
    {
        HideAllScreen();
        m_pauseScreen.SetScore(0);
        m_ingameScreen.ShowScreen();
        m_ingameScreen.SetScoreText(0);
        GameManager.Instance.StartGame();

        m_menuBackground.enabled = false;
        m_ingameBackground.enabled = true;
    }
    public void OpenPrivacyAndPolicy()
    {
        Application.OpenURL(POLICY_LINK);
    }
    public void PauseGame()
    {
        HideAllScreen();
        m_pauseScreen.ShowScreen();
        GameManager.Instance.PauseGame();

        m_menuBackground.enabled = true;
        m_ingameBackground.enabled = false;
    }
    public void ResumeGame()
    {
        HideAllScreen();
        m_ingameScreen.ShowScreen();
        GameManager.Instance.ResumeGame();

        m_menuBackground.enabled = false;
        m_ingameBackground.enabled = true;
    }
    public void EndGame()
    {
        HideAllScreen();
        m_endGameScreen.ShowScreen();

        m_menuBackground.enabled = true;
        m_ingameBackground.enabled = false;
    }
    public void BackToHome()
    {
        HideAllScreen();
        m_menuScreen.ShowScreen();
    }

    public void SetScore(int score, int highScore)
    {
        m_pauseScreen.SetScore(score);
        m_ingameScreen.SetScoreText(score);
        m_endGameScreen.SetScoreText(score);
        m_menuScreen.SetHighScore(highScore);
    }
}
