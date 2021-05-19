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


        private void UpdateCameraRig(Scene arg0, LoadSceneMode loadSceneMode)
        {
            if (_vrInstance)
            {
                _cameraRig = GameObject.FindWithTag(GameConstants.MainCamera);
            }
        }

        private void AssignChild(Scene arg0, LoadSceneMode loadSceneMode)
        {
            _cameraRig = GameObject.FindWithTag(GameConstants.MainCamera);
            _cameraRig.transform.parent = this.transform;
        }
    }
}