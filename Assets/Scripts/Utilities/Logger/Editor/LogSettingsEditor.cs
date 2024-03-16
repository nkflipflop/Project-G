using UnityEngine;
using UnityEditor;

namespace Utilities
{
	[CustomEditor(typeof(LogSettings))]
	public class LogSettingsEditor : UnityEditor.Editor
	{
		private LogSettings logSettings;
		private SerializedObject so;
		private bool showAboutInfo;

		private void OnEnable()
		{
			logSettings = (LogSettings)target;
			so = new SerializedObject(logSettings);

			logSettings.permissions ??= new bool[(int)Log.Platform.Count * (int)Log.Type.Count];
		}

	    public override void OnInspectorGUI()
	    {
	        so = new SerializedObject(logSettings);
		    
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
			EditorGUI.DropShadowLabel(headerRect, "Logger", headerStyle);
			GUI.backgroundColor = Color.white;

			// Platform/Log Type Permissions
			EditorGUILayout.Space(75);

			EditorGUI.BeginChangeCheck();
			
			for (int i = -1; i < (int)Log.Platform.Count; i++)
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
					EditorGUI.TextField(rect, ((Log.Platform)i).ToString(), style);
				}

				for (int j = 0; j < (int)Log.Type.Count; j++)
				{
					if (i == -1)
					{
						var style = new GUIStyle {
							alignment = TextAnchor.UpperLeft
						};

						style.normal.textColor = (Log.Type)j switch
						{
							Log.Type.Info => Color.green,
							Log.Type.Error => Color.red,
							Log.Type.Warning => Color.yellow,
							_ => style.normal.textColor
						};

						Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
						GUIUtility.RotateAroundPivot(-90, rect.position);
						EditorGUI.TextField(rect, ((Log.Type)j).ToString(), style);
						GUIUtility.RotateAroundPivot(90, rect.position);
					}
					else
					{
						Rect rect = GUILayoutUtility.GetRect(18, 18, "Toggle");
						logSettings.permissions[i * (int)Log.Type.Count + j] = EditorGUI.Toggle(rect,
							logSettings.permissions[i * (int)Log.Type.Count + j]);
					}
				}

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
			
			EditorGUILayout.Space(15);
			logSettings.priority = EditorGUILayout.IntSlider("Priority", logSettings.priority, 0, 10);
			
			// About Info
			EditorGUILayout.Space(15);
			showAboutInfo = EditorGUILayout.Foldout(showAboutInfo, "About the logger");

			if (showAboutInfo) 
			{
				EditorGUILayout.LabelField("Logger Version: ", "1.0");
				EditorGUILayout.LabelField("© 2021 Alexander, All Rights Reserved");
			}

			if (EditorGUI.EndChangeCheck()) 
			{
				EditorUtility.SetDirty(logSettings);
				so.ApplyModifiedProperties();
			}
	    }
	}
}
