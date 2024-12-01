using DG.Tweening;
using Gameplay.Runtime.Controllers;
using UnityEngine;

namespace Controllers
{
	public abstract class CameraControllerBase : MonoBehaviour
    {
        public Camera cam;
        protected bool isInitialized;
        [SerializeField] private bool selfInitialize;
        [SerializeField] private bool resizable; 
        
        private Vector3 targetPosition;
        private Vector3 cachedPosition;
        private float screenRatioToDefault;
        
        public virtual void Start()
        {
            if (selfInitialize)
            {
                Initialize();
            }
        }

        public virtual void Initialize()
        {
            GameManager.instance.SetCameraController(this);
        }
        
        public void SetCameraTargetPosition(Vector2 targetPosition)
        {
            this.targetPosition = targetPosition;
            this.targetPosition.z = -10;
        }
        
        public Vector3 GetCameraTopPosition()
        {
            return cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height, 10));
        }

        public Vector3 GetCameraBottomPosition()
        {

            return cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0, 10));
        }

        public Vector3 GetCameraRightPosition()
        {

            return cam.ScreenToWorldPoint(new Vector3(0, Screen.height / 2f, 10));
        }

        public Vector3 GetCameraLeftPosition()
        {
            return cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, 10));
        }
        
        public void Shake(Vector3 amount,int? loopCount=null,LoopType loopType= LoopType.Yoyo,int vibrato=20)
        {
            if (!DOTween.IsTweening(transform))
            {
                cachedPosition = transform.position;
                if (loopCount.HasValue)
                {
                    transform.DOShakePosition(0.2f, amount, vibrato).SetLoops(loopCount.Value,loopType).SetId("0");
                }
                else
                {
                    transform.DOShakePosition(0.2f, amount, vibrato);    
                }
            }
        }
        
        public void StopShakeAnimation()
        {
            transform.DOKill();
            transform.position = cachedPosition;
        }
    }
}