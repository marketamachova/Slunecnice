using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

namespace Cart
{
    public class CartCreator : MonoBehaviour
    {
        [SerializeField] private GameObject leftHand;
        [SerializeField] private GameObject rightHand;
        [SerializeField] private GameObject cartPrefab;
        [SerializeField] private UIControllerVRLobby uiController;
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


        private List<Vector3> _createdPoints = new List<Vector3>();
        // private List<Vector3> rightSidePoints = new List<Vector3>();

        public event Action OnCartCreated;


        private void Awake()
        {
            _leftHand = leftHand.GetComponent<OVRHand>();
            _rightHand = rightHand.GetComponent<OVRHand>();
            _leftHandSkeleton = leftHand.GetComponent<OVRSkeleton>();
            _rightHandSkeleton = rightHand.GetComponent<OVRSkeleton>();
        }

        private void Update()
        {
            // if (_interactable)
            // {
            //     var rightArrowPressed = Input.GetKeyDown(KeyCode.RightArrow);
            //     var leftArrowPressed = Input.GetKeyDown(KeyCode.LeftArrow);
            //
            //     if (rightArrowPressed || leftArrowPressed)
            //     {
            //         _interactable = false;
            //
            //         if (rightArrowPressed && !_rightSideCreated)
            //         {
            //             Debug.Log("RIGHT index pinhing");
            //             _currentPointPositionList.Add(new Vector3(0, 1.08f, 0));
            //             _rightSideCreated = true;
            //             _lastPointPosition = _currentPointPositionList[_currentPointPositionList.Count - 1];
            //             CreateCart(_lastPointPosition);
            //         }
            //
            //         if (leftArrowPressed && !_leftSideCreated && _rightSideCreated)
            //         {
            //             Debug.Log("LEFT index pinhing");
            //             _currentPointPositionList.Add(new Vector3(-0.7936f, 1.0853f, 0.2373f));
            //             _leftSideCreated = true;
            //             RotateCart(_lastPointPosition, _currentPointPositionList[_currentPointPositionList.Count - 1]);
            //         }
            //
            //
            //         _interactable = true;
            //     }
            // }

            if (_interactable)
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

        private void CreateCart(Vector3 pointPosition)
        {
            _cart = Instantiate(cartPrefab, pointPosition, cartPrefab.transform.rotation);
            uiController.EnablePanelExclusive("Step2");
        }

        private void RotateCart(Vector3 pos1, Vector3 pos2)
        {
            Vector3 pointsDifferenceVector = new Vector3(pos2.x - pos1.x, 0, pos2.z - pos1.z);
            var rotationAngle = Vector3.SignedAngle(Vector2.left, pointsDifferenceVector.normalized, Vector3.up);
            _cart.transform.Rotate(new Vector3(0, rotationAngle, 0));

            uiController.EnablePanelExclusive("Step3");
            uiController.ActivateTrue("DoneButton");
        }

        public void Reset()
        {
            Destroy(_cart);
            _currentPointPositionList = new List<Vector3>();
            _rightSideCreated = false;
            _leftSideCreated = false;
            uiController.EnablePanelExclusive("Step1");
            uiController.ActivateFalse("DoneButton");
        }
    }
}