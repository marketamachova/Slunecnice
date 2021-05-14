using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private LoaderUI loaderUI;

        private AsyncOperation _sceneLoadingOperation;
        private GameObject _camera;
        private GameObject _networkCamera;
        private string _currentSceneName;
        private string _loadedSceneName;

        public event Action SceneLoadingEnd;

        void Start()
        {
            _camera = GameObject.FindWithTag("MainCamera");
        }

        public void LoadScene(string scene, bool additiveSceneMode)
        {
            // _currentSceneName = scene;
            _currentSceneName = SceneManager.GetActiveScene().name;
            _loadedSceneName = scene;

            Debug.Log("_currentSceneName" + _currentSceneName);
            Debug.Log("scene loader.LoadScene");

            loaderUI.DisplayLoader(true); //potreba?

            var mobile = _currentSceneName == "AppOffline";

            // SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            _sceneLoadingOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

            DetachCameraFromNetworkPlayer();

            StartCoroutine(LoadSceneAsync());
        }

        private void DetachCameraFromNetworkPlayer()
        {
            //detach camera from networkCamera
            if (_camera)
            {
                _camera.transform.parent = null;
            }

            // move camera out of DontDestroyOnLoad
            SceneManager.MoveGameObjectToScene(_camera, SceneManager.GetActiveScene());
            // DontDestroyOnLoad(_camera);
        }

        private IEnumerator LoadSceneAsync()
        {
            var cameraFader = _camera.GetComponent<Fader>();
            if (cameraFader != null)
            {
                cameraFader.FadeOut();
            }

            while (!_sceneLoadingOperation.isDone)
            {
                float loadingProgress = _sceneLoadingOperation.progress;
                loaderUI.UpdateLoader(loadingProgress);

                yield return new WaitForSecondsRealtime(0.05f);
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_loadedSceneName));
            _currentSceneName = _loadedSceneName;


            yield return new WaitForSecondsRealtime(1);
            SceneLoadingEnd?.Invoke();
            loaderUI.DisplayLoader(false);

            if (cameraFader != null)
            {
                cameraFader.FadeIn();
            }

            yield return new WaitForEndOfFrame();
        }


        public void UnloadScene()
        {
            // SceneManager.UnloadScene(_currentSceneName);
            SceneManager.UnloadSceneAsync(_currentSceneName);
        }
    }
}