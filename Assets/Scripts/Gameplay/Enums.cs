using System;

namespace Gameplay
{
	[Flags]
	public enum GameStatus
	{
		Starting = 1 << 0,
		Playing = 1 << 1,
		Pause = 1 << 2,
		WaitingForFinish = 1 << 3,
		Finished = 1 << 4,
		Fail = 1 << 5,
		Success = 1 << 6,
		Menu = 1 << 7
	}
}