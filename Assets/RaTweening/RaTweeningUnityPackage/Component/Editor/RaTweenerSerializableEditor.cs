#if UNITY_EDITOR
using UnityEditor;

namespace RaTweening.Core.Elements
{
	[CustomEditor(typeof(RaTweenerSerializableElement))]
	internal class RaTweenerSerializableEditor : Editor
	{
		#region Variables

		private RaTweenElementEditorDrawer _drawer;

		#endregion

		#region Lifecycle

		protected void OnEnable()
		{
			try
			{
				_drawer = new RaTweenElementEditorDrawer(serializedObject);
			}
			catch { }
		}

		#endregion

		#region Public Methods

		public override void OnInspectorGUI()
		{
			try
			{
				_drawer.Draw(()=> 
				{
					RaTweenerComponentEditor.DrawDefaultInspectorWithoutScript(serializedObject);
				});
			}
			catch { };
		}

		#endregion
	}
}
#endif