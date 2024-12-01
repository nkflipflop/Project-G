using System;
using System.Reflection;
using Gameplay;
using General;

namespace Signals
{
    public static class SignalManager
    {
        public static void Reset()
        {
            Reset(typeof(Common.Signal));
            
            void Reset(Type type)
            {
                FieldInfo[] values = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance |
                                                    BindingFlags.Public | BindingFlags.Static);

                foreach (FieldInfo value in values)
                {
                    value.SetValue(value, null);
                }
            }
        }
    }
    
    namespace Common
    {
        public static class Signal
        {
            #region CommandManager
#if UNITY_EDITOR
            public static Action OnCommandManagerUpdated;
#endif
            public static Action OnAllCommandsCompleted;
            #endregion
            
            #region Common
            public static Action<int, SceneLoader.LoadingStep> OnSceneLoadStepCompleted;
            #endregion
            
            #region GameUIEventSystem
            public static Action<bool> SetInputState;
            #endregion
        }    
    }

    namespace Gameplay
    {
        public static class Signal
        {
            #region GameManager
            public static Action OnLevelStarted = delegate { };
            public static Action OnLevelFinished = delegate { };
            public static Action<GameStatus> OnGameStatusChanged = delegate { };
            #endregion
        }
    }
}