using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace _BMSLabSource.Scripts.Runtime.Utilities
{
    public class WaitUnityEvent : MonoBehaviour
    {
        [SerializeField]
        private float _waitTime = 1f;
        public UnityEvent Event;

        public void WaitAndInvokeEvent()
        {
            StartCoroutine(Wait());
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(_waitTime);
            Event?.Invoke();
        }
    }
}
