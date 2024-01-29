using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.ScriptableObjects.GameEvents
{
    public class AudioClipGameEventListener : MonoBehaviour
    {
        public List<AudioClipGameEvent> AudioClipGameEvents = new();
        public UnityEvent<AudioClipGameEvent> AudioClipGameEventReceived;

        private void OnEnable()
        {
            foreach (var audioClipGameEvent in AudioClipGameEvents)
            {
                audioClipGameEvent.Event.AddListener(() => OnAudioClipGameEventReceived(audioClipGameEvent));
            }
        }

        private void OnDisable()
        {
            foreach (var audioClipGameEvent in AudioClipGameEvents)
            {
                audioClipGameEvent.Event.RemoveListener(() => OnAudioClipGameEventReceived(audioClipGameEvent));
            }
        }

        public void OnAudioClipGameEventReceived(AudioClipGameEvent gameEvent)
        {
            AudioClipGameEventReceived?.Invoke(gameEvent);
        }
    }
}
