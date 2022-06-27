
using UnityEngine;
public class ISelectable : IListenerMonoBehaviour<ISelectableListener>, ISelectableInterface
{
	public void OnDeselect()
	{

	}

	public void OnHovered(in GameObject otherObject)
	{

	}

	public void OnSelect()
	{

	}

	public void OnUnhovered()
	{

	}
}

public interface ISelectableInterface 
{
    void OnSelect();

    void OnHovered(in GameObject otherObject);

    void OnUnhovered();

    void OnDeselect();
}

public interface ISelectableListener : IListenerInterface
{
    void OnSelect();

    void OnDeselect();
}
