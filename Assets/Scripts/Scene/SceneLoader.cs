using System;
using System.Collections;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using NetworkPlayer = Network.NetworkPlayer;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private LoaderUI loaderUI;

    private AsyncOperation _sceneLoadingOperation;
    private GameObject _camera;
    private GameObject _networkCamera;

    public event Action SceneLoadingBegin;
    public event Action SceneLoadingEnd;

    void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera");
    }
    
    public void LoadScene(string scene, bool additiveSceneMode)
    {
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
        while (!_sceneLoadingOperation.isDone)
        {
            float loadingProgress = _sceneLoadingOperation.progress;
            loaderUI.UpdateLoader(loadingProgress);
            
            yield return null;
        }
        
        SceneLoadingEnd?.Invoke();
        loaderUI.DisplayLoader(false);

        yield return new WaitForEndOfFrame();
    }
}