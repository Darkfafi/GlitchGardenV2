using UnityEngine;

namespace Game.Battle
{
	public class BattleGameModelsSOController : BehavioursSOControllerBase<BattleGameModelSOBase>
	{
		[SerializeField]
		private BattleGame _game;

		protected override void OnInit(BattleGameModelSOBase behaviourSO)
		{
			behaviourSO.SetData(_game);
		}

		protected override void OnDeinit(BattleGameModelSOBase behaviourSO)
		{
			behaviourSO.ClearData();
		}
	}
}