using UnityEngine;

namespace Utilities
{
	[CreateAssetMenu(fileName = "DebuggerSettings", menuName = "Utilities/Debugger Settings", order = 1)]
	public class DebuggerSettings : ScriptableObject
	{
		[HideInInspector] public bool[] permissions =
		{
			true, true, true,
			false, true, true,
			false, true, true,
			false, true, true
		};
		[HideInInspector] public int priority;
	}
}
