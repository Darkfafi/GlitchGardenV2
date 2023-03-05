using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField]
	private float _speed = 5;

	[SerializeField]
	private Transform _visualContainer = null;

	[SerializeField]
	private Rigidbody2D _rigidBody = null;

	private Player _owner = null;
	private bool _isFired = false;

	public void Fire(Player owner)
	{
		if(_isFired)
		{
			return;
		}

		_isFired = true;
		_owner = owner;

		// Direction
		Vector3 scale = _visualContainer.transform.localScale;
		scale.x = ApplyOwnerScale(scale.x);
		_visualContainer.transform.localScale = scale;
	}

	protected void Update()
	{
		if(_isFired)
		{
			Vector2 pos = _rigidBody.position;
			pos.x += ApplyOwnerScale(Time.deltaTime * _speed);
			_rigidBody.MovePosition(pos);
		}
	}

	private float ApplyOwnerScale(float value)
	{
		if(_owner.PlayerType == Player.Type.Away)
		{
			value *= -1;
		}
		return value;
	}
}
