using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WorkStationUIPanel : UIPanel , IWorkStationListener
{
	[SerializeField] private SelectionSystem m_SelectionSystem = null;

	[SerializeField] private RectTransform m_PopulationUIContainer = null;

	[SerializeField] private GameObject m_PopulationUIPrefab = null;

	[SerializeField] private WorkerPool m_WorkerPool = null;

	private List<WorkStationPopulationUIElement> m_PopulationElements = new List<WorkStationPopulationUIElement>();


	private IWorkStation GetWorkStation() 
	{
		return m_SelectionSystem.GetCurrentSelectedObject().GetComponent<IWorkStation>();
	}

	public override void HideUIPanel()
	{
		GetWorkStation().RemoveListener(this);
	}

	public override void RefreshUIPanel()
	{

	}

	public override void ShowUIPanel()
	{
		IWorkStation currentWorkStation = GetWorkStation();
		currentWorkStation.AddListener(this);
		List<WorkerType> workerTypesToDisplay = currentWorkStation.GetWorkerTypesToDisplay();
		int numNewWorkerTypesToDisplay = workerTypesToDisplay.Count;
		int numCurrentWorkerTypesDisplayed = m_PopulationElements.Count;
		int workerIndex = 0;

		if (numNewWorkerTypesToDisplay < numCurrentWorkerTypesDisplayed)
			m_PopulationElements.RemoveRange(numCurrentWorkerTypesDisplayed, numNewWorkerTypesToDisplay - numCurrentWorkerTypesDisplayed);
		else 
		{
			for (workerIndex = numCurrentWorkerTypesDisplayed; workerIndex < numNewWorkerTypesToDisplay; workerIndex++)
			{
				GameObject instantiatedUI = Instantiate(m_PopulationUIPrefab, m_PopulationUIContainer);
				WorkStationPopulationUIElement instantiatedUIElement = instantiatedUI.GetComponent<WorkStationPopulationUIElement>();
				m_PopulationElements.Add(instantiatedUIElement);
			}
		}
		
		for (int i = 0; i < m_PopulationElements.Count; i++) 
		{
			WorkerType workerTypeToDisplay = workerTypesToDisplay[i];
			WorkStationPopulationUIElement uiElement = m_PopulationElements[i];
			uiElement.Setup(workerTypeToDisplay);
			UpdateUIElement(workerTypeToDisplay, uiElement);
		}	
	}

	private void UpdateUIElement(WorkerType workerType, WorkStationPopulationUIElement uiElement) 
	{
		IWorkStation currentWorkStation = GetWorkStation();
		int numWorkersInPool = m_WorkerPool.GetNumWorkersAvailable(workerType);
		int numWorkersInWorkStation = currentWorkStation.GetNumWorkersOfType(workerType);
		int maxNumWorkersInWorkStation = currentWorkStation.GetMaxNumWorkersOfType(workerType);
		int numSlotsInWorkStation = currentWorkStation.GetTotalNumSlots();
		int numSlotsFilledInWorkStation = currentWorkStation.GetNumFilledSlots();
		bool canAddWorkersToWorkStation = currentWorkStation.CanAddMoreWorkersOfType(workerType);
		bool canRemoveWorkersFromWorkStation = currentWorkStation.CanRemoveMoreWorkersOfType(workerType);
		uiElement.UpdateElement(numWorkersInPool, numWorkersInWorkStation, maxNumWorkersInWorkStation, numSlotsFilledInWorkStation , numSlotsInWorkStation, canAddWorkersToWorkStation, canRemoveWorkersFromWorkStation);
	}

	private void UpdateUIElementOfType(WorkerType workerType) 
	{
		for (int i = 0; i < m_PopulationElements.Count; i++) 
		{
			if (m_PopulationElements[i].GetWorkerType() != workerType)
				continue;
			UpdateUIElement(workerType, m_PopulationElements[i]);
			return;
		}
	}

	public void OnMachineEfficiencyChanged()
	{

	}

	public void OnWorkersChanged(WorkerType workerType)
	{
		UpdateUIElementOfType(workerType);
	}
}
