using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementUIElement : MonoBehaviour
{
    [SerializeField] private PlacingSystem m_PlacingSystem = null;
    [SerializeField] private PlaceableObject m_PlaceableObjectType = null;
	[SerializeField] private Image m_ButtonImage = null;
	private void Awake()
	{
		m_ButtonImage.sprite = m_PlaceableObjectType.GetSpriteForRendering();
	}

	void OnButtonPressed() 
    {
        m_PlacingSystem.StartPlacingObject(m_PlaceableObjectType);
    }
}
