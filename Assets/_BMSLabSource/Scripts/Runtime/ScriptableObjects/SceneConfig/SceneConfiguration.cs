using UnityEngine;
using MyBox;
using System;

namespace _BMSLabSource.Scripts.Runtime.ScriptableObjects.SceneConfig
{

    [CreateAssetMenu(fileName = "New Scene Configuration", menuName = "Scene System/Scene Configuration")]
    public class SceneConfiguration : ScriptableObject, ISerializationCallbackReceiver
    {
        public SceneReference[] Scenes;

        public int CurrentIndex;

        [ReadOnly]
        private int _baseIndex = 0;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            CurrentIndex = _baseIndex;
        }

        public int GetSceneIndex()
        {
            return CurrentIndex;
        }

        public void IncrementSceneIndex()
        {
            CurrentIndex++;
        }
    }

    [Serializable]
    public class SceneReference
    {
        public UnityEngine.Object SceneAsset;
    }
}
