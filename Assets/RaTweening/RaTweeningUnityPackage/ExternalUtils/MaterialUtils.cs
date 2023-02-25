using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaTweening.Tools
{
	public static class MaterialUtils
	{
		public static Material GetRendererMaterial(Renderer target, bool sharedMaterial)
		{
#if UNITY_EDITOR
			if(!Application.isPlaying &&
				!sharedMaterial)
			{
				return target.sharedMaterial;
			}
#endif

			if(sharedMaterial)
			{
				return target.sharedMaterial;
			}

			return target.material;
		}
	}
}