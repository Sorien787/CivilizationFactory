
using UnityEngine;

public class IWorker : MonoBehaviour, ISelectableListener
{
	[SerializeField] private WorkerType m_WorkerType = null;
	[SerializeField] private WorkerPool m_WorkerPool = null;
	[SerializeField] private ISelectable m_SelectableComponent = null;

	public WorkerType GetWorkerType() 
	{
		return m_WorkerType;
	}

	public void SetWorkStation(IWorkStation workStation) 
	{

	}

	public void ResetWorkStation() 
	{

	}

	private void Awake()
	{
		m_WorkerPool.AddWorkersToPool(this, m_WorkerType);
		m_SelectableComponent.AddListener(this);
	}

	private void OnDestroy()
	{
		m_WorkerPool.RemoveWorkersFromPool(this);
	}

	public void OnSelect()
	{

	}

	public void OnDeselect()
	{

	}
}

public interface IWorkerListener : IListenerInterface
{
	void OnWorkerChangedStatus();
}

public enum WorkerStatus 
{
	Travelling,
	Working,
	Leisure,
	Sleeping
}
