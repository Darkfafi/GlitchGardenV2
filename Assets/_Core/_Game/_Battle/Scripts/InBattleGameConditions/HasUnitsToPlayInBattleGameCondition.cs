using System.Collections;
using UnityEngine;

namespace Game.Battle
{
	public class HasUnitsToPlayInBattleGameCondition : InBattleGameConditionBase
	{
		private Coroutine _checkRoutine = null;

		protected override void OnStartRunning()
		{
			_checkRoutine = StartCoroutine(CheckConditionRoutine());
		}

		protected override void OnStopRunning()
		{
			if(_checkRoutine != null)
			{
				StopCoroutine(_checkRoutine);
				_checkRoutine = null;
			}
		}

		protected override bool CheckCondition() => PlayerSide.HasResourcesForUnits();

		private IEnumerator CheckConditionRoutine()
		{
			while(HasData)
			{
				SetInGame(CheckCondition());
				yield return new WaitForSeconds(1f);
			}
			_checkRoutine = null;
		}
	}
}