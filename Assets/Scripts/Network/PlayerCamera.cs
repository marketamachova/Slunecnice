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
        [SerializeField] private GameObject rtWideCamera;
        [SerializeField] private GameObject rtWideTopCamera;
        [SerializeField] private GameObject rtTopCamera;
        // [SerializeField] private GameObject playerGazeDummy;

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
            var playerViewportPosition = _centerEyeAnchor.transform.position;
            var playerViewportRotation = _centerEyeAnchor.transform.rotation;

            rtCamera.transform.rotation = playerViewportRotation;
            rtWideCamera.transform.rotation = playerViewportRotation;


            var playerPosition = _cameraRig.transform.position;
            rtCamera.transform.position = playerViewportPosition;
            rtWideCamera.transform.position = playerViewportPosition;
            rtTopCamera.transform.position = new Vector3(playerPosition.x, 30f, playerPosition.z);
            rtWideTopCamera.transform.position = new Vector3(playerPosition.x, 30f, playerPosition.z);
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
            Debug.Log("assigning child MAIN CAMERA ");
            _cameraRig.transform.parent = this.transform;
        }
    }
}