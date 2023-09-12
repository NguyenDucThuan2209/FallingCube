using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] int m_xLength;
    [SerializeField] int m_yLength;
    [SerializeField] float m_heighStart = -1f;
    [SerializeField] float m_heightDiff = 1.275f;

    [SerializeField] Box m_boxPrefab;
    [SerializeField] Box[] m_bottomBoxs;

    private Box m_current;
    private Box[,] m_matrix;
    private int[] m_columnHeight;

    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing) return;

        if (Input.GetMouseButtonDown(0))
        {
            SpawnBox();
        }
        if (Input.GetMouseButton(0) && m_current != null)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x < (-GameManager.Instance.Horizontal.y * 2 / 5f))
            {
                HighlightBox(0);
                m_current.transform.position = new Vector3(-2.65f, 3f, 0f);
            }
            else if (mousePos.x > (GameManager.Instance.Horizontal.y * 2 / 5f))
            {
                HighlightBox(2);
                m_current.transform.position = new Vector3(2.65f, 3f, 0f);
            }
            else
            {
                HighlightBox(1);
                m_current.transform.position = new Vector3(0f, 3f, 0f);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x < (-GameManager.Instance.Horizontal.y * 2 / 5f))
            {
                DropBox(0);
            }
            else if (mousePos.x > (GameManager.Instance.Horizontal.y * 2 / 5f))
            {
                DropBox(2);
            }
            else
            {
                DropBox(1);
            }
        }
    }

    private void SpawnBox()
    {
        m_current = Instantiate(m_boxPrefab, transform);
    }
    private void UnhighlightAllBox()
    {
        for (int i = 0; i < m_bottomBoxs.Length; i++)
        {
            m_bottomBoxs[i].UnhighlightBox();
        }
        for (int i = 0; i < m_xLength; i++)
        {
            for (int j = 0; j < m_columnHeight[i]; j++)
            {
                if (m_matrix[i, j] != null)
                {
                    m_matrix[i, j].UnhighlightBox();
                }
            }
        }
    }
    private void HighlightBox(int column)
    {
        UnhighlightAllBox();

        if (m_columnHeight[column] > 0)
        {
            m_matrix[column, m_columnHeight[column] - 1].HighlightBox();
        }
        else
        {
            m_bottomBoxs[column].HighlightBox();
        }
    }
    private void DropBox(int column)
    {
        Destroy(m_current.gameObject);
    }
    public void Initialize()
    {
        m_matrix = new Box[m_xLength, m_yLength];
        m_columnHeight = new int[m_xLength];
        for (int i = 0; i < m_xLength; i++)
        {
            m_columnHeight[i] = 0;
        }
    }
}
