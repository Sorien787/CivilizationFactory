using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tool System")]
public class IToolSystem : IGameSubsystem
{
	private ITool m_CurrentActiveTool = null;

	[SerializeField] private ITool m_NullTool;

	public readonly CListener<IToolListener> m_Listeners = new();
	public void OnNewToolActivated(ITool newTool) 
	{
		m_CurrentActiveTool.OnToolEnd();
		m_CurrentActiveTool = newTool;
		m_CurrentActiveTool.OnToolBegin();
	}

	public override void GameStart()
	{
		m_CurrentActiveTool = m_NullTool;
	}

	public void OnToolDeactivated() 
	{
		m_CurrentActiveTool.OnToolEnd();
		//ForEachListener((IToolListener toolListener) => toolListener.OnToolDeselected(m_CurrentActiveTool));
		m_CurrentActiveTool = m_NullTool;
	}

	public ITool GetCurrentTool() 
	{
		return m_CurrentActiveTool;
	}

	public override void GameTick()
	{
		m_CurrentActiveTool.OnToolTick();
	}
}

public interface IToolListener : IListenerInterface
{
	void OnToolDeselected(ITool tool);
}
