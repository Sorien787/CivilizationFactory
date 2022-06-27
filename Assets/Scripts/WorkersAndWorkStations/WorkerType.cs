using UnityEngine;

public class WorkerType : ScriptableObject
{
	[SerializeField] private int m_WorkerSize = 0;

	public int GetWorkerSize() 
	{
		return m_WorkerSize;
	}
}
