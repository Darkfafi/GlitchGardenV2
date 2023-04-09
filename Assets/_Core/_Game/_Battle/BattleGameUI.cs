using Game.Battle.UI;
using RaDataHolder;
using UI;
using UnityEngine;

namespace Game.Battle
{
	public class BattleGameUI : RaMonoDataHolderBase<BattleGameSceneRoot>
	{
		[field: SerializeField]
		public BattleGameplayUIGroup GameplayUIGroup
		{
			get; private set;
		}

		[field: SerializeField]
		public GenericUIGroup WinUIGroup
		{
			get; private set;
		}

		[field: SerializeField]
		public GenericUIGroup LoseUIGroup
		{
			get; private set;
		}

		protected override void OnSetData()
		{
			GameplayUIGroup.SetData(Data.HomePlayerSide.Player);
		}

		protected override void OnClearData()
		{
			GameplayUIGroup.ClearData();
		}

		protected override void OnSetDataResolved()
		{
			GameplayUIGroup.Resolve();
		}
	}
}