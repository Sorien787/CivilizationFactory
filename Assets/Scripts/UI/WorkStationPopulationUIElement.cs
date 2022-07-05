using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class WorkStationPopulationUIElement : IListenerMonoBehaviour<IWorkStationPopulationUIListener>
{
    // Start is called before the first frame update
    private WorkerType m_WorkerType = null;

    [SerializeField] private Button m_AddSingleButton = null;

    [SerializeField] private Button m_AddMaxButton = null;

    [SerializeField] private Button m_RemoveSingleButton = null;

    [SerializeField] private Button m_RemoveAllButton = null;

    [SerializeField] private TextMeshProUGUI m_WorkerInformationText = null;

    public void Setup(in WorkerType workerType) 
    {
        m_WorkerType = workerType;
    }

    public void UpdateElement(in int totalWorkersInPool, in int totalWorkersInWorkStation, in int maxWorkersOfType, in int currentSlotsFilled, in int maxSlots, bool canAddWorkers, bool canRemoveWorkers) 
    {
        m_AddMaxButton.enabled = canAddWorkers;
        m_AddSingleButton.enabled = canAddWorkers;
        m_RemoveSingleButton.enabled = canRemoveWorkers;
        m_RemoveAllButton.enabled = canRemoveWorkers;
        m_WorkerInformationText.text = string.Format("%s / %s (%s)", totalWorkersInWorkStation, maxWorkersOfType, totalWorkersInPool);
    }

    public WorkerType GetWorkerType() 
    {
        return m_WorkerType;
    }
    public void OnAddSingle() 
    {
        ForEachListener((IWorkStationPopulationUIListener listener) => listener.OnAddSinglePressed(m_WorkerType));
    }

    public void OnAddMax() 
    {
        ForEachListener((IWorkStationPopulationUIListener listener) => listener.OnAddMaxPressed(m_WorkerType));
    }

    public void OnRemoveSingle() 
    {
        ForEachListener((IWorkStationPopulationUIListener listener) => listener.OnRemoveSinglePressed(m_WorkerType));
    }

    public void OnRemoveMax() 
    {
        ForEachListener((IWorkStationPopulationUIListener listener) => listener.OnRemoveMaxPressed(m_WorkerType));
    }
}

public interface IWorkStationPopulationUIListener : IListenerInterface 
{
    void OnAddSinglePressed(WorkerType workerType);
    void OnAddMaxPressed(WorkerType workerType);
    void OnRemoveSinglePressed(WorkerType workerType);
    void OnRemoveMaxPressed(WorkerType workerType);
}