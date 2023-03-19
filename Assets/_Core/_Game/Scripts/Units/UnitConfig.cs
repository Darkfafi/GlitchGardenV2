using Game.Battle;
using UnityEngine;

namespace Game
{
	[CreateAssetMenu(menuName = "Configs/UnitConfig", fileName = "UnitConfig")]
	public class UnitConfig : ScriptableObject
	{
		[field: SerializeField]
		public Sprite Icon
		{
			get; private set;
		}

		[field: SerializeField]
		public Color Color
		{
			get; private set;
		} = Color.white;

		[field: SerializeField]
		public BattleUnitConfigData BattleUnitConfigData
		{
			get; private set;
		} = new BattleUnitConfigData()
		{
			HealthPoints = 10,
		};
	}
}