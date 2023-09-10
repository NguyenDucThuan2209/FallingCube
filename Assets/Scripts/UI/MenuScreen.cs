using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : UIScreen
{
    [SerializeField] Text m_bestScore;
    [SerializeField] Button m_soundButton;
    [SerializeField] Button m_musicButton;
    private bool m_soundState = false;
    private bool m_musicState = false;

    public override void ShowScreen()
    {
        base.ShowScreen();

        m_soundState = SoundManager.Instance.SoundState;
        m_musicState = SoundManager.Instance.MusicState;

        m_soundButton.image.color = (m_soundState) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 1f, 1f, 1f);
        m_musicButton.image.color = (m_musicState) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 1f, 1f, 1f);
    }
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
    public void OnSoundButtonPressed()
    {
        m_soundState = !m_soundState;
        m_soundButton.image.color = (m_soundState) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 1f, 1f, 1f);

        SoundManager.Instance.PlaySound("Click");
        SoundManager.Instance.SetSoundState(m_soundState);
    }
    public void OnMusicButtonPressed()
    {
        m_musicState = !m_musicState;
        m_musicButton.image.color = (m_musicState) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 1f, 1f, 1f);

        SoundManager.Instance.PlaySound("Click");
        SoundManager.Instance.SetMusicState(m_musicState);
    }

    public void SetHighScore(int highScore)
    {
        m_bestScore.text = highScore.ToString();
    }
}
