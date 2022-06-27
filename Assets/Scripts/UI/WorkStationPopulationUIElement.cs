using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WorkStationPopulationUIElement : IListenerMonoBehaviour<WorkStationPopulationUIListener>
{
    // Start is called before the first frame update
    private WorkerType m_WorkerType = null;

    [SerializeField] private Button m_AddSingleButton = null;

    [SerializeField] private Button m_AddMaxButton = null;

    [SerializeField] private Button m_RemoveSingleButton = null;

    [SerializeField] private Button m_RemoveAllButton = null;

    [SerializeField] private Text m_WorkerInformationText = null;

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
    void OnAddSingle() 
    {
        ForEachListener((WorkStationPopulationUIListener listener) => listener.OnAddSinglePressed());
    }

    void OnAddMax() 
    {
        ForEachListener((WorkStationPopulationUIListener listener) => listener.OnAddMaxPressed());
    }

    void OnRemoveSingle() 
    {
        ForEachListener((WorkStationPopulationUIListener listener) => listener.OnRemoveSinglePressed());
    }

    void OnRemoveMax() 
    {
        ForEachListener((WorkStationPopulationUIListener listener) => listener.OnRemoveMaxPressed());
    }
}

public interface WorkStationPopulationUIListener : IListenerInterface 
{
    void OnAddSinglePressed();
    void OnAddMaxPressed();
    void OnRemoveSinglePressed();
    void OnRemoveMaxPressed();
}