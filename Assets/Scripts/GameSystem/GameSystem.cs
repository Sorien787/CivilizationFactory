using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
	[SerializeField] private List<IGameSubsystem<IListenerInterface>> m_GameSubSystems;

	private void Awake()
	{
		foreach(IGameSubsystem<IListenerInterface> gameSubsystem in m_GameSubSystems) 
		{
			gameSubsystem.GameStart();
		}
	}

	private void OnDestroy()
	{
		foreach (IGameSubsystem<IListenerInterface> gameSubsystem in m_GameSubSystems)
		{
			gameSubsystem.GameFinish();
		}
	}
}

