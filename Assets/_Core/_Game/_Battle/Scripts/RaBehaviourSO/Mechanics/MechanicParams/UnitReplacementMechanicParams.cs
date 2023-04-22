using UnityEngine;

namespace Game.Battle
{
	public class UnitReplacementMechanicParams : MonoBehaviour
	{
		[field: SerializeField]
		public BattleUnit UnitToReplace;

		[field: SerializeField]
		public UnitConfig NewUnitConfig;

		public UnitModel CreateUnitModel()
		{
			return new UnitModel(NewUnitConfig);
		}
	}
}