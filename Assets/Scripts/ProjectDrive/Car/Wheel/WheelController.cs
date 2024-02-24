using System;
using UnityEngine;

namespace ProjectDrive.Car.Wheel
{
    [RequireComponent(typeof(WheelCollider))]
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private GameObject wheelObject;
        [SerializeField] private WheelCollider wheelCollider;
        
        
        public bool canPower;

        public float GetRPM() => wheelCollider.rpm;

        private void Start()
        {
            wheelCollider = GetComponent<WheelCollider>();

            if (wheelObject == null)
            {
                Debug.LogError("Wheel GameObject is not assigned!");
            }
        }

        private void Update()
        {
            UpdateWheelPose();
        }

        private void UpdateWheelPose()
        {
            wheelCollider.GetWorldPose(out var pos, out var rot);
            wheelObject.transform.position = pos;
            wheelObject.transform.rotation = rot;
        }

        public void ApplyMotorTorque(float torque)
        {
            wheelCollider.motorTorque = torque;
        }

        public void ApplyBrakeTorque(float torque)
        {
            wheelCollider.brakeTorque = torque;
        }

        
    }
}