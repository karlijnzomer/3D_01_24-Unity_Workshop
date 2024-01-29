using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.ScriptableObjects.GameEvents
{
    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "Scriptable Objects/Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        [HideInInspector]
        public UnityEvent Event;

        public void Invoke()
        {
            Event?.Invoke();
        }
    }
}
