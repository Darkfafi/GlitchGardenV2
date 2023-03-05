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
		state.name += _nameMark;
	}

	public void ClearName(RaGOStateBase state)
	{
		state.name = state.name.Replace(_nameMark, "");
	}
}
