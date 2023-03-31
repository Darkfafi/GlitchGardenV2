using RaBehaviourSO;
using UnityEngine;
using RaFlags;
using System;

namespace Game.Battle
{
	public abstract class BattleGameMechanicSOBase : RaBehaviourSOBase
	{
		public event Action<bool> EnabledStateChangedEvent;

		public RaFlagsTracker IsEnabledFlags
		{
			get; private set;
		}

		public bool IsEnabled => IsEnabledFlags != null && !IsEnabledFlags.IsEmpty();

		protected override void OnSetup()
		{
			IsEnabledFlags = new RaFlagsTracker(OnEnabledStateChanged);
		}

		protected override void OnDispose()
		{
			if(IsEnabledFlags != null)
			{
				IsEnabledFlags.Dispose();
				IsEnabledFlags = null;
			}
		}

		protected override bool CheckDependency(ScriptableObject dependency, out string message)
		{
			if(dependency is BattleGameReferenceSOBase referenceSO)
			{
				if(!referenceSO.HasSetData)
				{
					message = "Data not set!";
					return false;
				}
			}
			message = default;
			return true;
		}

		private void OnEnabledStateChanged(bool isEmpty, RaFlagsTracker tracker)
		{
			EnabledStateChangedEvent?.Invoke(IsEnabled);
		}
	}
}