using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Properties")]
    [SerializeField] float m_jumpForce;
    [SerializeField] float m_cameraOffset;

    [Header("Character Preferences")]
    [SerializeField] Collider2D m_collider;
    [SerializeField] Rigidbody2D m_rigidbody;
    [SerializeField] SpriteRenderer m_visual;
    [SerializeField] Sprite[] m_visualOptions;

    private bool m_isPlayerDeath = false;
    private float m_timeCount = 0f;

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing) return;

        if (m_isPlayerDeath) return;

        CheckPlayerOutOfScreen();

        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Instance.PlaySound("Fly");
            m_rigidbody.velocity = transform.up * m_jumpForce;
        }

        if (transform.position.y > Camera.main.transform.position.y + m_cameraOffset)
        {
            Camera.main.transform.position = new Vector3(0f, transform.position.y - m_cameraOffset, -10f);
        }
    }
    private void LateUpdate()
    {
        if (GameManager.Instance.State != GameState.Playing) return;

        if (m_isPlayerDeath)
        {
            if (m_timeCount < 2f)
            {
                m_timeCount += Time.deltaTime;
                Camera.main.transform.position = new Vector3(0f, transform.position.y - m_cameraOffset, -10f);
            }
            else
            {
                gameObject.SetActive(false);
                GameManager.Instance.EndGame();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_isPlayerDeath) return;

        if (collision.CompareTag("Obstacle"))
        {
            OnPlayerDeath();
        }
        if (collision.CompareTag("Package"))
        {
            GameManager.Instance.ScorePoint();
            Destroy(collision.gameObject);
        }
    }

    private void OnPlayerDeath()
    {
        m_isPlayerDeath = true;
        m_collider.enabled = false;

        m_rigidbody.constraints = RigidbodyConstraints2D.None;
        m_rigidbody.AddTorque(5, ForceMode2D.Impulse);

        SoundManager.Instance.PlaySound("Lost");
    }
    private void CheckPlayerOutOfScreen()
    {
        if (transform.position.y - Camera.main.transform.position.y < GameManager.Instance.Vertical.x && !m_isPlayerDeath)
        {
            Debug.LogWarning("Player Death");
            OnPlayerDeath();
        }
    }

    public void InitializeCharacter(int visualID)
    {
        m_visual.sprite = m_visualOptions[visualID];
    }
}
