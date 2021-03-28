using Mirror;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private GameObject rtCamera;

    private bool _vrInstance;

    private GameObject _cameraRig;
    private Vector3 _position; // position of player gaze

    // [SerializeField] private Camera rightEye;
    // [SerializeField] private Camera leftEye;
    private OVRManager _ovrManager;
    private GameController _gameController;


    private void Awake()
    {
        _vrInstance = SceneManager.GetActiveScene().name == "VROffline";

        if (_vrInstance)
        {
            SceneManager.sceneLoaded += AssignChild;
            SceneManager.sceneLoaded += UpdateCameraRig;
            _cameraRig = GameObject.FindWithTag("MainCamera");
            Debug.Log(_cameraRig.name);
            _ovrManager = _cameraRig.GetComponent<OVRManager>();
            Debug.Log(_ovrManager);

            // _gameController = GetComponent<GameController>();
            // _gameController.player.Add(_cameraRig);
            // _gameController.PLa
        }
    }

    void Start()
    {
    }


    void Update()
    {
        if (_vrInstance)
        {
            SyncUserPositionAndRotation();
        }
    }

    private void EnableEyesCameras()
    {
        // leftEye.tag = "MainCamera";
        // rightEye.tag = "MainCamera";
        // leftEye.enabled = true;
        // rightEye.enabled = true;
    }

    /**
     * Update networked RTCamera transforms
     */
    private void SyncUserPositionAndRotation()
    {
        // Debug.Log(_ovrManager);
        if (_ovrManager != null) //TODO
        {
            var playerRotation = _ovrManager.headPoseRelativeOffsetRotation;
            var playerViewPortPos = _ovrManager.headPoseRelativeOffsetTranslation;
            rtCamera.transform.rotation = Quaternion.Inverse(Quaternion.Euler(playerRotation));

            var playerPosition = _cameraRig.transform.position;
            rtCamera.transform.position = playerPosition + playerViewPortPos;
        }
    }


    private void UpdateCameraRig(Scene arg0, LoadSceneMode loadSceneMode)
    {
        if (_vrInstance)
        {
            _cameraRig = GameObject.FindWithTag("MainCamera");
            _ovrManager = _cameraRig.GetComponent<OVRManager>();
        }
    }

    private void AssignChild(Scene arg0, LoadSceneMode loadSceneMode)
    {
        _cameraRig = GameObject.FindWithTag("MainCamera");
        Debug.Log("assigning child MAIN CAMERA DOPICI");
        _cameraRig.transform.parent = this.transform;
    }
}