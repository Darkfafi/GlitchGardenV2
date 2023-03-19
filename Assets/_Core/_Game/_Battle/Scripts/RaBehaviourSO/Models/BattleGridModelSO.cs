using UnityEngine;

namespace Game.Battle
{

	[CreateAssetMenu(menuName = "Behaviours/Models/BattleGridModelSO")]
	public class BattleGridModelSO : BattleGameModelSOBase
	{
		public BattleGameGrid Grid
		{
			get; private set;
		}

		protected override void OnSetup()
		{
			Grid = Game.Grid;
		}

		protected override void OnStart()
		{

		}

		protected override void OnEnd()
		{
			Grid = null;
		}
	}
}