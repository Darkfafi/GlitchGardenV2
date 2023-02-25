using System;
using UnityEngine;

namespace RaTweening.Core.Elements
{
	/// <summary>
	/// The element representing <see cref="RaTweenBase"/> tweens within the <see cref="RaTweenerComponent"/>
	/// This covers all of the tweens which choose it as the represenative element which are also able to be serialzied and have an empty constructor option.
	/// > Note: All <see cref="RaTween"/> inheritors use this element to be serialized in the editor
	/// </summary>
	public class RaTweenerSerializableElement : RaTweenerElementBase
	{
		#region Editor Variables

		[Header("RaTweenerElement")]
		[SerializeField, SerializeReference]
		private RaTweenBase _tween = null;

		#endregion

		#region Protected Methods

		protected override void Init(Type tweenType)
		{
			_tween = (RaTweenBase)Activator.CreateInstance(tweenType);

			// Dynamic Tween Auto Targeting
			if(_tween is IRaTweenTarget targetTween)
			{
				Type type = targetTween.GetTargetTypeRaw();

				if(IsOfType<GameObject>(type))
				{
					targetTween.SetTargetRaw(gameObject);
				}
				else if(IsOfType<Component>(type) && gameObject.TryGetComponent(type, out Component component))
				{
					targetTween.SetTargetRaw(component);
				}
			}

			CalculateDefaultValues();
		}

		protected override RaTweenBase CreateTweenCore()
		{
			return _tween.Clone(false);
		}

		protected override string GetElementName()
		{
			return _tween == null ? nameof(RaTweenerSerializableElement) : _tween.GetType().Name;
		}

		protected override void CalculateDefaultValues()
		{
			_tween?.SetDefaultValuesInternal();
		}

		#endregion

		#region Internal Methods

		internal RaTweenBase GetTweenInternal()
		{
			return _tween;
		}

		#endregion

		#region Private Methods

		private bool IsOfType<T>(Type t)
		{
			return typeof(T).IsAssignableFrom(t);
		}

		#endregion
	}
}