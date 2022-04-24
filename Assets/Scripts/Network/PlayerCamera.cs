using Mirror;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    /**
     * handles syncing of VR player's viewpoint with render-texture cameras
     * 1. synchronises the OVRCameraRig position with cameras positions
     * 2. synchronises the CenterEyeAnchor rotation with rtCamera and rtCameraWides rotations
     */
    public class PlayerCamera : NetworkBehaviour
    {
        [SerializeField] private GameObject rtCamera;
        [SerializeField] private GameObject rtWideCamera;
        [SerializeField] private GameObject rtWideTopCamera;
        [SerializeField] private GameObject rtTopCamera;

        private bool _vrInstance;
        private GameObject _cameraRig;
        private GameObject _centerEyeAnchor;
        private VRController _vrController;


        private void Awake()
        {
            _vrInstance = SceneManager.GetActiveScene().name == GameConstants.VROffline;

            if (_vrInstance)
            {
                SceneManager.sceneLoaded += AssignChild;
                SceneManager.sceneLoaded += UpdateCameraRig;

                _centerEyeAnchor = GameObject.FindWithTag(GameConstants.CenterEyeAnchor);
                _cameraRig = GameObject.FindWithTag(GameConstants.MainCamera);
            }
        }


        void Update()
        {
            if (_vrInstance)
            {
                SyncUserPositionAndRotation();
            }
        }

        
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


        /**
         * needs to have Scene and LoadSceneMode arguments
         * in order to be subscribed to SceneManager event
         */
        private void UpdateCameraRig(Scene arg0, LoadSceneMode loadSceneMode)
        {
            if (_vrInstance)
            {
                _cameraRig = GameObject.FindWithTag(GameConstants.MainCamera);
            }
        }

        /**
         * needs to have Scene and LoadSceneMode arguments
         * in order to be subscribed to SceneManager event
         */
        private void AssignChild(Scene arg0, LoadSceneMode loadSceneMode)
        {
            _cameraRig = GameObject.FindWithTag(GameConstants.MainCamera);
            _cameraRig.transform.parent = this.transform;
        }
    }
}