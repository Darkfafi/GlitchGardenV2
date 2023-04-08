namespace Game.Battle
{
	public class HasHealthInBattleGameCondition : InBattleGameConditionBase
	{
		protected override void OnStartRunning()
		{
			if(Player.Health != null)
			{
				Player.Health.HealthChangedEvent += OnHealthChangedEvent;
			}
		}

		protected override void OnStopRunning()
		{
			if(Player.Health != null)
			{
				Player.Health.HealthChangedEvent -= OnHealthChangedEvent;
			}
		}

		protected override bool CheckCondition() => Player.Health != null && Player.Health.IsAlive;

		private void OnHealthChangedEvent(Health health, int delta)
		{
			SetInGame(CheckCondition());
		}
	}
}
