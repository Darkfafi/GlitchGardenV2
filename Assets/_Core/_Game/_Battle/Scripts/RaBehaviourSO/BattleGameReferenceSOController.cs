using UnityEngine;

namespace Game.Battle
{
	public class BattleGameReferenceSOController : BehavioursSOControllerBase<BattleGameReferenceSOBase>
	{
		[SerializeField]
		private BattleGame _game;

		protected override void OnInit(BattleGameReferenceSOBase behaviourSO)
		{
			behaviourSO.SetData(_game);
		}

		protected override void OnDeinit(BattleGameReferenceSOBase behaviourSO)
		{
			behaviourSO.ClearData();
		}
	}
}