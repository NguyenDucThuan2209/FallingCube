using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] int m_id;
    [SerializeField] SpriteRenderer m_sprite;
    [SerializeField] SpriteRenderer m_highlight;

    public void HighlightBox()
    {
        m_highlight.gameObject.SetActive(true);
    }
    public void UnhighlightBox()
    {
        m_highlight.gameObject.SetActive(false);
    }
    public void SetOrder(int order)
    {
        m_highlight.sortingOrder = order;
        m_sprite.sortingOrder = order + 1;
    }
}
