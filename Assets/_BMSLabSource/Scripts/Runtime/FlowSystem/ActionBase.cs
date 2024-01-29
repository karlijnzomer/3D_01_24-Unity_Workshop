using UnityEngine;
using UnityEngine.Events;
using MyBox;

namespace _BMSLabSource.Scripts.Runtime.FlowSystem
{
    /// <summary>
    /// An individual action throws events when it begins and when it is completed.
    /// Actions are managed by a FlowManager.
    /// </summary>
    public class ActionBase : MonoBehaviour
    {
        [SerializeField]
        private string _description = "";

        [System.Serializable]
        public class UnityActionEvent : UnityEvent<ActionBase>
        {
        }

        [Header("Events")]
        public UnityActionEvent Begin;
        public UnityActionEvent Completed;

        private string _cachedGameObjectName;
        private string _runtimeGameObjectName;


        private void Awake()
        {
            Deactivate();
#if UNITY_EDITOR
            _runtimeGameObjectName = gameObject.name;
#endif
        }

        private void Start() { }

        private void Activate()
        {
            enabled = true;
            gameObject.SetActive(true);
#if UNITY_EDITOR
            gameObject.name = _runtimeGameObjectName + " (ACTIVE)";
#endif
        }

        private void Deactivate()
        {
            enabled = false;
            gameObject.SetActive(false);
        }

        public virtual void Initiate()
        {
            Activate();
            Begin?.Invoke(this);
        }

        [ButtonMethod]
        public void Complete()
        {
            Completed?.Invoke(this);

#if UNITY_EDITOR
            gameObject.name = _runtimeGameObjectName + " (COMPLETED)";
#endif

            Deactivate();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            CacheGameObjectName();
        }

        protected virtual void CacheGameObjectName()
        {
            _cachedGameObjectName = gameObject.GetComponent<ActionBase>().GetType().Name.ToString();
        }

        private void OnValidate()
        {
            CacheGameObjectName();

            if (_description == "" || _description == " ")
            {
                gameObject.name = _cachedGameObjectName;
                Debug.Log("+ " + _cachedGameObjectName);
            }
            else
            {
                gameObject.name = _cachedGameObjectName + " (" + _description + ")";
            }
        }
#endif
    }
}
