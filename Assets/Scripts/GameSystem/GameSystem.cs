using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
	[SerializeField] private List<IGameSubsystem> m_GameSubSystems = new List<IGameSubsystem>();

	private void Awake()
	{
		foreach (IGameSubsystem gameSubsystem in m_GameSubSystems)
		{
			gameSubsystem.GameStart();
		}
	}

	private void Update()
	{
		foreach (IGameSubsystem gameSubsystem in m_GameSubSystems)
		{
			gameSubsystem.GameTick();
		}
	}

	private void OnDestroy()
	{
		foreach (IGameSubsystem gameSubsystem in m_GameSubSystems)
		{
			gameSubsystem.GameFinish();
		}
	}
}

