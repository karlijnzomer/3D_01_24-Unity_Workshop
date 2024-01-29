using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.Utilities
{
    public class UtilityBase : MonoBehaviour
    {
        [SerializeField]
        protected bool _active = false;
        [SerializeField]
        private bool _activateOnStart = true;
        [SerializeField]
        protected Transform _target;

        public UnityEvent Activated, Deactivated;

        private void Awake()
        {
            if (_activateOnStart)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        public void Activate()
        {
            enabled = true;
            _active = true;

            Activated?.Invoke();
        }

        public void Deactivate()
        {
            enabled = false;
            _active = false;

            Deactivated?.Invoke();
        }

        public void SetTarget(Transform newTarget)
        {
            _target = newTarget;
        }

    }
}
