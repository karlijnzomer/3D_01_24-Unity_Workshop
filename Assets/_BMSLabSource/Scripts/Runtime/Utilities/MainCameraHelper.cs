using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.Utilities
{
    public class MainCameraHelper : MonoBehaviour
    {
        [SerializeField]
        private bool _debug = false;
        private Camera _mainCamera = null;

        public UnityEvent<Transform> MainCameraFound;

        [SerializeField, Tooltip("Provide the tag that should be searched for. E.g. FollowPoint for UI windows.")]
        private string _targetTag = "FollowPoint";

        public UnityEvent<Transform> FollowPointFound;


        private void Start()
        {
            _mainCamera = Camera.main;

            if (_mainCamera != null)
            {
                if (_debug)
                    Debug.Log("Found main camera: " + _mainCamera.name, gameObject);

                MainCameraFound.Invoke(_mainCamera.transform);
                SearchForFollowPoint();
                return;

            }

            if (_debug)
                Debug.Log("Did not find a main camera on startup. Make sure your main camera is enabled.", gameObject);

        }

        private void SearchForFollowPoint()
        {
            foreach (Transform child in _mainCamera.transform)
            {
                if (child.CompareTag(_targetTag))
                {
                    if (_debug)
                        Debug.Log("Found follow point: " + child.name, gameObject);

                    FollowPointFound.Invoke(child);
                    return;
                }
            }

            if (_debug)
                Debug.Log("No child with tag " + _targetTag + " found under main camera.", gameObject);
        }

    }
}
