using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.Utilities
{
    public class ApplicationUnityEvent : MonoBehaviour
    {
        public UnityEvent ApplicationQuit;

        private void OnApplicationQuit()
        {
            ApplicationQuit?.Invoke();
        }
    }
}
