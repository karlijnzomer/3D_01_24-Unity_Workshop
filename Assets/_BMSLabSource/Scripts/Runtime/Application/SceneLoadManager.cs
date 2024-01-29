using _BMSLabSource.Scripts.Runtime.ScriptableObjects.SceneConfig;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace _BMSLabSource.Scripts.Runtime.Application
{
    public class SceneLoadManager : MonoBehaviour
    {
        private AsyncOperation _loadOperation;
        private bool _isLoading = false;

        private SceneConfiguration _sceneConfig = null;

        public UnityEvent SceneLoaded;

        [SerializeField]
        private bool _setSceneActiveOnLoad = false;

        private void Awake()
        {
            SceneLoaded?.AddListener(OnSceneLoaded);
        }

        public void LoadSceneAdditiveByReference(string sceneName)
        {
            if (!_isLoading)
            {
                StartCoroutine(LoadSceneAdditiveAsync(sceneName));
            }
            else
            {
                Debug.LogWarning("A scene is already being loaded.");
            }
        }

        private IEnumerator LoadSceneAdditiveAsync(string sceneName)
        {
            _isLoading = true;

            _loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!_loadOperation.isDone)
            {
                yield return null;
            }

            SceneLoaded?.Invoke();
            _isLoading = false;
            Debug.Log("Loaded scene:" + sceneName);

            if (_setSceneActiveOnLoad == true)
            {
                Scene loadedScene = SceneManager.GetSceneByName(sceneName);
                if (loadedScene.isLoaded)
                {
                    SceneManager.SetActiveScene(loadedScene);
                }
            }
        }

        public void UnloadThisScene()
        {
            StartCoroutine(WaitAndUnload());
        }

        private IEnumerator WaitAndUnload()
        {
            yield return new WaitForSeconds(0.1f);
            UnloadSceneByReference(gameObject.scene.name);
        }

        public void UnloadSceneByReference(string sceneName)
        {
            if (!_isLoading)
            {
                // Check if the scene is loaded before unloading it
                Scene scene = SceneManager.GetSceneByName(sceneName);
                if (scene.isLoaded)
                {
                    SceneManager.UnloadSceneAsync(sceneName);
                    Debug.Log("Unloaded scene: " + sceneName + ".", gameObject);
                }
                else
                {
                    Debug.LogWarning($"Scene '{sceneName}' is not loaded.");
                }
            }
            else
            {
                Debug.LogWarning("A scene is currently being loaded.");
            }
        }

        public void LoadSceneFromConfig(SceneConfiguration config)
        {
            if (config == null)
            {
                Debug.LogError("Missing SceneConfiguration reference.", gameObject);
                return;
            }

            _sceneConfig = config;

            if (config.GetSceneIndex() == config.Scenes.Length)
            {
                Debug.Log("No available scenes left to load.", gameObject);
                return;
            }

            var sceneToLoad = config.Scenes[config.GetSceneIndex()].SceneAsset.name.ToString();
            LoadSceneAdditiveByReference(sceneToLoad);
        }

        private void OnSceneLoaded()
        {
            if (_sceneConfig == null)
                return;

            _sceneConfig.IncrementSceneIndex();
        }
    }
}
