using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Placing Tool")]
public class PlacingSystem : ITool
{
	private static readonly NullPlaceable m_nullPlaceable = new();
	private IPlaceableObject m_currentPlaceableObject = m_nullPlaceable;
	[SerializeField] private IToolSystem m_ToolSystem = null;

	[SerializeField] private Material m_validPlacementMaterial = null;
	[SerializeField] private Material m_invalidPlacementMaterial = null;
	private float m_CurrentRotation = 0.0f;
	private Camera m_camera = null;

	public override void GameStart()
	{
		Reset();
	}

	public override void OnToolEnd()
	{
		Reset();
	}

	public void StartPlacingObject(in IPlaceableObject newPlaceable) 
	{
		if (m_currentPlaceableObject == newPlaceable) 
		{
			m_ToolSystem.OnToolDeactivated();
			return;
		}

		m_ToolSystem.OnNewToolActivated(this);
		m_currentPlaceableObject = newPlaceable;
	}

	private void Reset() 
	{
		m_camera = Camera.main;
		m_currentPlaceableObject = m_nullPlaceable;
		m_CurrentRotation = 0.0f;
	}


	public override void OnToolTick()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			m_ToolSystem.OnToolDeactivated();
			return;
		}

		m_CurrentRotation += Input.mouseScrollDelta.y;
		m_CurrentRotation %= 360.0f;

		Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
		if (!hit.collider)
			return;

		Material usedPlacementMaterial = m_validPlacementMaterial;
		bool isPlaceable = true;
		if (m_currentPlaceableObject.GetOccluded(hit))
		{
			isPlaceable = false;
			usedPlacementMaterial = m_invalidPlacementMaterial;
		}

		Quaternion objectRotation = Quaternion.AngleAxis(m_CurrentRotation, Vector3.up);
		Matrix4x4 mat = Matrix4x4.TRS(hit.point, objectRotation, Vector3.one);
		Mesh meshToRender = m_currentPlaceableObject.GetMeshToRender(hit);
		if (meshToRender)
			Graphics.DrawMesh(meshToRender, mat, usedPlacementMaterial, 0);

		if (!Input.GetMouseButtonDown(0) || !isPlaceable)
			return;

		m_currentPlaceableObject = m_currentPlaceableObject.OnSuccessfulPlacingAtPosition(hit);
	}
}

public class NullPlaceable : IPlaceableObject
{
	public Mesh GetMeshToRender(RaycastHit hit)
	{
		return null;
	}

	public bool GetOccluded(RaycastHit hit)
	{
		return true;
	}

	public void OnSetupPlacedObjectInternal(GameObject newObject)
	{

	}

	public IPlaceableObject OnSuccessfulPlacingAtPosition(RaycastHit hit)
	{
		return this;
	}
}

public interface IPlacingListener : IListenerInterface
{

}