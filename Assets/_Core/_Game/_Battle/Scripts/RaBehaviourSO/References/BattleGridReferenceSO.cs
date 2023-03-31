using UnityEngine;

namespace Game.Battle
{

	[CreateAssetMenu(menuName = "Behaviours/References/BattleGridReferenceSO")]
	public class BattleGridReferenceSO : BattleGameReferenceSOBase
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