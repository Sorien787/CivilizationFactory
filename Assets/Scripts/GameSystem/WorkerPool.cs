using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorkerPool : IGameSubsystem<IWorkerListener>
{
    private Dictionary<WorkerType, HashSet<IWorker>> m_Workers = new Dictionary<WorkerType, HashSet<IWorker>>();

	public override void GameFinish()
	{
		m_Workers.Clear();
	}

	public override void GameStart()
	{

	}

	public HashSet<IWorker> GetWorkersByType(in WorkerType workerType) 
	{
		if (!m_Workers.ContainsKey(workerType))
			return new HashSet<IWorker>();

		return m_Workers[workerType];
	}

	public void OnWorkerAdded(IWorker worker) 
	{
		if (!m_Workers.ContainsKey(worker.GetWorkerType()))
			m_Workers.Add(worker.GetWorkerType(), new HashSet<IWorker>());

		m_Workers[worker.GetWorkerType()].Add(worker);
	}

	public void OnWorkerRemoved(IWorker worker) 
	{
		WorkerType workerType = worker.GetWorkerType();
		if (!m_Workers.ContainsKey(workerType))
			return;

		HashSet<IWorker> workersOfType = m_Workers[workerType];

		if (!workersOfType.Contains(worker))
			return;

		workersOfType.Remove(worker);

		if (workersOfType.Count > 0)
			return;

		m_Workers.Remove(workerType);
	}

	public List<IWorker> OnRequestWorkers(int numWorkers, WorkerType workerType) 
	{
		return new List<IWorker>();
	}

	public void OnReleaseWorkers(WorkerType workerType, List<IWorker> workers) 
	{

	}

	public int GetNumWorkersAvailable(WorkerType workerType) 
	{
		return m_Workers[workerType].Count;
	}

	public override void GameTick()
	{
	}
}

public interface IWorkerPoolListener : IListenerInterface
{
	void OnWorkerAdded(WorkerType workerType);

	void OnWorkerRemoved(WorkerType workerType);
}
