using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Utilities
{
	public static class Debugger
	{
		public enum Platform
		{
			UnityEditor, Standalone, Android, iOS,
			Count
		}

		public enum LogType
		{
			Info, Error, Warning,
			Count
		}

		public enum TextFormat
		{
			Classic, Bold, Italic
		}

		private static bool isInitialized;
		private static DebuggerSettings debuggerSettings;

		/// <summary>
		/// Reads and assigns the debugger settings from Resources folder.
		/// </summary>
		private static void Initialize()
		{
			debuggerSettings = Resources.Load<DebuggerSettings>("LSDebuggerSettings");
			isInitialized = true;
		}

		/// <summary>
		/// Clears the Console window in the editor.
		/// </summary>
		public static void ClearLogs()
		{
#if UNITY_EDITOR			
			var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
			Type type = assembly.GetType("UnityEditor.LogEntries");
			MethodInfo method = type.GetMethod("Clear");
			method?.Invoke(new object(), null);
#endif			
		}

		#region Log Info Functions
		/// <summary>
		/// Logs a message to the console with optional settings.
		/// </summary>
		/// <param name="message">Log message.</param>
		/// <param name="obj">Related gameObject. (Optional)</param>
		/// <param name="color">Color of the message text. (Optional)</param>
		/// <param name="format">Text format. (Optional)</param>
		/// <param name="size">Size of the message text. (Optional)</param>
		/// <param name="priority">Priority of the log message. (Optional)</param>
		/// <param name="pause">Pauses the editor after logging the message. (Optional)</param>
		public static void Log(object message, GameObject obj = null, Color? color = null,
			TextFormat format = TextFormat.Classic, int size = 12, int priority = 0, bool pause = false)
		{
			Log((message, null, null, null, null, null, null), " ", obj, color, format, size, priority,
				pause);
		}

		public static void Log(ValueTuple<object, object> messages, string separator = " ", GameObject obj = null,
			Color? color = null, TextFormat format = TextFormat.Classic, int size = 12, int priority = 0,
			bool pause = false)
		{
			Log((messages.Item1, messages.Item2, null, null, null, null, null),
				separator, obj, color, format, size, priority, pause);
		}

		public static void Log(ValueTuple<object, object, object> messages, string separator = " ",
			GameObject obj = null,
			Color? color = null, TextFormat format = TextFormat.Classic, int size = 12, int priority = 0,
			bool pause = false)
		{
			Log((messages.Item1, messages.Item2, messages.Item3, null, null, null, null),
				separator, obj, color, format, size, priority, pause);
		}

		public static void Log(ValueTuple<object, object, object, object> messages, string separator = " ",
			GameObject obj = null,
			Color? color = null, TextFormat format = TextFormat.Classic, int size = 12, int priority = 0,
			bool pause = false)
		{
			Log((messages.Item1, messages.Item2, messages.Item3, messages.Item4, null, null, null),
				separator, obj, color, format, size, priority, pause);
		}

		public static void Log(ValueTuple<object, object, object, object, object> messages, string separator = " ",
			GameObject obj = null,
			Color? color = null, TextFormat format = TextFormat.Classic, int size = 12, int priority = 0,
			bool pause = false)
		{
			Log((messages.Item1, messages.Item2, messages.Item3, messages.Item4, messages.Item5, null, null),
				separator, obj, color, format, size, priority, pause);
		}

		public static void Log(ValueTuple<object, object, object, object, object, object> messages,
			string separator = " ", GameObject obj = null,
			Color? color = null, TextFormat format = TextFormat.Classic, int size = 12, int priority = 0,
			bool pause = false)
		{
			Log((messages.Item1, messages.Item2, messages.Item3, messages.Item4, messages.Item5,
					messages.Item6, null),
				separator, obj, color, format, size, priority, pause);
		}

		public static void Log(ValueTuple<object, object, object, object, object, object, object> messages,
			string separator = " ", GameObject obj = null,
			Color? color = null, TextFormat format = TextFormat.Classic, int size = 12, int priority = 0,
			bool pause = false)
		{
			if (!CheckLogPermission(LogType.Info, priority))
			{
				return;
			}

			Debug.Log(BuildLogMessage(messages, separator, color, format, size), obj);

			if (pause)
			{
				Debug.Break();
			}
		}
		#endregion

		#region Log Error Functions
		/// <summary>
		/// Logs an error message to the console with optional settings.
		/// </summary>
		/// <param name="message">Log message.</param>
		/// <param name="obj">Related gameObject. (Optional)</param>
		/// <param name="pause">Pauses the editor after logging the message. (Optional)</param>
		public static void LogError(object message, GameObject obj = null, bool pause = false)
		{
			LogError((message, null, null, null, null, null, null), " ", obj, pause);
		}

		public static void LogError(ValueTuple<object, object> messages, string separator = " ", GameObject obj = null, 
			bool pause = false)
		{
			LogError((messages.Item1, messages.Item2, null, null, null, null, null),
				separator, obj, pause);
		}

		public static void LogError(ValueTuple<object, object, object> messages, string separator = " ",
			GameObject obj = null, bool pause = false)
		{
			LogError((messages.Item1, messages.Item2, messages.Item3, null, null, null, null),
				separator, obj, pause);
		}

		public static void LogError(ValueTuple<object, object, object, object> messages, string separator = " ",
			GameObject obj = null, bool pause = false)
		{
			LogError((messages.Item1, messages.Item2, messages.Item3, messages.Item4, null, null, null),
				separator, obj, pause);
		}

		public static void LogError(ValueTuple<object, object, object, object, object> messages, string separator = " ",
			GameObject obj = null, bool pause = false)
		{
			LogError((messages.Item1, messages.Item2, messages.Item3, messages.Item4, messages.Item5, null, null),
				separator, obj, pause);
		}

		public static void LogError(ValueTuple<object, object, object, object, object, object> messages,
			string separator = " ", GameObject obj = null, bool pause = false)
		{
			LogError((messages.Item1, messages.Item2, messages.Item3, messages.Item4, messages.Item5,
					messages.Item6, null), separator, obj, pause);
		}

		public static void LogError(ValueTuple<object, object, object, object, object, object, object> messages,
			string separator = " ", GameObject obj = null, bool pause = false)
		{
			if (!CheckLogPermission(LogType.Error, 10))
			{
				return;
			}

			Debug.LogError(BuildLogMessage(messages, separator), obj);

			if (pause)
			{
				Debug.Break();
			}
		}
		#endregion

		#region Log Warning Functions
		/// <summary>
		/// Logs a warning message to the console with optional settings.
		/// </summary>
		/// <param name="message">Log message.</param>
		/// <param name="obj">Related gameObject. (Optional)</param>
		/// <param name="pause">Pauses the editor after logging the message. (Optional)</param>
		public static void LogWarning(object message, GameObject obj = null, bool pause = false)
		{
			LogWarning((message, null, null, null, null, null, null), " ", obj, pause);
		}

		public static void LogWarning(ValueTuple<object, object> messages, string separator = " ",
			GameObject obj = null, bool pause = false)
		{
			LogWarning((messages.Item1, messages.Item2, null, null, null, null, null),
				separator, obj, pause);
		}

		public static void LogWarning(ValueTuple<object, object, object> messages, string separator = " ",
			GameObject obj = null, bool pause = false)
		{
			LogWarning((messages.Item1, messages.Item2, messages.Item3, null, null, null, null),
				separator, obj, pause);
		}

		public static void LogWarning(ValueTuple<object, object, object, object> messages, string separator = " ",
			GameObject obj = null, bool pause = false)
		{
			LogWarning((messages.Item1, messages.Item2, messages.Item3, messages.Item4, null, null, null),
				separator, obj, pause);
		}

		public static void LogWarning(ValueTuple<object, object, object, object, object> messages,
			string separator = " ", GameObject obj = null, bool pause = false)
		{
			LogWarning((messages.Item1, messages.Item2, messages.Item3, messages.Item4, messages.Item5, null, null),
				separator, obj, pause);
		}

		public static void LogWarning(ValueTuple<object, object, object, object, object, object> messages,
			string separator = " ", GameObject obj = null, bool pause = false)
		{
			LogWarning((messages.Item1, messages.Item2, messages.Item3, messages.Item4, messages.Item5,
					messages.Item6, null), separator, obj, pause);
		}

		public static void LogWarning(ValueTuple<object, object, object, object, object, object, object> messages,
			string separator = " ", GameObject obj = null, bool pause = false)
		{
			if (!CheckLogPermission(LogType.Warning, 10))
			{
				return;
			}

			Debug.LogWarning(BuildLogMessage(messages, separator), obj);

			if (pause)
			{
				Debug.Break();
			}
		}
		#endregion

		#region Helper Functions
		/// <summary>
		/// Checks whether or not the given message can be logged.
		/// </summary>
		/// <param name="type">Type of the log message.</param>
		/// <param name="priority">Priority value of the message.</param>
		/// <returns>Returns a boolean value by checking the given type and priority value.</returns>
		private static bool CheckLogPermission(LogType type, int priority)
		{
#if DEBUG_MODE
			return true;
#endif
			CheckDebuggerInitialized();
			
			if (debuggerSettings == null || debuggerSettings.permissions == null)
			{
				Debug.LogError(
					"LSDebugger hasn't been initialized. You need to call Initialize() function at the start of the application.");
				return false;
			}
			if (type == LogType.Info && priority < debuggerSettings.priority)
			{
				return false;
			}

#if UNITY_EDITOR
			int platformIndex = 0;
#elif UNITY_STANDALONE
			int platformIndex = 1;
#elif UNITY_ANDROID
			int platformIndex = 2;
#elif UNITY_IOS
			int platformIndex = 3;
#endif

			return debuggerSettings.permissions[platformIndex * (int) LogType.Count + (int) type];
		}

		/// <summary>
		/// Checks whether or not the debugger is initialized. If not, then initializes it.
		/// </summary>
		private static void CheckDebuggerInitialized()
		{
			if (!isInitialized)
			{
				Initialize();
			}
		}

		/// <summary>
		/// Builds the final log message by using the given settings.
		/// </summary>
		private static string BuildLogMessage(
			ValueTuple<object, object, object, object, object, object, object> messages, string separator,
			Color? color = null, TextFormat format = TextFormat.Classic, int size = 12)
		{
			StringBuilder text = new StringBuilder("");

			ConcatenateMessages(ref text, messages, separator);
			if (size != 12)
			{
				SetSize(ref text, size);	
			}
			if (format != TextFormat.Classic)
			{
				SetFormat(ref text, format);	
			}
			if (color != null)
			{
				SetColor(ref text, (Color)color);	
			}
			return text.ToString();
		}

		private static void ConcatenateMessages(ref StringBuilder text,
			ValueTuple<object, object, object, object, object, object, object> messages, string separator)
		{
			if (messages.Item1 != null)
			{
				text.Append(messages.Item1);
			}
			if (messages.Item2 != null)
			{
				text.Append(separator);
				text.Append(messages.Item2);
			}
			if (messages.Item3 != null)
			{
				text.Append(separator);
				text.Append(messages.Item3);
			}
			if (messages.Item4 != null)
			{
				text.Append(separator);
				text.Append(messages.Item4);
			}
			if (messages.Item5 != null)
			{
				text.Append(separator);
				text.Append(messages.Item5);
			}
			if (messages.Item6 != null)
			{
				text.Append(separator);
				text.Append(messages.Item6);
			}
			if (messages.Item7 != null)
			{
				text.Append(messages.Item7);
			}
		}

		private static void SetColor(ref StringBuilder text, Color color)
		{
			text.Insert(0, "<color=#");
			text.Insert(8, ColorUtility.ToHtmlStringRGB(color));
			text.Insert(14, ">");
			text.Append("</color>");
		}

		private static void SetFormat(ref StringBuilder text, TextFormat format)
		{
			switch (format)
			{
				case TextFormat.Bold:
					text.Insert(0, "<b>");
					text.Append("</b>");
					break;
				case TextFormat.Italic:
					text.Insert(0, "<i>");
					text.Append("</i>");
					break;
			}
		}

		private static void SetSize(ref StringBuilder text, int size)
		{
			text.Insert(0, "<size=");
			text.Insert(6, size + ">");
			text.Append("</size>");
		}
		#endregion
	}
}