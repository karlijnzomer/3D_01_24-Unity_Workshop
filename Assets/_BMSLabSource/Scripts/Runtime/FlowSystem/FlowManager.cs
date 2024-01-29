using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;
using UnityEngine.SceneManagement;

namespace _BMSLabSource.Scripts.Runtime.FlowSystem
{
    /// <summary>
    /// The Flow Manager manages action components on children GameObjects of this object.
    /// </summary>
    public class FlowManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Automatically initiates the first available action.")]
        private bool _autoStart = true;

        [SerializeField, ReadOnly, Tooltip("Action components of children GameObjects. Will be auto-filled on runtime.")]
        protected List<ActionBase> _actions = new List<ActionBase>();

        private ActionBase _currentAction = null;

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                ActionBase action = child.GetComponent<ActionBase>();

                if (action != null)
                    _actions.Add(action);
            }

            foreach (ActionBase action in _actions)
            {
                action.Completed.AddListener(OnActionCompleted);
            }
        }

        private void Start()
        {
            if (_autoStart)
            {
                InitiateAction();
            }
        }

        private void OnActionCompleted(ActionBase action)
        {
            _actions.Remove(action);
            InitiateAction();
        }

        private void InitiateAction()
        {
            if (_actions.Count > 0)
            {
                var action = _actions[0];
                _currentAction = action;
                action.Initiate();
            }
            else
            {
                _currentAction = null;
                Debug.Log(gameObject.name + " (" + gameObject.scene.name + ") completed its actions.", gameObject);
            }
        }

        [ButtonMethod]
        public void CompleteActiveAction()
        {
            if (_currentAction != null)
            {
                _currentAction.Complete();
            }
            else
            {
                Debug.Log("No actions remaining to complete.", gameObject);
            }
        }
    }
}
