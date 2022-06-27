using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    [SerializeField] private UIPanelSystem m_UIPanelSystem = null;
    [SerializeField] private UIPanelType m_PanelType = null;
    // Start is called before the first frame update
    void Awake()
    {
        m_UIPanelSystem.AddPanel(m_PanelType, this);
    }

    public abstract void ShowUIPanel();

    public abstract void RefreshUIPanel();

    public abstract void HideUIPanel();
}
