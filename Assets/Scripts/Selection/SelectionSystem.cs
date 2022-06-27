using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSystem : IGameSubsystem<ISelectionListener>
{

	private static NullSelectable m_NullSelectable = new NullSelectable();

	private ISelectableInterface m_CurrentSelectableObject = null;
	private ISelectableInterface m_CurrentHoveredObject = null;
	public override void GameFinish()
	{

	}

	public override void GameTick()
	{
		Camera camera = Camera.main;
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 10, QueryTriggerInteraction.Ignore);
		GameObject hoveredObject = hit.transform.gameObject;

		ISelectableInterface selectable = null;
		if (!hoveredObject || !hoveredObject.GetComponent<ISelectable>())
			selectable = m_NullSelectable;
		else
			selectable = hoveredObject.GetComponent<ISelectable>();
		
		if (selectable != m_CurrentHoveredObject)
		{
			m_CurrentHoveredObject.OnHovered(null);
			m_CurrentHoveredObject = selectable;
			m_CurrentHoveredObject.OnUnhovered();
		}

		if (Input.GetMouseButtonDown(0))
			SetSelectedObject(m_CurrentHoveredObject);
	}

	public override void GameStart()
	{
		m_CurrentSelectableObject = m_NullSelectable;
	}

	public void SetSelectedObject(ISelectableInterface newSelectedObject) 
	{
		if (m_CurrentSelectableObject == newSelectedObject)
			return;

		m_CurrentSelectableObject.OnDeselect();
		m_CurrentSelectableObject = newSelectedObject;
		m_CurrentSelectableObject.OnSelect();
	}

	public ISelectable GetCurrentSelectedObject() 
	{
		return m_CurrentSelectableObject;
	}
}

public class NullSelectable : ISelectableInterface
{
	public void OnDeselect(){}
	public void OnHovered(in GameObject otherObject){}
	public void OnSelect(){}
	public void OnUnhovered(){}
}

public interface ISelectionListener : IListenerInterface 
{

}
