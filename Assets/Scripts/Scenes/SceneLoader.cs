﻿using System;
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

        public event Action SceneLoadingEnd;

        void Start()
        {
            _camera = GameObject.FindWithTag("MainCamera");
        }

        public void LoadScene(string scene, bool additiveSceneMode)
        {
            _currentSceneName = scene;
            
            Debug.Log("_currentSceneName" + _currentSceneName);
            Debug.Log("scene loader.LoadScene");
            
            loaderUI.DisplayLoader(true); //potreba?
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