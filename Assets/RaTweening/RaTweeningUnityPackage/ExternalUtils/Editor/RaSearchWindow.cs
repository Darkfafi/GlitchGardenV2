#if UNITY_EDITOR
using UnityEngine;

using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RaTweening
{
	internal class RaSearchWindow : EditorWindow
	{
		#region Consts

		private const string SearchbarControl = "SearchbarControl";

		#endregion

		#region Variables

		private bool InMultiSelectMode = false;

		private GUIStyle _toolbarStyle;
		private GUIStyle _toolbarSearchTextStyle;
		private GUIStyle _entryItemButtonStyle;

		private Action<int> _onSelectionCallback;
		private object[] _selectionItems;

		private Action<(int index, bool selected)[]> _onMultiSelectEndedCallback;
		private (object item, bool selected)[] _multiSelectionItems;
		private Dictionary<int, bool> _changedMultiSelectionItems = new Dictionary<int, bool>();

		private string _searchString = "";
		private Vector2 _scrollPosition = Vector2.zero;

		private bool _showIndex = true;

		#endregion

		#region Properties

		public bool IsOpen
		{
			get; private set;
		}

		#endregion

		#region Lifecycle

		private void OnGUI()
		{
			if(_toolbarStyle == null)
			{
				_toolbarStyle = new GUIStyle(GUI.skin.FindStyle("Toolbar"));
			}

			if(_toolbarSearchTextStyle == null)
			{
				_toolbarSearchTextStyle = new GUIStyle(GUI.skin.FindStyle("ToolbarSeachTextField"));
			}

			if(_entryItemButtonStyle == null)
			{
				_entryItemButtonStyle = new GUIStyle(GUI.skin.button)
				{
					stretchWidth = true,
					alignment = TextAnchor.MiddleLeft
				};
			}

			// SearchBar
			GUILayout.BeginHorizontal(_toolbarStyle);
			{
				GUI.SetNextControlName(SearchbarControl);
				_searchString = GUILayout.TextField(_searchString, _toolbarSearchTextStyle);
			}
			GUILayout.EndHorizontal();

			if(focusedWindow)
			{
				GUI.FocusControl(SearchbarControl);
			}

			if(InMultiSelectMode)
			{
				if(GUILayout.Button("Confirm Selection"))
				{
					_onMultiSelectEndedCallback?.Invoke(_changedMultiSelectionItems.Select((pair) => (pair.Key, pair.Value)).ToArray());
					_onMultiSelectEndedCallback = null;
					return;
				}

				EditorGUILayout.Space();
			}

			int selectedIndex = -1;
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true);
			{
				// Selection
				int showCount = 0;
				if(InMultiSelectMode)
				{
					for(int i = 0; i < _multiSelectionItems.Length; i++)
					{
						(object item, bool selected) = _multiSelectionItems[i];
						if(MatchesSearch(item, i))
						{
							if(!_changedMultiSelectionItems.TryGetValue(i, out bool currentlySelected))
							{
								currentlySelected = selected;
							}

							GUI.color = currentlySelected ? Color.green : Color.white;
							if(GUILayout.Button(GetNameForItem(i, item), _entryItemButtonStyle) || KeyboardSelection(showCount))
							{
								selectedIndex = i;
							}
							GUI.color = Color.white;
							showCount++;
						}
					}
				}
				else
				{
					for(int i = 0; i < _selectionItems.Length; i++)
					{
						if(MatchesSearch(_selectionItems[i], i))
						{
							if(GUILayout.Button(GetNameForItem(i, _selectionItems[i]), _entryItemButtonStyle) || KeyboardSelection(showCount))
							{
								selectedIndex = i;
							}

							showCount++;
						}
					}
				}
			}
			GUILayout.EndScrollView();

			if(selectedIndex >= 0)
			{
				if(InMultiSelectMode)
				{
					bool itemOriginalValue = _multiSelectionItems[selectedIndex].selected;
					if(_changedMultiSelectionItems.ContainsKey(selectedIndex))
					{
						_changedMultiSelectionItems.Remove(selectedIndex);
					}
					else
					{
						_changedMultiSelectionItems[selectedIndex] = !itemOriginalValue;
					}
				}
				else
				{
					Action<int> callback = _onSelectionCallback;
					_onSelectionCallback = null;
					callback(selectedIndex);
				}
			}
		}

		private void OnDestroy()
		{
			IsOpen = false;

			if(_onSelectionCallback != null)
			{
				_onSelectionCallback(-1);
				_onSelectionCallback = null;
			}

			if(_onMultiSelectEndedCallback != null)
			{
				_onMultiSelectEndedCallback(new (int, bool)[] { });
				_onMultiSelectEndedCallback = null;
			}

			_selectionItems = null;
			_multiSelectionItems = null;
		}

		#endregion

		#region Public Methods

		public static RaSearchWindow OpenWindow(Action<int> onSelectionCallback, params object[] itemsToSelectFrom)
		{
			return BaseOpenWindow(x => x.Setup(onSelectionCallback, itemsToSelectFrom));
		}

		public static RaSearchWindow OpenWindow(Action<(int, bool)[]> onMultiSelectEndedCallback, params (object, bool)[] itemsToSelectFrom)
		{
			return BaseOpenWindow(x => x.Setup(onMultiSelectEndedCallback, itemsToSelectFrom));
		}

		public void SetShowIndex(bool showIndex)
		{
			_showIndex = showIndex;
		}

		#endregion

		#region Private Methods

		private static RaSearchWindow BaseOpenWindow(Action<RaSearchWindow> setupRequest)
		{
			RaSearchWindow window = GetWindow<RaSearchWindow>();
			window.IsOpen = true;
			window.SetShowIndex(true);
			setupRequest(window);
			window.Show(true);
			window.Focus();
			window.Repaint();
			return window;
		}

		private void Setup(Action<int> onSelectionCallback, params object[] itemsToSelectFrom)
		{
			_scrollPosition = Vector2.zero;
			_onSelectionCallback = onSelectionCallback;
			_selectionItems = itemsToSelectFrom;
			InMultiSelectMode = false;
		}

		private void Setup(Action<(int, bool)[]> onMultiSelectEndedCallback, params (object, bool)[] itemsToSelectFrom)
		{
			_scrollPosition = Vector2.zero;
			_onMultiSelectEndedCallback = onMultiSelectEndedCallback;
			_multiSelectionItems = itemsToSelectFrom;
			_changedMultiSelectionItems = new Dictionary<int, bool>();
			InMultiSelectMode = true;
		}

		private string GetNameForItem(int index, object item)
		{
			if(_showIndex)
			{
				return string.Concat(index, ": ", item.ToString());
			}
			else
			{
				return item.ToString();
			}
		}

		private bool KeyboardSelection(int showIndex)
		{
			return Event.current.keyCode == KeyCode.Return && showIndex == 0 && focusedWindow;
		}

		private bool MatchesSearch(object obj, int index)
		{
			if(string.IsNullOrEmpty(_searchString) || index.ToString().IndexOf(_searchString) >= 0)
			{
				return true;
			}

			string[] parts = _searchString.Split(' ');

			for(int i = 0; i < parts.Length; i++)
			{
				if(!obj.ToString().ToLower().Contains(parts[i].ToLower()))
				{
					return false;
				}
			}

			return true;

		}

		#endregion
	}
}
#endif