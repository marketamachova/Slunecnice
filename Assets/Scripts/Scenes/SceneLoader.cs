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
        private Fader _fader;
        private string _currentSceneName;

        public event Action SceneLoadingBegin;
        public event Action SceneLoadingEnd;
        public event Action UnloadSceneBegin;

        void Start()
        {
            _camera = GameObject.FindWithTag("MainCamera");
            _fader = GetComponent<Fader>();
        }
    
        public void LoadScene(string scene, bool additiveSceneMode)
        {
            _currentSceneName = scene;
            Debug.Log("scene loader.LoadScene");
            loaderUI.DisplayLoader(true); //potreba?
            _sceneLoadingOperation = SceneManager.LoadSceneAsync(scene,
                additiveSceneMode ? LoadSceneMode.Additive : LoadSceneMode.Single);
            SceneLoadingBegin?.Invoke();
            DetachCameraFromNetworkPlayer();
            StartCoroutine(LoadSceneAsync());
        }

        private void DetachCameraFromNetworkPlayer()
        {
            //detach camera from networkCamera
            _camera.transform.parent = null;
        
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

                yield return null;
            }

            SceneLoadingEnd?.Invoke();
            loaderUI.DisplayLoader(false);
            // _camera.GetComponent<Fader>().FadeIn();

            
            if (cameraFader != null)
            {
                cameraFader.FadeIn();
            }
            yield return new WaitForEndOfFrame();
        }
        

        public void UnloadScene()
        {
            UnloadSceneBegin?.Invoke();
            SceneManager.UnloadSceneAsync(_currentSceneName);
        }
    }
}