using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationPlaceable : PlaceableObject
{
	[SerializeField] private WorkStationType m_WorkStationType = null;
	public override void OnSetupPlacedObjectInternal(GameObject newObject)
	{
		newObject.GetComponent<IWorkStation>().SetMachineType(m_WorkStationType);
	}
}
