using UnityEngine;

namespace Game.Battle
{
	public class TargetBattlePlayerData : MonoBehaviour
	{
		[field: SerializeField]
		public BattlePlayer CurrentTarget
		{
			get; private set;
		}

		public bool SetTarget(BattlePlayer player)
		{
			if(CurrentTarget != player)
			{
				CurrentTarget = player;
				return true;
			}
			return false;
		}
	}
}