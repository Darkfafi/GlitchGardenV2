using UnityEngine;

namespace Game
{
	public class Health
	{
		public delegate void HPHandler(Health health, int delta);
		public event HPHandler HealedEvent;
		public event HPHandler DamagedEvent;
		public event HPHandler HealthChangedEvent;

		public int HP
		{
			get; private set;
		}

		public int MaxHP
		{
			get; private set;
		}

		public bool IsAlive => HP > 0;

		public float NormalizedHealth => MaxHP > 0f ? Mathf.Clamp01((float)HP / MaxHP) : 0f;

		public Health(int amount)
		{
			MaxHP = HP = amount;
		}

		public void Damage(int amount)
		{
			int preHP = HP;
			amount = Mathf.Max(amount, 0);
			HP = Mathf.Max(HP - amount, 0);

			int delta = HP - preHP;
			DamagedEvent?.Invoke(this, Mathf.Abs(delta));
			HealthChangedEvent?.Invoke(this, delta);
		}

		public void Heal(int amount)
		{
			int preHP = HP;
			amount = Mathf.Max(amount, 0);
			HP = Mathf.Min(HP + amount, MaxHP);

			int delta = HP - preHP;
			HealedEvent?.Invoke(this, Mathf.Abs(delta));
			HealthChangedEvent?.Invoke(this, delta);
		}
	}
}