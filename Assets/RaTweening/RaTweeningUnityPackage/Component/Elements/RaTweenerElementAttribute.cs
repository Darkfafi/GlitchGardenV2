using System;

namespace RaTweening
{
	/// <summary>
	/// This attribute is used to bind a tween to a <see cref="RaTweening.Core.Elements.RaTweenerElementBase"/> which will represent their serialization.
	/// This should be placed above the class of a <see cref="RaTweenBase"/> inheritor.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class RaTweenerElementAttribute : Attribute
	{
		/// <summary>
		/// The Element Type which should represent the tagged tween
		/// </summary>		
		public readonly Type ElementType;

		/// <summary>
		/// The type of the element desired to represent this tween should be specified here
		/// </summary>
		/// <param name="type">The type of a <see cref="RaTweening.Core.Elements.RaTweenerElementBase"/>. Can be filled in as `typeof(RaTweenerSerializableElement)` for example</param>
		public RaTweenerElementAttribute(Type type)
		{
			ElementType = type;
		}
	}
}