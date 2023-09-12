using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] int m_id;
    [SerializeField] GameObject m_highlight;

    public void HighlightBox()
    {
        m_highlight.SetActive(true);
    }
    public void UnhighlightBox()
    {
        m_highlight.SetActive(false);
    }
}
