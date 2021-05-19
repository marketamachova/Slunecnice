using System;
using System.Collections;
using Player;
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
            _camera = GameObject.FindWithTag(GameConstants.MainCamera);
        }

        public void LoadScene(string scene, bool additiveSceneMode)
        {
            _currentSceneName = SceneManager.GetActiveScene().name;
            _loadedSceneName = scene;

            loaderUI.DisplayLoader(true);

            _sceneLoadingOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

            DetachCameraFromNetworkPlayer();

            StartCoroutine(LoadSceneAsync());
        }

        private void DetachCameraFromNetworkPlayer()
        {
            //detach camera from NetworkCamera
            if (_camera)
            {
                _camera.transform.parent = null;
            }

            // move camera out of DontDestroyOnLoad
            SceneManager.MoveGameObjectToScene(_camera, SceneManager.GetActiveScene());
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
            SceneManager.UnloadSceneAsync(_currentSceneName);
        }
    }
}