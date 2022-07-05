using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUIPanelElement : MonoBehaviour, IToolListener
{
    [SerializeField] private IToolSystem m_ToolSystem = null;
    [SerializeField] private ITool m_LinkedTool = null;
    
    public void OnToolButtonPressed() 
    {
        if (m_ToolSystem.GetCurrentTool() == m_LinkedTool)
            return;
        m_ToolSystem.m_Listeners.AddListener(this);
    }

    public void OnToolDeselected(ITool tool) 
    {
        if (tool != m_LinkedTool)
            return;

        m_ToolSystem.m_Listeners.RemoveListener(this);
    }
}
