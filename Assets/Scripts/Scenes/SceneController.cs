using Player;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Scenes
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private Transform startingPoint;
        [SerializeField] private Vector3 startingPositionLobby;
        [SerializeField] private Quaternion startingRotationLobby;
        private GameObject _player;
        private GameObject _mainCamera;
        private GameObject _rtCamera;
        private bool _mobile;

        void Start()
        {
            _mobile = SceneManager.GetSceneAt(0).name == "AppOffline";
            _player = GameObject.FindWithTag("NetworkCamera");
            
            _mainCamera = GameObject.FindWithTag("MainCamera");
            
            MovePlayersAtStartingPosition();
            
        }

        public void MovePlayersAtStartingPosition()
        {
            Debug.Log("MOVE player at starting position");
            _player.transform.position = startingPoint.position;
            _player.transform.rotation = startingPoint.rotation;

            if (_mainCamera)
            {
                _mainCamera.transform.parent = _player.transform;
                _mainCamera.transform.position = _player.transform.position;
                _mainCamera.transform.localPosition = new Vector3(0, 1.5f, -0.273f);
            }

            // if (_mainCamera.name == "MobileCamera")
            // {
            //     _mainCamera.transform.parent = null;
            // }
        }

        private void AttachScriptsVR()
        {
            Debug.Log("attaching scripts VR");
        }

        public void MovePlayersAtStartingPositionLobby()
        {
            _player.transform.position = startingPositionLobby;
            _player.transform.rotation = startingRotationLobby;
        }
    }
}