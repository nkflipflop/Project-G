using UnityEngine;
using UnityEngine.EventSystems;

namespace General
{
    public class GameUIEventSystem : MonoBehaviour
    {
        public EventSystem eventSystem;

        private void SetEventSystemState(bool isActive)
        {
            eventSystem.enabled = isActive;
        }

        private void OnEnable()
        {
            Signals.Common.Signal.SetInputState += SetEventSystemState;
        }
    
        private void OnDisable()
        {
            Signals.Common.Signal.SetInputState -= SetEventSystemState;
        }
    }
}