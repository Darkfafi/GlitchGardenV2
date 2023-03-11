using RaFSM;
using UnityEngine;

public class StateNameMarker : MonoBehaviour
{
	[SerializeField]
	private string _nameMark = " <---";

	public void SetName(RaGOStateBase newState, RaGOStateBase oldState)
	{
		ClearName(oldState);
		SetName(newState);
	}

	public void SetName(RaGOStateBase state)
	{
		if(state != null)
		{
			state.name += _nameMark;
		}
	}

	public void ClearName(RaGOStateBase state)
	{
		if(state != null)
		{
			state.name = state.name.Replace(_nameMark, "");
		}
	}
}
