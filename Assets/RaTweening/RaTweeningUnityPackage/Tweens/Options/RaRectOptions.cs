using System;
using UnityEngine;

namespace RaTweening
{
	/// <summary>
	/// This Util class hold the tools to exclude various Values of a Rect from tweening calculations
	/// </summary>
	public static class RaRectOptions
	{
		#region Public Methods

		/// <summary>
		/// Returns a <see cref="RectValue"/> which only contains the Rect Values which are not defined within the input
		/// </summary>
		/// <param name="inclValues">Input which Rect Values not to define</param>
		/// <returns>The value of all Rect Values excluding the input</returns>
		public static RectValue GetOnlyIncludeRectValues(RectValue inclValues)
		{
			RectValue returnValue = RectValue.None;

			if(!inclValues.HasFlag(RectValue.X))
			{
				returnValue |= RectValue.X;
			}

			if(!inclValues.HasFlag(RectValue.Y))
			{
				returnValue |= RectValue.Y;
			}

			if(!inclValues.HasFlag(RectValue.Width))
			{
				returnValue |= RectValue.Width;
			}

			if(!inclValues.HasFlag(RectValue.Height))
			{
				returnValue |= RectValue.Height;
			}

			return returnValue;
		}

		/// <summary>
		/// Resets the final result to the original Rect based on which Rect Values are to be excluded from the final calculation
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="final">The final value</param>
		/// <param name="excludeValues">The rect values which should be reset to the original value within the final value</param>
		/// <returns>The new final value</returns>
		public static Rect ApplyExcludeRectValues(Rect original, Rect final, RectValue excludeValues)
		{
			if(excludeValues.HasFlag(RectValue.X))
			{
				final.x = original.x;
			}

			if(excludeValues.HasFlag(RectValue.Y))
			{
				final.y = original.y;
			}

			if(excludeValues.HasFlag(RectValue.Width))
			{
				final.width = original.width;
			}

			if(excludeValues.HasFlag(RectValue.Height))
			{
				final.height = original.height;
			}

			return final;
		}

		#endregion


		#region Nested

		/// <summary>
		/// Flag based enum representing Rect Values
		/// </summary>
		[Flags]
		public enum RectValue
		{
			None = 0,
			/// <summary>
			/// The X Axis of a Rect
			/// </summary>
			X = 1,
			/// <summary>
			/// The Y Axis of a Rect
			/// </summary>
			Y = 2,
			/// <summary>
			/// The Width of a Rect
			/// </summary>
			Width = 4,
			/// <summary>
			/// The Height of a Rect
			/// </summary>
			Height = 8,

			All = X | Y | Width | Height
		}

		#endregion
	}
}