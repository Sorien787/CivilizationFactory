using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimUIPanel : UIPanel
{
    [SerializeField] private Animator m_Animator = null;


	public override void ShowUIPanel()
	{
		m_Animator.SetTrigger("Show");
	}

	public override void HideUIPanel()
	{
		m_Animator.SetTrigger("Hide");
	}
}
