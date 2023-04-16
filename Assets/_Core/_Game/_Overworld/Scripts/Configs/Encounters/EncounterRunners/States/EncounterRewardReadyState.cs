namespace Game.Campaign
{
	public class EncounterRewardReadyState : EncounterStateBase
	{
		public bool IsRewardClaimed
		{
			get; private set;
		}

		protected override void OnInit()
		{

		}

		protected override void OnEnter()
		{
			IsRewardClaimed = false;
		}

		protected override void OnExit(bool isSwitch)
		{

		}

		protected override void OnDeinit()
		{
			IsRewardClaimed = false;
		}

		public void ClaimReward()
		{
			if(IsCurrentState && !IsRewardClaimed)
			{
				IsRewardClaimed = true;
				Dependency.GoToNextState();
			}
		}
	}
}