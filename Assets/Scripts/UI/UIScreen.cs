using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    [SerializeField] GameObject m_panel;
    
    public virtual void ShowScreen()
    {
        m_panel.SetActive(true);
    }
    public virtual void HideScreen()
    {
        m_panel.SetActive(false);
    }
}
