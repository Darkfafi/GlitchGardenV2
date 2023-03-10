using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField]
	private float _speed = 5;

	[SerializeField]
	private Transform _orientationContainer = null;

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

		_orientationContainer.localScale = owner.GetOrientation(_orientationContainer.localScale);
	}

	protected void Update()
	{
		if(_isFired)
		{
			Vector2 pos = _rigidBody.position;
			pos.x += _owner.GetOrientation(Time.deltaTime * _speed);
			_rigidBody.MovePosition(pos);
		}
	}
}
