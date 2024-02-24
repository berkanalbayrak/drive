using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectDrive.Camera
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class CineMachineConditionalFreeLook : MonoBehaviour
    {
        private CinemachineFreeLook _freeLookCamera;
        private float _xInputAxisDefaultSpeed;
        private float _yInputAxisDefaultSpeed;

        private void Awake()
        {
            _freeLookCamera = GetComponent<CinemachineFreeLook>();
            
            _xInputAxisDefaultSpeed = _freeLookCamera.m_XAxis.m_MaxSpeed;
            _yInputAxisDefaultSpeed = _freeLookCamera.m_YAxis.m_MaxSpeed;
        }

        private void Update()
        {
            
            var enableFreeLook = (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject());
            
            _freeLookCamera.m_XAxis.m_MaxSpeed = enableFreeLook ? _xInputAxisDefaultSpeed : 0;
            _freeLookCamera.m_YAxis.m_MaxSpeed = enableFreeLook ? _yInputAxisDefaultSpeed : 0;
        }
    }
}