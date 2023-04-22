using RaFSM;
using UnityEngine;

public class UnityState : RaGOStateBase
{
	[SerializeField]
	private bool _disableOnInactiveState = false;

	protected override void OnInit()
	{
		if(_disableOnInactiveState)
		{
			gameObject.SetActive(false);
		}
	}

	protected override void OnDeinit()
	{
		if(_disableOnInactiveState)
		{
			gameObject.SetActive(false);
		}
	}

	protected override void OnEnter()
	{
		if(_disableOnInactiveState)
		{
			gameObject.SetActive(true);
		}
	}

	protected override void OnExit(bool isSwitch)
	{
		if(_disableOnInactiveState)
		{
			gameObject.SetActive(false);
		}
	}
}
