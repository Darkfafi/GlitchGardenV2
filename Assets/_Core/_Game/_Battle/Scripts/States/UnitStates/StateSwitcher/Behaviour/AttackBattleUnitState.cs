using RaFSM;
using UnityEngine;

namespace Game.Battle
{
	public class AttackBattleUnitState : RaGOStateBase<TargetBattleUnitData>
	{
		[Header("Events")]
		public StateEvent AttackedEvents;
		public StateEvent FailedAttackEvents;

		[Header("Options")]
		[SerializeField]
		private int _damage = 1;

		[Header("Settings")]
		[SerializeField]
		private bool _attackOnEnter = true;

		protected override void OnInit()
		{

		}

		protected override void OnEnter()
		{
			if(_attackOnEnter)
			{
				Attack();
			}
		}

		protected override void OnExit(bool isSwitch)
		{

		}

		protected override void OnDeinit()
		{

		}

		public void Attack()
		{
			if(Dependency.CurrentTarget != null)
			{
				Dependency.CurrentTarget.Health.Damage(_damage);
				AttackedEvents.Invoke(this);
			}
			else
			{
				FailedAttackEvents.Invoke(this);
			}
		}
	}
}