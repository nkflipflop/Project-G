using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        [NonSerialized] public bool isLoadingAnimationActive;
        
        public async UniTask LoadScene(string scene)
        {
            isLoadingAnimationActive = true;
            Signals.Common.Signal.SetInputState?.Invoke(false);
        
            DOTween.KillAll();
            // ObjectPoolController.instance.ResetAllActiveObjects();

            await LoadAsyncScene(scene);
        }
        
        private async UniTask LoadAsyncScene(string scene)
        {
            AsyncOperation loadScene = SceneManager.LoadSceneAsync(scene);
            loadScene.allowSceneActivation = false;
            await UniTask.WaitUntil(IsLoadOperationCompleted);
            loadScene.allowSceneActivation = true;
            
            await UniTask.WaitUntil(() => loadScene.isDone);

            isLoadingAnimationActive = false;
            
            bool IsLoadOperationCompleted()
            {
                return loadScene.progress >= 0.9f && Time.timeSinceLevelLoad > 1f;
            }
        }
    }
}