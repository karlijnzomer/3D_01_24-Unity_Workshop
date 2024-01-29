using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.ScriptableObjects.GameEvents
{
    [CreateAssetMenu(fileName = "NewAudioClipGameEvent", menuName = "Scriptable Objects/Events/AudioClip Game Event")]
    public class AudioClipGameEvent : GameEvent
    {
        public AudioClip AudioClip;

        public float GetClipLength()
        {
            return AudioClip.length;
        }

        public void Test()
        {

        }
    }
}
