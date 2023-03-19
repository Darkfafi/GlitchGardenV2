using UnityEngine;

namespace Game.Battle
{
	public class TargetBattleUnitData : MonoBehaviour
	{
		[field: SerializeField]
		public BattleUnit CurrentTarget
		{
			get; private set;
		}

		public bool SetTarget(BattleUnit unit)
		{
			if(CurrentTarget != unit)
			{
				CurrentTarget = unit;
				return true;
			}
			return false;
		}
	}
}