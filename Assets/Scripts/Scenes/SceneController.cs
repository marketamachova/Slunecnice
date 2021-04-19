using PathCreation;
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
        [SerializeField] private GameObject pathCreator;
        private GameObject _player;
        private GameObject _mainCamera;
        private GameObject _rtCamera;
        private GameObject _cart;
        private bool _mobile;
        
        private const float CartHeight = 1.49222f;

        void Start()
        {
            _mobile = SceneManager.GetSceneAt(0).name == "AppOffline";
            _player = GameObject.FindWithTag("NetworkCamera");
            _cart = GameObject.FindWithTag("Cart");
            
            _mainCamera = GameObject.FindWithTag("MainCamera");
            
            MovePlayersAtStartingPosition();
            
        }

        public void MovePlayersAtStartingPosition()
        {
            Debug.Log("MOVE player at starting position");
            _player.transform.position = startingPoint.position;
            _player.transform.rotation = startingPoint.rotation;

            if (_mainCamera) //wtf
            {
                _mainCamera.transform.parent = _player.transform;
                _mainCamera.transform.position = _player.transform.position;
                _mainCamera.transform.localPosition = new Vector3(0, 1.5f, -0.273f);
            }
            
            // PositionPathCreator();
        }

        private void PositionPathCreator()
        {
            var pathCreatorZeroPositionY = pathCreator.transform.position.y;
            var cartPivotCenterDistanceY = _cart.transform.position.y - _cart.GetComponent<Renderer>().bounds.center.y;
            var pathCreatorPositionY = CartHeight - cartPivotCenterDistanceY + pathCreatorZeroPositionY;
            var pathCreatorPosition = pathCreator.transform.position;
            pathCreator.transform.position =
                new Vector3(pathCreatorPosition.x, pathCreatorPositionY, pathCreatorPosition.z);
            Debug.Log("set pathCreator position to " + pathCreator.transform.position);
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