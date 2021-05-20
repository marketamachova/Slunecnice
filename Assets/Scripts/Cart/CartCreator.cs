using System;
using System.Collections.Generic;
using System.Numerics;
using Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Cart
{
    /**
    * manages the calibration process - creates and rotates cart at points indicated by user
    */
    public class CartCreator : MonoBehaviour
    {
        [Header("Hands")] [SerializeField] private GameObject leftHand;
        [SerializeField] private GameObject rightHand;

        [Header("Cart")] [SerializeField] private Material cartMaterial;
        [SerializeField] private GameObject cartPrefab;
        [SerializeField] private Vector3 defaultCartPosition;

        [Header("UI")] [SerializeField] private CartCreationUIController uiController;
        [SerializeField] private bool debug = true;

        private readonly List<Renderer> _cartRendererComponents = new List<Renderer>();
        private OVRHand _leftHand;
        private OVRHand _rightHand;
        private OVRSkeleton _leftHandSkeleton;
        private OVRSkeleton _rightHandSkeleton;
        private int _stepCounter;

        private bool _interactable = true;

        private bool _isLeftIndexFingerPinching = false;
        private bool _isRightIndexFingerPinching = false;
        private bool _leftSideCreated = false;
        private bool _rightSideCreated = false;

        private List<Vector3> _currentPointPositionList = new List<Vector3>();
        private Vector3 _lastPointPosition;
        private GameObject _cart;

        public event Action OnCartCreatorCalibrationComplete;


        private void Awake()
        {
            _leftHand = leftHand.GetComponent<OVRHand>();
            _rightHand = rightHand.GetComponent<OVRHand>();
            _leftHandSkeleton = leftHand.GetComponent<OVRSkeleton>();
            _rightHandSkeleton = rightHand.GetComponent<OVRSkeleton>();
        }

        /**
         * Waits first for detecting the pinch gesture on right hand => instantiate cart at the indicated point,
         * then for the left => rotates the cart to be in accordance with the left indicated point
         * saves the indicated points in the _currenPointsPositionList
         * 
         */
        private void Update()
        {
            if (_interactable && !EventSystem.current.currentSelectedGameObject)
            {
                _isLeftIndexFingerPinching = _leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
                _isRightIndexFingerPinching = _rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);


                if (_isLeftIndexFingerPinching || _isRightIndexFingerPinching)
                {
                    _interactable = false;


                    if (_isRightIndexFingerPinching && !_rightSideCreated)
                    {
                        _rightSideCreated = true;
                        _currentPointPositionList.Add(_rightHandSkeleton.Bones[20].Transform.position);
                        _lastPointPosition = _currentPointPositionList[_currentPointPositionList.Count - 1];
                        CreateCart(_lastPointPosition);
                    }

                    if (_isLeftIndexFingerPinching && !_leftSideCreated && _rightSideCreated)
                    {
                        _leftSideCreated = true;
                        _currentPointPositionList.Add(_leftHandSkeleton.Bones[20].Transform.position);
                        RotateCart(_lastPointPosition, _currentPointPositionList[_currentPointPositionList.Count - 1]);
                    }

                    _interactable = true;
                }
            }
        }

        /**
         * instantiate cart at the given position
         * displays UI step 2
         */
        private void CreateCart(Vector3 pointPosition)
        {
            _cart = Instantiate(cartPrefab, pointPosition, cartPrefab.transform.rotation);

            uiController.DisplayCalibrationStep2();
        }

        /**
         * rotates the cart in a following way:
         * 1. compute the distance between the indicated points in 2D space (plain from the x-axis and z-axis)
         * (y position is fixed with the first indicated point)
         * 2. computes the angle by which the newly created cart has to be rotated around the y-axis
         * 3. rotates cart by the angle about the y-axis
         * displays UI step 3
         */
        private void RotateCart(Vector3 pos1, Vector3 pos2)
        {
            Vector3 pointsDifferenceVector = new Vector3(pos2.x - pos1.x, 0, pos2.z - pos1.z);
            var rotationAngle = Vector3.SignedAngle(Vector2.left, pointsDifferenceVector.normalized, Vector3.up);
            _cart.transform.Rotate(new Vector3(0, rotationAngle, 0));

            uiController.DisplayCalibrationStep3();
        }


        public void Reset()
        {
            Destroy(_cart);
            _cart = null;
            _currentPointPositionList = new List<Vector3>();
            _rightSideCreated = _leftSideCreated = false;

            uiController.DisplayCalibrationStep1();
        }

        private void ColorCart()
        {
            _cartRendererComponents.AddRange(_cart.GetComponentsInChildren<Renderer>());
            _cartRendererComponents.ForEach(component => component.material = cartMaterial);
        }

        public void SkipCalibration()
        {
            if (_cart != null)
            {
                Destroy(_cart);
            }

            _cart = Instantiate(cartPrefab, cartPrefab.transform.position, cartPrefab.transform.rotation);
            EndCalibration();
        }

        /**
         * 1. disables detecting of pinch gestures
         * 2. rotates cart and the player in order to face the UI
         * 3. sets cart the children of player
         * 4. invokes calibration complete event
         */
        public void EndCalibration()
        {
            _interactable = false;
            ColorCart();

            var networkCamera = GameObject.FindWithTag(GameConstants.NetworkCamera);

            var cartRotation = _cart.transform.eulerAngles;
            _cart.transform.Rotate(-cartRotation);


            var children = networkCamera.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                child.transform.Rotate(-cartRotation);
            }

            networkCamera.transform.Rotate(cartRotation);
            _cart.transform.parent = networkCamera.transform;

            OnCartCreatorCalibrationComplete?.Invoke();
        }
    }
}