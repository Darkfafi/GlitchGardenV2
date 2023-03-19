using UnityEngine;

namespace Game.Battle
{
	public class BattleProjectileDamageEffect : BattleProjectileEffectBase
	{
		[SerializeField]
		private int _damageAmount = 1;

		public override void ApplyEffect(BattleProjectile projectile, BattleUnit hitTarget)
		{
			hitTarget.Health.Damage(_damageAmount);
			Destroy(projectile.gameObject);
		}
	}
}