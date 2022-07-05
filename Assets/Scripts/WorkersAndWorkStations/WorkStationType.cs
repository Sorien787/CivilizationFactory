using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationType : ScriptableObject
{
	[SerializeField] private string m_MachineName;
	[SerializeField] private List<WorkerType> m_AllowedWorkerTypes = new List<WorkerType>();
	[SerializeField] private int m_MaxWorkerSlots = 0;
	[SerializeField] private string m_MachineDescriptor;

	public int GetMaxSlots => m_MaxWorkerSlots;
	public List<WorkerType> GetAllowedWorkerTypes => m_AllowedWorkerTypes;
	public string GetName => m_MachineName;
	public string GetDescriptor => m_MachineDescriptor;
}
