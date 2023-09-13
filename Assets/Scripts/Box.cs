using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] int m_id;
    [SerializeField] int m_order;
    [SerializeField] SpriteRenderer m_sprite;
    [SerializeField] SpriteRenderer m_highlight;

    public int ID => m_id;
    public int Order => m_order;

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
        m_sprite.sortingOrder = order + 2;
        m_highlight.sortingOrder = order + 1;
    }
}
