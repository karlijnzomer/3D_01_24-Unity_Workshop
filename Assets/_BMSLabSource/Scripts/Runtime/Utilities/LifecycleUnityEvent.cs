using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.Utilities
{
    public class LifecycleUnityEvent : MonoBehaviour
    {
        public UnityEvent Enabled, Awoke, Started, Disabled;

        private void OnEnable()
        {
            Enabled?.Invoke();
        }

        private void Awake()
        {
            Awoke?.Invoke();
        }

        private void Start()
        {
            Started?.Invoke();
        }

        private void OnDisable()
        {
            Disabled?.Invoke();
        }
    }
}
