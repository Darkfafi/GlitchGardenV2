using RaBehaviourSO;

namespace Game.Battle
{

	public abstract class BattleGameReferenceSOBase : RaBehaviourSOBase
	{
		protected BattleGameSceneRoot Game
		{
			get; private set;
		}

		public bool HasSetData
		{
			get; private set;
		}

		public void SetData(BattleGameSceneRoot game)
		{
			if(IsInitialized)
			{
				return;
			}

			Game = game;
			HasSetData = true;
		}

		public void ClearData()
		{
			if(!IsInitialized)
			{
				return;
			}

			HasSetData = false;
			Game = null;
		}

		protected override void OnDispose()
		{
			Game = null;
		}
	}
}