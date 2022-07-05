using UnityEngine;

[CreateAssetMenu(menuName ="Placeable Object Type")]
public class PlaceableObject : ScriptableObject, IPlaceableObject
{
	[SerializeField] Mesh m_RenderMesh = null;
	[SerializeField] Sprite m_SpriteForRendering = null;

	public Sprite GetSpriteForRendering()
	{
		return m_SpriteForRendering;
	}
	public Mesh GetMeshToRender(RaycastHit hit)
	{
		return m_RenderMesh;
	}

	public bool GetOccluded(RaycastHit hit)
	{
		return false;
	}

	public virtual void OnSetupPlacedObjectInternal(GameObject newObject) { }

	public IPlaceableObject OnSuccessfulPlacingAtPosition(RaycastHit hit)
	{
		return this;
	}
}

public interface IPlaceableObject 
{
    Mesh GetMeshToRender(RaycastHit hit);
    bool GetOccluded(RaycastHit hit);
    IPlaceableObject OnSuccessfulPlacingAtPosition(RaycastHit hit);

	void OnSetupPlacedObjectInternal(GameObject newObject);
}
