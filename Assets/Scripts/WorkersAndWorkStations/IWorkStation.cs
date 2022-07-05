using System.Collections.Generic;

using UnityEngine;

public class IWorkStation : IListenerMonoBehaviour<IWorkStationListener>, ISelectableListener, IWorkerListener, IWorkerPoolListener
{

	[SerializeField] private WorkerPool m_WorkerPool = null;

	[SerializeField] private ISelectable m_SelectableComponent = null;

	[SerializeField] private UIPanelType m_WorkstationUIPanel;

	[SerializeField] private UIPanelSystem m_UIPanelSystem;

	private float m_CurrentMachineEfficiency = 0.0f;

	private int m_CurrentWorkerSlots = 0;

	private WorkStationType m_WorkStationType = null;

	private WorkerContainer m_WorkerContainer = new WorkerContainer();

	private void Awake()
	{
		m_SelectableComponent.AddListener(this);	
	}

	public void SetMachineType(WorkStationType stationType) 
	{
		m_WorkStationType = stationType;
	}

	/// <summary>
	/// ISelectableListener function
	/// </summary>
	public void OnSelect()
	{
		m_WorkerPool.m_Listeners.AddListener(this);
		m_UIPanelSystem.DisplayUIPanelByIdentifier(m_WorkstationUIPanel);
	}

	public void OnDeselect()
	{
		m_WorkerPool.m_Listeners.RemoveListener(this);
		m_UIPanelSystem.HideUIPanelByIdentifier(m_WorkstationUIPanel);
	}

	/// <summary>
	/// IWorkerPoolListener function
	/// </summary>
	/// 
	public void OnWorkersRemoved(WorkerType workerType)
	{
		OnAvailableWorkersChanged(workerType);
	}

	public void OnWorkersAdded(WorkerType workerType)
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
		return m_WorkStationType.GetAllowedWorkerTypes;
	}

	public int GetNumWorkersOfType(WorkerType workerType) 
	{
		return m_WorkerContainer.GetNumWorkersOfType(workerType);
	}

	public int GetMaxNumWorkersOfType(WorkerType workerType) 
	{
		return m_WorkStationType.GetMaxSlots / workerType.GetWorkerSize();
	}

	public int GetTotalNumSlots()
	{
		return m_WorkStationType.GetMaxSlots;
	}

	public string GetDescription() 
	{
		return m_WorkStationType.GetDescriptor;
	}

	public string GetName() 
	{
		return m_WorkStationType.GetName;
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
		return m_WorkerContainer.GetNumWorkersOfType(workerType) > 0;
	}

	public float GetCurrentMachineEfficiency() 
	{
		return m_CurrentMachineEfficiency;
	}

	// called by UI to change workers
	private int GetNumPossibleWorkersToAdd(in WorkerType workerType) 
	{
		int numSlotsAvailable = m_WorkStationType.GetMaxSlots - m_CurrentWorkerSlots;
		int numWorkersCanAdd = numSlotsAvailable / workerType.GetWorkerSize();
		int numWorkersAvailable = m_WorkerPool.GetNumWorkersOfType(workerType);
		return Mathf.Min(numWorkersAvailable, numWorkersCanAdd);
	}


	public void OnWorkerAdded(in WorkerType workerType) 
	{
		// this will decrease the number of available workers
		// so will refresh UI by listener
		List<IWorker> workers = m_WorkerPool.RemoveWorkersFromPool(workerType, 1);
		AddWorkers(workerType, workers);
	}

	public void OnWorkersMaximised(in WorkerType workerType) 
	{
		int numWorkersToAdd = GetNumPossibleWorkersToAdd(workerType);
		
		// this will decrease the number of available workers
		// so will refresh UI by listener
		List<IWorker> workers = m_WorkerPool.RemoveWorkersFromPool(workerType, numWorkersToAdd);

		AddWorkers(workerType, workers);
	}

	private void AddWorkers(in WorkerType workerType, in List<IWorker> workersToAdd) 
	{
		m_WorkerContainer.AddWorkersOfType(workerType, workersToAdd);

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
		m_CurrentWorkerSlots -= m_WorkerContainer.GetNumWorkersOfType(workerType) * workerType.GetWorkerSize();
		RemoveWorkers(workerType, m_WorkerContainer.GetNumWorkersOfType(workerType));
	}

	private void RemoveWorkers(in WorkerType workerType, in int numWorkersToRemove) 
	{
		List<IWorker> workersRemoved = m_WorkerContainer.RequestWorkersByType(workerType, numWorkersToRemove);

		// this will increase the number of available workers
		// so will refresh UI by listener
		m_WorkerPool.AddWorkersToPool(workersRemoved, workerType);

		foreach(IWorker worker in workersRemoved) 
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
