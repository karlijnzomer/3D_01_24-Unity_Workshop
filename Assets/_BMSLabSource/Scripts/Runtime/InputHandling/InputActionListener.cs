using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _BMSLabSource.Scripts.Runtime.InputHandling
{
    public class InputActionListener : MonoBehaviour
    {
        [System.Serializable]
        public class ActionEvent : UnityEvent<InputAction.CallbackContext> { }

        [SerializeField]
        private InputActionReference _actionReference;

        [SerializeField]
        private bool _invokeOnPerformed = true;
        [SerializeField]
        private bool _invokeOnCanceled = false;

        [SerializeField]
        private ActionEvent _eventToTrigger;

        void Start()
        {
            if (_actionReference == null || _eventToTrigger == null) return;

            _actionReference.action.Enable();

            if (_invokeOnPerformed)
                _actionReference.action.performed += (context) => { _eventToTrigger.Invoke(context); };

            if (_invokeOnCanceled)
                _actionReference.action.canceled += (context) => { _eventToTrigger.Invoke(context); };
        }

        private void OnDestroy()
        {
            _actionReference.action.performed -= (context) => { _eventToTrigger.Invoke(context); };
            _actionReference.action.canceled -= (context) => { _eventToTrigger.Invoke(context); };
        }
    }
}