using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private GameObject playerGaze;
    [SerializeField] private GameObject rtCamera;

    private bool _vrInstance;

    private GameObject _cameraRig;
    private Vector3 _position; // position of player gaze

    // [SerializeField] private Camera rightEye;
    // [SerializeField] private Camera leftEye;
    private OVRManager _ovrManager;


    private void Awake()
    {
        _vrInstance = SceneManager.GetActiveScene().name == "VROffline";

        if (_vrInstance)
        {
            SceneManager.sceneLoaded += AssignChild;
            SceneManager.sceneLoaded += UpdateCameraRig;
            _cameraRig = GameObject.FindWithTag("MainCamera");
            _ovrManager = _cameraRig.GetComponent<OVRManager>();
            Debug.Log(_ovrManager);
        }
    }

    void Start()
    {
    }


    void Update()
    {
        if (_vrInstance)
        {
            HandleUserRotation();
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
    private void HandleUserRotation()
    {
        var playerRotation = _ovrManager.headPoseRelativeOffsetRotation;
        var playerHeight = _ovrManager.headPoseRelativeOffsetTranslation; // sync player height?
        playerGaze.transform.rotation = Quaternion.Euler(playerRotation);
        rtCamera.transform.rotation = Quaternion.Euler(playerRotation);
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