using RaFSM;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game
{
	public class ChargeState : RaGOStateBase
	{
		[SerializeField]
		private float _duration = 1f;

		[SerializeField]
		private bool _useRange = false;

		[SerializeField]
		private Vector2 _rangeDuration = Vector2Int.one;

		[FormerlySerializedAs("ChangedEvent")]
		public UnityEvent ChargedEvent;

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

			if(_useRange)
			{
				_duration = Random.Range(_rangeDuration.x, _rangeDuration.y);
			}
		}

		protected void Update()
		{
			if(IsCurrentState)
			{
				Time += UnityEngine.Time.deltaTime;
				if(Mathf.Approximately(NormalizedTime, 1f))
				{
					ChargedEvent.Invoke();
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
}