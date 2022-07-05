using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WorkStationUIPanel : MonoBehaviour, IUIPanelListener, IWorkStationListener, IWorkStationPopulationUIListener
{
	[SerializeField] private SelectionSystem m_SelectionSystem = null;

	[SerializeField] private UIPanel m_Panel;

	[SerializeField] private RectTransform m_PopulationUIContainer = null;

	[SerializeField] private GameObject m_PopulationUIPrefab = null;

	[SerializeField] private WorkerPool m_WorkerPool = null;

	[SerializeField] private TextMeshProUGUI m_SlotsInformationText = null;

	[SerializeField] private TextMeshProUGUI m_FactoryNameText = null;

	[SerializeField] private TextMeshProUGUI m_FactoryDescriptionText = null;

	[SerializeField] private TextMeshProUGUI m_FactoryEfficiencyText = null;

	private readonly List<WorkStationPopulationUIElement> m_PopulationElements = new();
	private void Awake() 
	{
		m_Panel.m_Listeners.AddListener(this);
	}
	private IWorkStation GetWorkStation() 
	{
		if (m_SelectionSystem.GetCurrentSelectedObject().GetGameObject())
			return null;
		return m_SelectionSystem.GetCurrentSelectedObject().GetGameObject().GetComponent<IWorkStation>();
	}


	private void UpdateUIElement(WorkerType workerType, WorkStationPopulationUIElement uiElement) 
	{
		IWorkStation currentWorkStation = GetWorkStation();
		int numWorkersInPool = m_WorkerPool.GetNumWorkersOfType(workerType);
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
		IWorkStation currentWorkStation = GetWorkStation();
		OnSetMachineEfficiencyVal(currentWorkStation);
	}

	private void OnSetMachineEfficiencyVal(IWorkStation currentWorkStation) 
	{
		m_FactoryEfficiencyText.text = currentWorkStation.GetCurrentMachineEfficiency().ToString();
	}

	private void OnSetMachineBasicData(IWorkStation currentWorkStation) 
	{
		m_FactoryDescriptionText.text = currentWorkStation.GetDescription();
		m_FactoryNameText.text = currentWorkStation.GetName();
	}

	private void OnSetMachineSlotData(IWorkStation currentWorkStation) 
	{
		m_SlotsInformationText.text = string.Format("%s / %s", currentWorkStation.GetNumFilledSlots().ToString(), currentWorkStation.GetTotalNumSlots().ToString());
	}

	public void OnWorkersChanged(WorkerType workerType)
	{
		UpdateUIElementOfType(workerType);
	}

	public void OnAddSinglePressed(WorkerType workerType)
	{
		GetWorkStation().OnWorkerAdded(workerType);
	}

	public void OnAddMaxPressed(WorkerType workerType)
	{
		GetWorkStation().OnWorkersMaximised(workerType);
	}

	public void OnRemoveSinglePressed(WorkerType workerType)
	{
		GetWorkStation().OnWorkerRemoved(workerType);
	}

	public void OnRemoveMaxPressed(WorkerType workerType)
	{
		GetWorkStation().OnWorkersMinimized(workerType);
	}

	public void OnShowUIPanel()
	{

		IWorkStation currentWorkStation = GetWorkStation();
		currentWorkStation.AddListener(this);
		List<WorkerType> workerTypesToDisplay = currentWorkStation.GetWorkerTypesToDisplay();
		int numNewWorkerTypesToDisplay = workerTypesToDisplay.Count;
		int numCurrentWorkerTypesDisplayed = m_PopulationElements.Count;

		if (numNewWorkerTypesToDisplay < numCurrentWorkerTypesDisplayed)
			m_PopulationElements.RemoveRange(numCurrentWorkerTypesDisplayed, numNewWorkerTypesToDisplay - numCurrentWorkerTypesDisplayed);
		else
		{
			for (int workerIndex = numCurrentWorkerTypesDisplayed; workerIndex < numNewWorkerTypesToDisplay; workerIndex++)
			{
				GameObject instantiatedUI = Instantiate(m_PopulationUIPrefab, m_PopulationUIContainer);
				WorkStationPopulationUIElement instantiatedUIElement = instantiatedUI.GetComponent<WorkStationPopulationUIElement>();
				m_PopulationElements.Add(instantiatedUIElement);
				instantiatedUIElement.AddListener(this);
			}
		}

		for (int i = 0; i < m_PopulationElements.Count; i++)
		{
			WorkerType workerTypeToDisplay = workerTypesToDisplay[i];
			WorkStationPopulationUIElement uiElement = m_PopulationElements[i];
			uiElement.Setup(workerTypeToDisplay);
			UpdateUIElement(workerTypeToDisplay, uiElement);
		}

		OnSetMachineEfficiencyVal(currentWorkStation);
		OnSetMachineBasicData(currentWorkStation);
		OnSetMachineSlotData(currentWorkStation);
	}

	public void OnHideUIPanel()
	{
		GetWorkStation().RemoveListener(this);
	}

	public void OnRefreshUIPanel()
	{

	}
}
