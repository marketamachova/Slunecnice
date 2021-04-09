using Mirror;
using Player;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class PlayerCamera : NetworkBehaviour
    {
        [SerializeField] private SceneController sceneController;
        [SerializeField] private GameObject rtCamera;
        [SerializeField] private GameObject playerGazeDummy;

        private bool _vrInstance;

        private GameObject _cameraRig;
        private GameObject _centerEyeAnchor;
        private Vector3 _position; // position of player gaze
    
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
                _ovrManager = _cameraRig.GetComponent<OVRManager>();
                _ovrManager = _cameraRig.GetComponent<OVRManager>();
            
                _centerEyeAnchor = GameObject.FindWithTag("CenterEyeAnchor");
            }
        }
    

        void Update()
        {
            if (_vrInstance)
            {
                SyncUserPositionAndRotation();
            }
        }
    

        /**
     * Update networked RTCamera transforms
     */
        private void SyncUserPositionAndRotation()
        {
            // Debug.Log(_ovrManager);
            // if (_ovrManager != null) //TODO
            {
                // var playerRotation = _ovrManager.headPoseRelativeOffsetRotation;
                // var playerViewPortPos = _ovrManager.headPoseRelativeOffsetTranslation;

                var playerViewportPosition = _centerEyeAnchor.transform.position;
                var playerViewportRotation= _centerEyeAnchor.transform.rotation;
            
                // Debug.Log(playerViewportRotation.x);
                // Debug.Log("pos x: " + playerViewportPosition.x);

                // rtCamera.transform.rotation = Quaternion.Inverse(Quaternion.Euler(pla));
                rtCamera.transform.rotation = playerViewportRotation;
                // rtCamera.transform.Rotate(0, 70f, 0);

                playerGazeDummy.transform.position = playerViewportPosition;
                playerGazeDummy.transform.rotation = playerViewportRotation;

                var playerPosition = _cameraRig.transform.position;
                rtCamera.transform.position = playerViewportPosition;
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
}