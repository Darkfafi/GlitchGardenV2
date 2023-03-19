using RaDataHolder;
using UnityEngine;

namespace Game.Battle
{
	public abstract class InBattleGameConditionBase : RaMonoDataHolderBase<BattleGame>
	{
		[SerializeField]
		private BattlePlayer.Type _playerType = BattlePlayer.Type.Home;

		[SerializeField]
		private bool _setCondition = false;

		public BattlePlayer.Type PlayerSideType => _playerType;
		public BattlePlayerSide PlayerSide => Data != null ? Data.GetPlayerSide(_playerType) : null;

		protected override void OnSetData()
		{
		}

		protected override void OnSetDataResolved()
		{
			OnStartRunning();
			SetInGame(CheckCondition());
		}

		protected override void OnClearData()
		{
			OnStopRunning();
			SetInGame(false);
		}

		protected abstract void OnStartRunning();
		protected abstract void OnStopRunning();

		protected abstract bool CheckCondition();

		protected void SetInGame(bool inGame)
		{
			if(_setCondition == inGame)
			{
				return;
			}

			if(!HasData)
			{
				return;
			}

			_setCondition = inGame;

			if(inGame)
			{
				PlayerSide.InGameFlags.Register(this);
			}
			else
			{
				PlayerSide.InGameFlags.Unregister(this);
			}
		}
	}
}