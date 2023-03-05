using RaFSM;
using UnityEngine;
using UnityEngine.Events;

public class ChargeState : RaGOStateBase
{
	[SerializeField]
	private float _duration = 1f;

	public UnityEvent ChangedEvent;

	public float Time
	{
		get; private set;
	}

	public float Duration => _duration;

	public float NormalizedTime => Mathf.Clamp01(Time / Duration);

	protected override void OnInit()
	{

	}

	protected override void OnEnter()
	{
		Time = 0f;
	}

	protected void Update()
	{
		if(IsCurrentState)
		{
			Time += UnityEngine.Time.deltaTime;
			if(Mathf.Approximately(NormalizedTime, 1f))
			{
				ChangedEvent.Invoke();
			}
		}
	}

	protected override void OnExit(bool isSwitch)
	{

	}

	protected override void OnDeinit()
	{

	}
}
