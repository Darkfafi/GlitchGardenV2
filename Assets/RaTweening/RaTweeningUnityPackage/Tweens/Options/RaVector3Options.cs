using System;
using UnityEngine;

namespace RaTweening
{
	/// <summary>
	/// This Util class hold the tools to exclude various axes in 3D space from tweening calculations
	/// </summary>
	public static class RaVector3Options
	{
		#region Public Methods

		/// <summary>
		/// Returns a <see cref="Axis"/> which only contains the Axes which are not defined within the input
		/// </summary>
		/// <param name="inclAxes">Input which Axes not to define</param>
		/// <returns>The value of all Axes excluding the input</returns>
		public static Axis GetOnlyIncludeAxes(Axis inclAxes)
		{
			Axis returnValue = Axis.None;

			if(!inclAxes.HasFlag(Axis.X))
			{
				returnValue |= Axis.X;
			}

			if(!inclAxes.HasFlag(Axis.Y))
			{
				returnValue |= Axis.Y;
			}

			if(!inclAxes.HasFlag(Axis.Z))
			{
				returnValue |= Axis.Z;
			}

			return returnValue;
		}

		/// <summary>
		/// Resets the final result to the original Vector based on which Axes are to be excluded from the final calculation
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="final">The final value</param>
		/// <param name="excludeAxes">The axes which should be reset to the original value within the final value</param>
		/// <returns>The new final value</returns>
		public static Vector3 ApplyExcludeAxes(Vector3 original, Vector3 final, Axis excludeAxes)
		{
			if(excludeAxes.HasFlag(Axis.X))
			{
				final.x = original.x;
			}

			if(excludeAxes.HasFlag(Axis.Y))
			{
				final.y = original.y;
			}

			if(excludeAxes.HasFlag(Axis.Z))
			{
				final.z = original.z;
			}

			return final;
		}

		#endregion


		#region Nested

		/// <summary>
		/// Flag based enum representing 3D Axis
		/// </summary>
		[Flags]
		public enum Axis
		{
			None = 0,
			/// <summary>
			/// The X Axis in 3D Space
			/// </summary>
			X = 1,
			/// <summary>
			/// The Y Axis in 3D Space
			/// </summary>
			Y = 2,
			/// <summary>
			/// The Z Axis in 3D Space
			/// </summary>
			Z = 4,
			All = X | Y | Z
		}

		#endregion
	}
}