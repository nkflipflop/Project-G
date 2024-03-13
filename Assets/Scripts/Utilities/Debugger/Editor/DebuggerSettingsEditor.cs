using UnityEngine;
using UnityEditor;

namespace Utilities
{
	[CustomEditor(typeof(DebuggerSettings))]
	public class DebuggerSettingsEditor : Editor
	{
		private DebuggerSettings debuggerSettings;
		private SerializedObject so;
		private bool showAboutInfo;

		private void OnEnable()
		{
			debuggerSettings = (DebuggerSettings)target;
			so = new SerializedObject(debuggerSettings);

			debuggerSettings.permissions ??= new bool[(int)Debugger.Platform.Count * (int)Debugger.LogType.Count];
		}

	    public override void OnInspectorGUI()
	    {
	        so = new SerializedObject(debuggerSettings);
		    
			// Header
			Rect headerRect = GUILayoutUtility.GetRect(300, 30, "TextField");
			var headerStyle = new GUIStyle {
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
				fontSize = 16,
				normal = {
					textColor = Color.cyan
				}
			};

			GUI.backgroundColor = Color.blue;
			EditorGUI.DropShadowLabel(headerRect, "Debugger", headerStyle);
			GUI.backgroundColor = Color.white;

			// Platform/Log Type Permissions
			EditorGUILayout.Space(75);

			EditorGUI.BeginChangeCheck();
			
			for (int i = -1; i < (int)Debugger.Platform.Count; i++)
			{
				EditorGUILayout.BeginVertical();
				EditorGUILayout.BeginHorizontal();

				if (i == -1)
				{
					Rect rect = GUILayoutUtility.GetRect(50, 18, "TextField");
					EditorGUI.DropShadowLabel(rect, "");
				}
				else
				{
					var style = new GUIStyle {
						alignment = TextAnchor.MiddleRight,
						padding = new RectOffset(0, 15, 0, 0),
						normal = {
							textColor = Color.white
						}
					};
					var rect = GUILayoutUtility.GetRect(50, 18, "TextField");
					EditorGUI.TextField(rect, ((Debugger.Platform)i).ToString(), style);
				}

				for (int j = 0; j < (int)Debugger.LogType.Count; j++)
				{
					if (i == -1)
					{
						var style = new GUIStyle {
							alignment = TextAnchor.UpperLeft
						};

						style.normal.textColor = (Debugger.LogType)j switch
						{
							Debugger.LogType.Info => Color.green,
							Debugger.LogType.Error => Color.red,
							Debugger.LogType.Warning => Color.yellow,
							_ => style.normal.textColor
						};

						Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
						GUIUtility.RotateAroundPivot(-90, rect.position);
						EditorGUI.TextField(rect, ((Debugger.LogType)j).ToString(), style);
						GUIUtility.RotateAroundPivot(90, rect.position);
					}
					else
					{
						Debug.LogError(i * (int)Debugger.LogType.Count + j);
						Rect rect = GUILayoutUtility.GetRect(18, 18, "Toggle");
						debuggerSettings.permissions[i * (int)Debugger.LogType.Count + j] = EditorGUI.Toggle(rect,
							debuggerSettings.permissions[i * (int)Debugger.LogType.Count + j]);
					}
				}

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
			
			EditorGUILayout.Space(15);
			debuggerSettings.priority = EditorGUILayout.IntSlider("Priority", debuggerSettings.priority, 0, 10);
			
			// About Info
			EditorGUILayout.Space(15);
			showAboutInfo = EditorGUILayout.Foldout(showAboutInfo, "About the debugger");

			if (showAboutInfo) 
			{
				EditorGUILayout.LabelField("Debugger Version: ", "1.0");
				EditorGUILayout.LabelField("© 2021 Alexander, All Rights Reserved");
			}

			if (EditorGUI.EndChangeCheck()) 
			{
				EditorUtility.SetDirty(debuggerSettings);
				so.ApplyModifiedProperties();
			}
	    }
	}
}
