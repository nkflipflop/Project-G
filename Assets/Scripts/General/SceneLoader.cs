using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pooling;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        public enum LoadingStep
        {
            None,
            Started,
            LoadingPanelActivated,
            LoadingOperationCompleted,
            NextSceneActivated,
            Finished
        }
        
        [NonSerialized] public bool isLoadingAnimationActive;
        
        public async UniTask LoadScene(int sceneIndex)
        {
            Signals.Common.Signal.OnSceneLoadStepCompleted?.Invoke(sceneIndex, LoadingStep.Started);
            isLoadingAnimationActive = true;
            Signals.Common.Signal.SetInputState?.Invoke(false);
        
            DOTween.KillAll();
            PoolFactory.instance.ResetAll();

            await LoadAsyncScene(sceneIndex);
            Signals.Common.Signal.OnSceneLoadStepCompleted?.Invoke(sceneIndex, LoadingStep.Finished);
        }
        
        private async UniTask LoadAsyncScene(int sceneIndex)
        {
            AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneIndex);
            loadScene.allowSceneActivation = false;
            await UniTask.WaitUntil(IsLoadOperationCompleted);
            Signals.Common.Signal.OnSceneLoadStepCompleted?.Invoke(sceneIndex, LoadingStep.LoadingOperationCompleted);
            loadScene.allowSceneActivation = true;
            
            await UniTask.WaitUntil(() => loadScene.isDone);
            
            Signals.Common.Signal.OnSceneLoadStepCompleted?.Invoke(sceneIndex, LoadingStep.NextSceneActivated);

            isLoadingAnimationActive = false;
            
            bool IsLoadOperationCompleted()
            {
                return loadScene.progress >= 0.9f && Time.timeSinceLevelLoad > 1f;
            }
        }
    }
}