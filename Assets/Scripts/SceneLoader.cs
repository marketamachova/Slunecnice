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
    public event Action RandomEvent;

    void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera");
    }
    
    public void LoadScene(string scene, bool additiveSceneMode)
    {
        Debug.Log("scene loader.LoadScene");
        loaderUI.DisplayLoader();
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
        RandomEvent?.Invoke();
        while (!_sceneLoadingOperation.isDone)
        {
            float loadingProgress = _sceneLoadingOperation.progress;
            loaderUI.UpdateLoader(loadingProgress);
            
            yield return null;
        }
        
        Debug.Log("loading scene END");
        SceneLoadingEnd?.Invoke();
        Debug.Log("loading scene END");

        yield return new WaitForEndOfFrame();


        // Debug.Log("joining");
        // _networkPlayer.GetComponent<NetworkPlayer>().Join();
        // Debug.Log("joined");

        // EnableVRController();
    }
}