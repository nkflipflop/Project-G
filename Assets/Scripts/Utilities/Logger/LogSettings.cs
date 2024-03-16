using UnityEngine;

namespace Utilities
{
	[CreateAssetMenu(fileName = "LogSettings", menuName = "Utilities/Log Settings", order = 1)]
	public class LogSettings : ScriptableObject
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
