using UnityEngine;

namespace RaTweening.Core
{
	/// <summary>
	/// Util class that handles the logics of <see cref="RaModifierType"/>
	/// </summary>
	public static class RaTweenModifier
	{
		/// <summary>
		/// Applies the ModifierType to the given value and returns the modified variant of that value
		/// </summary>
		/// <param name="modifierType">The ModifierType to apply to the given value</param>
		/// <param name="x">Representing the value to modify, mainly used to specify the normalized time within the tween</param>
		/// <returns>The modified variant of x</returns>
		public static float ApplyModifier(RaModifierType modifierType, float x)
		{
			switch(modifierType)
			{
				// Modifiers
				case RaModifierType.AbsSin:
					return Mathf.Abs(Mathf.Sin(x * Mathf.PI));

				case RaModifierType.Yoyo:
					if(x <= 0.5f)
					{
						x *= 2f;
					}
					else
					{
						x *= -2f;
						x += 2f;
					}
					return x;

				case RaModifierType.Reverse:
					return 1f - x;

				// Default
				case RaModifierType.None:
					return x;

				// Not Implemented Exception
				default:
					throw new System.NotImplementedException($"{nameof(RaModifierType)} {modifierType} not implemented");
			}
		}
	}
}