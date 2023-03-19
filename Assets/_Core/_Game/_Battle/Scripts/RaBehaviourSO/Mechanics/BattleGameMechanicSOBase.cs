using RaBehaviourSO;
using UnityEngine;
using RaFlags;

namespace Game.Battle
{
	public abstract class BattleGameMechanicSOBase : RaBehaviourSOBase
	{
		public RaFlagsTracker IsEnabledFlags
		{
			get; private set;
		}

		public bool IsEnabled => IsEnabledFlags != null && !IsEnabledFlags.IsEmpty();

		protected override void OnSetup()
		{
			IsEnabledFlags = new RaFlagsTracker();
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
			if(dependency is BattleGameModelSOBase modelSO)
			{
				if(!modelSO.HasSetData)
				{
					message = "Data not set!";
					return false;
				}
			}
			message = default;
			return true;
		}
	}
}