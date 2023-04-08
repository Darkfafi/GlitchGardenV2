using UnityEngine;

namespace Game.Battle
{
	public class BattleProjectile : MonoBehaviour
	{
		[SerializeField]
		private float _speed = 5;

		[SerializeField]
		private Transform _orientationContainer = null;

		[SerializeField]
		private Rigidbody2D _rigidBody = null;

		[SerializeField]
		private BattlePlayerDetectionType _detectionType = BattlePlayerDetectionType.Opposite;

		[SerializeField]
		private BattleProjectileEffectBase _effect = null;

		[SerializeField]
		private float _destroyAfterUnitsTraveled = 30f;

		private BattlePlayer _owner = null;
		private bool _isFired = false;
		private float _totalDistanceTravelled = 0f;

		public void Fire(BattlePlayer owner)
		{
			if(_isFired)
			{
				return;
			}

			_isFired = true;
			_owner = owner;

			_orientationContainer.localScale = owner.GetOrientation(_orientationContainer.localScale);
		}

		protected void FixedUpdate()
		{
			if(_isFired)
			{
				Vector2 pos = _rigidBody.position;
				float distanceToTravel = Time.fixedDeltaTime * _speed;
				pos.x += _owner.GetOrientation(distanceToTravel);
				_totalDistanceTravelled += distanceToTravel;
				_rigidBody.MovePosition(pos);

				if(_totalDistanceTravelled >= _destroyAfterUnitsTraveled)
				{
					Destroy(gameObject);
				}
			}
		}

		protected void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.gameObject.TryGetComponent(out BattleUnit unit))
			{
				if(_detectionType.HasFlag(BattlePlayerDetectionType.Same) &&
					_owner.PlayerType == unit.Owner.PlayerType)
				{
					_effect.ApplyEffect(this, unit);
				}

				if(_detectionType.HasFlag(BattlePlayerDetectionType.Opposite) &&
					_owner.PlayerType != unit.Owner.PlayerType)
				{
					_effect.ApplyEffect(this, unit);
				}
			}
		}
	}
}