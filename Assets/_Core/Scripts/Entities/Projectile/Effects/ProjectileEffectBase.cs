using UnityEngine;

public abstract class ProjectileEffectBase : MonoBehaviour
{
	public abstract void ApplyEffect(Projectile projectile, Unit hitTarget);
}
