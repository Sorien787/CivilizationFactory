using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UI Panel System")]
public class UIPanelSystem : IGameSubsystem
{
	Dictionary<UIPanelType, UIPanel> m_UIPanels = new Dictionary<UIPanelType, UIPanel>();

	List<UIPanelType> m_CurrentlyOpenPanels = new List<UIPanelType>();

	public void AddPanel(UIPanelType panelType, UIPanel panel) 
	{
		m_UIPanels.Add(panelType, panel);
	}

	private void Clear() 
	{
		m_UIPanels.Clear();
		m_CurrentlyOpenPanels.Clear();
	}

	public override void GameFinish()
	{
		Clear();
	}

	public override void GameStart()
	{
		Clear();
	}

	public bool DisplayUIPanelByIdentifier(in UIPanelType UIPanelType) 
	{
		if (!m_UIPanels.ContainsKey(UIPanelType))
			return false;
		m_UIPanels[UIPanelType].ShowUIPanel();
		return true;
	}


	public bool HideUIPanelByIdentifier(in UIPanelType UIPanelType) 
	{
		if (!m_UIPanels.ContainsKey(UIPanelType))
			return false;
		m_UIPanels[UIPanelType].RefreshUIPanel();
		return true;
	}

	public bool RefreshUIPanelByIdentifier(in UIPanelType UIPanelType) 
	{
		if (!m_UIPanels.ContainsKey(UIPanelType))
			return false;
		m_UIPanels[UIPanelType].HideUIPanel();
		return true;
	}

	public override void GameTick()
	{
	}
}



