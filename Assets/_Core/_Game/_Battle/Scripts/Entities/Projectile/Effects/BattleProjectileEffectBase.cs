using UnityEngine;

namespace Game.Battle
{
	public abstract class BattleProjectileEffectBase : MonoBehaviour
	{
		public abstract void ApplyEffect(BattleProjectile projectile, BattleUnit hitTarget);
	}

}