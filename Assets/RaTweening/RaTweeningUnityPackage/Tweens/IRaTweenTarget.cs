using System;

namespace RaTweening
{
	public interface IRaTweenTarget
	{
		Type GetTargetTypeRaw();
		void SetTargetRaw(object value);

	}
}