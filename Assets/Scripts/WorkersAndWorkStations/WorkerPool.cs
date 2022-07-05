using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Worker Pool System")]
public class WorkerPool : IGameSubsystem
{ 
	private readonly WorkerContainer m_WorkerContainer = new();
	public readonly CListener<IWorkerPoolListener> m_Listeners = new();
	public void AddWorkersToPool(List<IWorker> workers, WorkerType workerType) 
	{
		m_WorkerContainer.AddWorkersOfType(workerType, workers);
		m_Listeners.ForEachListener((IWorkerPoolListener listener) => listener.OnWorkersAdded(workerType));
	}

	public void AddWorkersToPool(IWorker workers, WorkerType workerType)
	{
		m_WorkerContainer.AddWorkersOfType(workerType, workers);
		m_Listeners.ForEachListener((IWorkerPoolListener listener) => listener.OnWorkersAdded(workerType));
	}

	public int GetNumWorkersOfType(WorkerType workerType) 
	{
		return m_WorkerContainer.GetNumWorkersOfType(workerType);
	}

	public void RemoveWorkersFromPool(IWorker worker)
	{
		m_WorkerContainer.RemoveWorker(worker);
		m_Listeners.ForEachListener((IWorkerPoolListener listener) => listener.OnWorkersRemoved(worker.GetWorkerType()));
	}

	public List<IWorker> RemoveWorkersFromPool(WorkerType workerType, in int numWorkers) 
	{
		List<IWorker> workers = m_WorkerContainer.RequestWorkersByType(workerType, numWorkers);
		m_Listeners.ForEachListener((IWorkerPoolListener listener) => listener.OnWorkersRemoved(workerType));
		return workers;
	}


	public override void GameFinish()
	{
		m_WorkerContainer.Clear();
	}

	public override void GameStart()
	{

	}

	public override void GameTick()
	{

	}
}

public class WorkerContainer
{
	Dictionary<WorkerType, WorkerTypePool> m_pool = new Dictionary<WorkerType, WorkerTypePool>();

	public void Clear() 
	{
		m_pool.Clear();
	}

	public List<IWorker> RequestWorkersByType(in WorkerType workerType, in int numWorkers) 
	{
		if (!m_pool.ContainsKey(workerType))
			return new List<IWorker>();
		return m_pool[workerType].GetWorkers(numWorkers);
	}

	public void AddWorkersOfType(in WorkerType workerType, params IWorker[] workers) 
	{
		if (!m_pool.ContainsKey(workerType))
			m_pool.Add(workerType, new WorkerTypePool(workers));
		m_pool[workerType].AddWorkers(workers);
	}

	public void AddWorkersOfType(in WorkerType workerType, List<IWorker> workers)
	{
		if (!m_pool.ContainsKey(workerType))
			m_pool.Add(workerType, new WorkerTypePool(workers));
		m_pool[workerType].AddWorkers(workers);
	}


	public void RemoveWorker(in IWorker worker) 
	{
		if (!m_pool.ContainsKey(worker.GetWorkerType()))
			return;
		m_pool[worker.GetWorkerType()].RemoveWorker(worker);
	}

	public int GetNumWorkersOfType(in WorkerType workerType) 
	{
		if (!m_pool.ContainsKey(workerType))
			return 0;
		return m_pool[workerType].GetNumWorkersAvailable();
	}
}

public class WorkerTypePool
{
	public WorkerTypePool(params IWorker[] workers) 
	{
		m_Workers.AddRange(workers);
	}

	public WorkerTypePool(List<IWorker> workers)
	{
		m_Workers.AddRange(workers);
	}

	private List<IWorker> m_Workers = new List<IWorker>();
	public List<IWorker> AccessWorkers()
	{
		return m_Workers;
	}

	public void AddWorkers(params IWorker[] workers)
	{
		m_Workers.AddRange(workers);
	}

	public void AddWorkers(List<IWorker> workers) 
	{
		m_Workers.AddRange(workers);
	}

	public List<IWorker> GetWorkers(int numWorkers)
	{
		List<IWorker> workers = new List<IWorker>();
		if (numWorkers < m_Workers.Count)
		{
			for (int i = 0; i < numWorkers; i++)
			{
				workers.Add(m_Workers[m_Workers.Count - 1 - i]);
			}
			m_Workers.RemoveRange(m_Workers.Count - numWorkers, numWorkers);
		}
		return workers;
	}

	public void RemoveWorker(IWorker worker)
	{
		m_Workers.Remove(worker);
	}

	public int GetNumWorkersAvailable()
	{
		return m_Workers.Count;
	}
}

public interface IWorkerPoolListener : IListenerInterface
{
	void OnWorkersRemoved(WorkerType workerType);

	void OnWorkersAdded(WorkerType workerType);
}
