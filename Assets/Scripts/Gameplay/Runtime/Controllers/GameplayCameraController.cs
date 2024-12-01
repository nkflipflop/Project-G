using Cinemachine;
using Controllers;

namespace Gameplay.Runtime.Controllers
{
	public class GameplayCameraController : CameraControllerBase
	{
		public CinemachineVirtualCamera cinemachineVirtualCamera;

		public override void Start()
		{
			base.Start();
			cinemachineVirtualCamera.Follow = cinemachineVirtualCamera.LookAt = GameManager.instance.Player.transform;
		}
	}
}