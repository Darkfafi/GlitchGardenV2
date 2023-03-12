using UnityEngine;

public class ProjectileDamageEffect : ProjectileEffectBase
{
	[SerializeField]
	private int _damageAmount = 1;

	public override void ApplyEffect(Projectile projectile, Unit hitTarget)
	{
		hitTarget.Health.Damage(_damageAmount);
		Destroy(projectile.gameObject);
	}
}
