using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.Utilities
{
    public class CollisionToEvent : MonoBehaviour
    {
        [Serializable]
        public class GameObjectUnityEvent : UnityEvent<GameObject> { }

        [Header("Configuration")]
        [SerializeField, Tooltip("Write log messages to the console.")]
        private bool _debug = false;
        [SerializeField]
        private List<GameObject> _acceptedObjects = new();

        [Header("Events")]
        public GameObjectUnityEvent TriggerEntered, TriggerExited, CollisionEntered, CollisionExited;

        private Dictionary<GameObject, int> _colliderCount = new Dictionary<GameObject, int>();
        private Dictionary<GameObject, int> _collisionCount = new Dictionary<GameObject, int>();

        private void Start() { }

        private void OnTriggerEnter(Collider other)
        {

            if (_colliderCount.ContainsKey(other.gameObject))
            {
                _colliderCount[other.gameObject]++;
            }
            else
            {
                _colliderCount[other.gameObject] = 1;
                HandleTriggerStateChange(other, TriggerEntered, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_colliderCount.ContainsKey(other.gameObject))
            {
                _colliderCount[other.gameObject]--;
                if (_colliderCount[other.gameObject] <= 0)
                {
                    _colliderCount.Remove(other.gameObject);
                    HandleTriggerStateChange(other, TriggerExited, false);
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var go = collision.gameObject;

            if (_collisionCount.ContainsKey(go))
            {
                _collisionCount[go]++;
            }
            else
            {
                _collisionCount[go] = 1;
                HandleCollisionStateChange(collision, CollisionEntered, true);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            var go = collision.gameObject;

            if (_collisionCount.ContainsKey(go))
            {
                _collisionCount[go]--;
                if (_collisionCount[go] <= 0)
                {
                    _collisionCount.Remove(go);
                    HandleCollisionStateChange(collision, CollisionExited, false);
                }
            }
        }

        private void HandleTriggerStateChange(Collider other, GameObjectUnityEvent gameObjectUnityEvent, bool entered)
        {
            if (_acceptedObjects.Contains(other.gameObject) || (_acceptedObjects.Count == 0))
            {
                gameObjectUnityEvent?.Invoke(other.gameObject);

                if (_debug == false)
                    return;

                if (entered)
                {
                    Debug.Log("Trigger entered: " + other.gameObject.name, gameObject);
                }
                else
                {
                    Debug.Log("Trigger exited: " + other.gameObject.name, gameObject);
                }
            }
        }

        private void HandleCollisionStateChange(Collision collision, GameObjectUnityEvent gameObjectUnityEvent, bool entered)
        {
            if (_acceptedObjects.Contains(collision.gameObject) || (_acceptedObjects.Count == 0))
            {
                gameObjectUnityEvent?.Invoke(collision.gameObject);

                if (_debug == false)
                    return;

                if (entered)
                {
                    Debug.Log("Collision entered: " + collision.gameObject.name, gameObject);
                }
                else
                {
                    Debug.Log("Collision exited: " + collision.gameObject.name, gameObject);
                }
            }
        }
    }
}
