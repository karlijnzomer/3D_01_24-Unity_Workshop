using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.ScriptableObjects.GameEvents
{
    public class GameEventListener : MonoBehaviour
    {
        public List<GameEvent> GameEvents = new();
        public UnityEvent<GameEvent> GameEventReceived;

        protected virtual void OnEnable()
        {
            foreach (var gameEvent in GameEvents)
            {
                gameEvent.Event.AddListener(() => OnGameEventReceived(gameEvent));
            }
        }

        protected virtual void OnDisable()
        {
            foreach (var gameEvent in GameEvents)
            {
                gameEvent.Event.RemoveListener(() => OnGameEventReceived(gameEvent));
            }
        }

        private void OnGameEventReceived(GameEvent gameEvent)
        {
            GameEventReceived?.Invoke(gameEvent);
        }
    }
}
