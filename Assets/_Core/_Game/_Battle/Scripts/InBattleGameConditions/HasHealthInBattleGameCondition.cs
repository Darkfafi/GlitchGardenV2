namespace Game.Battle
{
	public class HasHealthInBattleGameCondition : InBattleGameConditionBase
	{
		protected override void OnStartRunning()
		{
			if(PlayerSide.Health != null)
			{
				PlayerSide.Health.HealthChangedEvent += OnHealthChangedEvent;
			}
		}

		protected override void OnStopRunning()
		{
			if(PlayerSide.Health != null)
			{
				PlayerSide.Health.HealthChangedEvent -= OnHealthChangedEvent;
			}
		}

		protected override bool CheckCondition() => PlayerSide.Health != null && PlayerSide.Health.IsAlive;

		private void OnHealthChangedEvent(Health health, int delta)
		{
			SetInGame(CheckCondition());
		}
	}
}
