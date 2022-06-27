using System.Collections.Generic;

using UnityEngine;

public class IWorkStation : IListenerMonoBehaviour<IWorkStationListener>, ISelectableListener, IWorkerListener, IWorkerPoolListener
{
	[SerializeField] private List<WorkerType> m_AllowedWorkerTypes = new List<WorkerType>();

	[SerializeField] private WorkerPool m_WorkerPool = null;

	[SerializeField] private int m_MaxWorkerSlots = 0;

	[SerializeField] private ISelectable m_SelectableComponent = null;

	[SerializeField] private UIPanelType m_WorkstationUIPanel;

	[SerializeField] private UIPanelSystem m_UIPanelSystem;

	private float m_CurrentMachineEfficiency = 0.0f;

	private int m_CurrentWorkerSlots = 0;

	private Dictionary<WorkerType, List<IWorker>> m_CurrentWorkers = new Dictionary<WorkerType, List<IWorker>>();

	private void Awake()
	{
		m_SelectableComponent.AddListener(this);	
	}

	/// <summary>
	/// ISelectableListener function
	/// </summary>
	public void OnSelect()
	{
		m_WorkerPool.AddListener(this);
		m_UIPanelSystem.DisplayUIPanelByIdentifier(m_WorkstationUIPanel);
	}

	public void OnDeselect()
	{
		m_WorkerPool.RemoveListener(this);
		m_UIPanelSystem.HideUIPanelByIdentifier(m_WorkstationUIPanel);
	}

	/// <summary>
	/// IWorkerPoolListener function
	/// </summary>
	public void OnWorkerAdded(WorkerType workerType)
	{
		OnAvailableWorkersChanged(workerType);
	}

	public void OnWorkerRemoved(WorkerType workerType)
	{
		OnAvailableWorkersChanged(workerType);
	}

	/// <summary>
	/// IWorkerListener functions
	/// </summary>
	public void OnWorkerChangedStatus()
	{
		OnMachineEfficiencyChanged();
	}

	// called by UI to determine what we show

	public List<WorkerType> GetWorkerTypesToDisplay() 
	{
		return m_AllowedWorkerTypes;
	}

	public float GetCurrentEfficiency() 
	{
		return m_CurrentMachineEfficiency;
	}

	public int GetNumWorkersOfType(WorkerType workerType) 
	{
		return m_CurrentWorkers[workerType].Count;
	}

	public int GetMaxNumWorkersOfType(WorkerType workerType) 
	{
		return m_MaxWorkerSlots / workerType.GetWorkerSize();
	}

	public int GetTotalNumSlots()
	{
		return m_MaxWorkerSlots;
	}

	public int GetNumFilledSlots() 
	{
		return m_CurrentWorkerSlots;
	}

	public bool CanAddMoreWorkersOfType(WorkerType workerType) 
	{
		int numWorkersCanAdd = GetNumPossibleWorkersToAdd(workerType);
		return numWorkersCanAdd != 0;
	}

	public bool CanRemoveMoreWorkersOfType(WorkerType workerType) 
	{
		return m_CurrentWorkers.ContainsKey(workerType);
	}

	public float GetCurrentMachineEfficiency() 
	{
		return m_CurrentMachineEfficiency;
	}

	// called by UI to change workers
	private int GetNumPossibleWorkersToAdd(in WorkerType workerType) 
	{
		int numSlotsAvailable = m_MaxWorkerSlots - m_CurrentWorkerSlots;
		int numWorkersCanAdd = numSlotsAvailable / workerType.GetWorkerSize();
		int numWorkersAvailable = m_WorkerPool.GetNumWorkersAvailable(workerType);
		return Mathf.Min(numWorkersAvailable, numWorkersCanAdd);
	}


	public void OnWorkerAdded(in WorkerType workerType) 
	{
		// this will decrease the number of available workers
		// so will refresh UI by listener
		List<IWorker> workers = m_WorkerPool.OnRequestWorkers(1, workerType);
		AddWorkers(workerType, workers);
	}

	public void OnWorkersMaximised(in WorkerType workerType) 
	{
		int numWorkersToAdd = GetNumPossibleWorkersToAdd(workerType);
		
		// this will decrease the number of available workers
		// so will refresh UI by listener
		List<IWorker> workers = m_WorkerPool.OnRequestWorkers(numWorkersToAdd, workerType);

		AddWorkers(workerType, workers);
	}

	public void AddWorkers(in WorkerType workerType, in List<IWorker> workersToAdd) 
	{
		m_CurrentWorkers[workerType].AddRange(workersToAdd);
		m_CurrentWorkerSlots += workersToAdd.Count * workerType.GetWorkerSize();
		foreach(IWorker worker in workersToAdd) 
		{
			worker.SetWorkStation(this);
		}
		OnMachineEfficiencyChanged();
	}

	public void OnWorkerRemoved(in WorkerType workerType) 
	{
		RemoveWorkers(workerType, 1);
	}

	public void OnWorkersMinimized(in WorkerType workerType) 
	{
		m_CurrentWorkerSlots -= m_CurrentWorkers[workerType].Count * workerType.GetWorkerSize();
		RemoveWorkers(workerType, m_CurrentWorkers[workerType].Count);
	}

	public void RemoveWorkers(in WorkerType workerType, in int numWorkersToRemove) 
	{
		List<IWorker> currentWorkersOfType = m_CurrentWorkers[workerType];
		currentWorkersOfType.RemoveRange(currentWorkersOfType.Count - numWorkersToRemove, currentWorkersOfType.Count - 1);
		List<IWorker> workersReleased = new List<IWorker>();

		int startWorkerIndex = currentWorkersOfType.Count - numWorkersToRemove;
		for (int workerIndex = 0; workerIndex < numWorkersToRemove; workerIndex++) 
		{
			workersReleased.Add(currentWorkersOfType[workerIndex + startWorkerIndex]);
		}
		currentWorkersOfType.RemoveRange(startWorkerIndex, currentWorkersOfType.Count - 1);
		// this will increase the number of available workers
		// so will refresh UI by listener
		m_WorkerPool.OnReleaseWorkers(workerType, workersReleased);

		foreach(IWorker worker in workersReleased) 
		{
			worker.ResetWorkStation();
		}
		
		OnMachineEfficiencyChanged();
	}

	private void OnAvailableWorkersChanged(WorkerType workerType) 
	{
		ForEachListener((IWorkStationListener listener) => listener.OnWorkersChanged(workerType));
	}

	private void OnMachineEfficiencyChanged() 
	{
		ForEachListener((IWorkStationListener listener) => listener.OnMachineEfficiencyChanged());
	}
}

public interface IWorkStationListener : IListenerInterface
{
	void OnMachineEfficiencyChanged();

	void OnWorkersChanged(WorkerType workerType);
}
