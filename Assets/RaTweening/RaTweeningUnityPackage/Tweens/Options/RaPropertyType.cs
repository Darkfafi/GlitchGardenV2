namespace RaTweening
{
	/// <summary>
	/// Enum Representing Various Value Types which can be passed to a Material
	/// </summary>
	public enum RaPropertyType
	{
		/// <summary>
		/// Affects the given property by Name of target Material
		/// </summary>
		PropertyName = 0,
		/// <summary>
		/// Affects the given property by ID of target Material
		/// </summary>
		PropertyID = 1,
	}

	/// <summary>
	/// Enum Representing Various Value Types which can be passed to a Material
	/// > Note: This includes a Default value which would direct the default material value for the given tween
	/// </summary>
	public enum RaPropertyDefaultType
	{
		/// <summary>
		/// Affects the Default channel of target Material 
		/// > Note: For Example, for a Color tween this would be '_Color' 
		/// </summary>
		Default = 0,
		/// <summary>
		/// Affects the given property by Name of target Material
		/// </summary>
		PropertyName = 1,
		/// <summary>
		/// Affects the given property by ID of target Material
		/// </summary>
		PropertyID = 2,

	}
}