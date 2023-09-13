using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] int m_xLength;
    [SerializeField] int m_yLength;
    [SerializeField] float m_heighStart = -1f;
    [SerializeField] float m_heightDiff = 1.275f;
    [SerializeField] float m_dropHeight = 5.5f;

    [SerializeField] Box[] m_boxPrefabs;
    [SerializeField] Box[] m_bottomBoxs;
    [SerializeField] TMPro.TextMeshPro m_text;

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

        m_text.text = "";
        if (m_matrix != null)
        {
            for (int i = 0; i < m_xLength; i++)
            {
                for (int j = 0; j < m_yLength; j++)
                {
                    if (m_matrix[i, j])
                    {
                        m_text.text += m_matrix[i, j] + "\n";
                    }
                    else
                    {
                        m_text.text += "null \n";
                    }
                }
                m_text.text += "===================\n";
            }
        }

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.y > m_dropHeight) return;

        if (Input.GetMouseButtonDown(0))
        {
            SpawnBox();
        }
        if (Input.GetMouseButton(0) && m_current != null)
        {   
            if (mousePos.x < (-GameManager.Instance.Horizontal.y * 2 / 5f))
            {
                HighlightBox(0);
                m_current.transform.position = new Vector3(-2.65f, m_dropHeight, 0f);
            }
            else if (mousePos.x > (GameManager.Instance.Horizontal.y * 2 / 5f))
            {
                HighlightBox(2);
                m_current.transform.position = new Vector3(2.65f, m_dropHeight, 0f);
            }
            else
            {
                HighlightBox(1);
                m_current.transform.position = new Vector3(0f, m_dropHeight, 0f);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
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
        m_current = Instantiate(m_boxPrefabs[0], transform);
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

        if (m_columnHeight[column] == 0)
        {
            m_bottomBoxs[column].HighlightBox();
        }
        else if (m_columnHeight[column] < m_yLength)
        {
            m_matrix[column, m_columnHeight[column] - 1].HighlightBox();
        }
    }
    private void DropBox(int column)
    {
        if (m_columnHeight[column] < m_yLength)
        {
            SoundManager.Instance.PlaySound("Drop");

            var targetObj = m_current.transform;
            var start = m_current.transform.position;
            var end = new Vector3(m_current.transform.position.x, m_columnHeight[column] * m_heightDiff + m_heighStart, 0f);
            StartCoroutine(GameManager.IE_Translate(targetObj, start, end, 0.1f, () => {  }));

            m_columnHeight[column]++;
            m_current.SetOrder(m_columnHeight[column]);
            m_matrix[column, m_columnHeight[column] - 1] = m_current;

            MergeBox(column, m_columnHeight[column] - 1);
        }
        else
        {
            Destroy(m_current.gameObject);
        }

        m_current = null;
        UnhighlightAllBox();
    }
    private void MergeBox(int column, int row)
    {
        var currentBox = m_matrix[column, row];
        var upBox = (row > 0) ? m_matrix[column, row - 1] : null;
        var leftBox = (column > 0) ? m_matrix[column - 1, row] : null;
        var rightBox = (column + 1 < m_xLength) ? m_matrix[column + 1, row] : null;

        if (upBox != null)
        {
            if (upBox.ID == currentBox.ID)
            {
                m_columnHeight[column]--;

                var start = currentBox.transform.position;
                var end = upBox.transform.position;
                StartCoroutine(GameManager.IE_Translate(currentBox.transform, start, end, 0.1f));

                for (int i = 0; i < m_boxPrefabs.Length; i++)
                {
                    if (m_boxPrefabs[i].ID == currentBox.ID * 2)
                    {
                        Debug.LogWarning($"Merge:  {currentBox} ({column}, {row}) to {upBox.name} ({column}, {row - 1})");
                        GameManager.Instance.ScorePoint(currentBox.ID * 2);
                        SoundManager.Instance.PlaySound("Merge");

                        var positionMerge = new Vector3(upBox.transform.position.x, (row - 1) * m_heightDiff + m_heighStart, 0f);
                        
                        m_matrix[column, row] = null;
                        m_matrix[column, row - 1] = Instantiate(m_boxPrefabs[i], positionMerge, Quaternion.identity, transform);
                        m_matrix[column, row - 1].SetOrder(m_columnHeight[column]);

                        Destroy(upBox.gameObject);
                        Destroy(currentBox.gameObject);

                        MergeBox(column, row - 1);
                    }
                }
                return;
            }
        }
        if (leftBox != null)
        {
            if (leftBox.ID == currentBox.ID)
            {
                m_columnHeight[column - 1]--;

                var start = leftBox.transform.position;
                var end = currentBox.transform.position;
                StartCoroutine(GameManager.IE_Translate(leftBox.transform, start, end, 0.1f));

                for (int i = 0; i < m_boxPrefabs.Length; i++)
                {
                    if (m_boxPrefabs[i].ID == currentBox.ID * 2)
                    {
                        Debug.LogWarning($"Merge: {leftBox.name} ({column - 1}, {row}) to {currentBox} ({column}, {row})");
                        GameManager.Instance.ScorePoint(currentBox.ID * 2);
                        SoundManager.Instance.PlaySound("Merge");

                        var positionMerge = new Vector3(currentBox.transform.position.x, row * m_heightDiff + m_heighStart, 0f);

                        m_matrix[column - 1, row] = null;
                        m_matrix[column, row] = Instantiate(m_boxPrefabs[i], positionMerge, Quaternion.identity, transform);
                        m_matrix[column, row].SetOrder(m_columnHeight[column]);

                        Destroy(leftBox.gameObject);
                        Destroy(currentBox.gameObject);

                        MergeBox(column, row);
                    }
                }

                AlignColumn(column - 1);
                ArrangeColumn(column - 1);
                return;
            }
        }
        if (rightBox != null)
        {
            if (rightBox.ID == currentBox.ID)
            {
                m_columnHeight[column + 1]--;

                var start = rightBox.transform.position;
                var end = currentBox.transform.position;
                StartCoroutine(GameManager.IE_Translate(rightBox.transform, start, end, 0.1f));

                for (int i = 0; i < m_boxPrefabs.Length; i++)
                {
                    if (m_boxPrefabs[i].ID == currentBox.ID * 2)
                    {
                        Debug.LogWarning($"Merge: {rightBox.name} ({column + 1}, {row}) to {currentBox} ({column}, {row})");
                        GameManager.Instance.ScorePoint(currentBox.ID * 2);
                        SoundManager.Instance.PlaySound("Merge");

                        var positionMerge = new Vector3(currentBox.transform.position.x, row * m_heightDiff + m_heighStart, 0f);

                        m_matrix[column + 1, row] = null;
                        m_matrix[column, row] = Instantiate(m_boxPrefabs[i], positionMerge, Quaternion.identity, transform);
                        m_matrix[column, row].SetOrder(m_columnHeight[column]);

                        Destroy(rightBox.gameObject);
                        Destroy(currentBox.gameObject);

                        MergeBox(column, row);
                    }
                }

                AlignColumn(column + 1);
                ArrangeColumn(column + 1);
                return;
            }
        }

        CheckGameState();
    }
    private void AlignColumn(int column)
    {
        //Debug.Log("Align Column: " + column);
        int isNeedToDrop = 0;
        for (int i = 0; i < m_yLength; i++)
        {
            if (m_matrix[column, i] != null)
            {
                if (isNeedToDrop > 0)
                {
                    var targetObj = m_matrix[column, i].transform;
                    var start = m_matrix[column, i].transform.position;
                    var end = new Vector3(m_matrix[column, i].transform.position.x, (i - isNeedToDrop) * m_heightDiff + m_heighStart, 0f);

                    StartCoroutine(GameManager.IE_Translate(targetObj, start, end, 0.1f));
                    m_matrix[column, i - isNeedToDrop] = m_matrix[column, i];
                    m_matrix[column, i] = null;
                    AlignColumn(column);

                    for (int j = 0; j < m_yLength; j++)
                    {
                        if (m_matrix[column, j] == null)
                        {
                            int count = j;
                            do
                            {
                                count++;
                                if (count >= m_yLength)
                                {
                                    return;
                                }
                            } while (m_matrix[column, count] == null);
                            Debug.LogError("Not Clearly Align Column!");
                        }
                    }
                    return;
                }
                continue;
            }
            else
            {
                isNeedToDrop++;
            }
        }
    }
    private void ArrangeColumn(int column)
    {
        for (int i = 0; i < m_yLength; i++)
        {
            if (m_matrix[column, i] != null)
            {
                m_matrix[column, i].SetOrder(i);
            }
            else
            {
                return;
            }
        }
    }
    private void CheckGameState()
    {
        var countFullStack = 0;
        for (int i = 0; i < m_xLength; i++)
        {
            if (m_columnHeight[i] == m_yLength)
            {
                countFullStack++;
            }
        }
        if (countFullStack == m_xLength)
        {
            GameManager.Instance.EndGame();
        }
    }

    public void Initialize()
    {
        m_current = null;
        m_matrix = new Box[m_xLength, m_yLength];
        
        m_columnHeight = new int[m_xLength];
        for (int i = 0; i < m_xLength; i++)
        {
            m_columnHeight[i] = 0;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Box box))
            {
                if (box.ID > 0)
                {
                    Destroy(box.gameObject);
                }
            }
        }
    }
}
