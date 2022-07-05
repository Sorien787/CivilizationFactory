using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    [SerializeField] private UIPanelSystem m_UIPanelSystem = null;
    [SerializeField] private UIPanelType m_PanelType = null;
    public readonly CListener<IUIPanelListener> m_Listeners = new();
    // Start is called before the first frame update
    void Awake()
    {
        m_UIPanelSystem.AddPanel(m_PanelType, this);
    }

    public virtual void ShowUIPanel() { }

    public virtual void RefreshUIPanel() { }

    public virtual void HideUIPanel() { }
}

public interface IUIPanelListener : IListenerInterface 
{
    void OnShowUIPanel();
    void OnHideUIPanel();
    void OnRefreshUIPanel();
}
