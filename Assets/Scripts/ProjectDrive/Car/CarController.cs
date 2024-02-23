using System.Linq;
using ProjectDrive.Car.Engine.Config;
using ProjectDrive.Car.Transmission;
using ProjectDrive.Car.Transmission.Config;
using ProjectDrive.Car.Wheel;
using ProjectDrive.UI;
using UnityEngine;

namespace ProjectDrive.Car
{
    public class CarController : MonoBehaviour
    {
        public RPMGaugeController rpmGaugeController;

        [SerializeField] private EngineConfigSO engineConfigSO;
        [SerializeField] private AutomaticTransmissionConfigSO automaticTransmissionConfigSO;
        [SerializeField] private WheelController[] wheelControllers;
        [SerializeField] private Rigidbody rigidBody;

        private Engine.Engine engine;
        private AutomaticTransmission automaticTransmission;
        
        private float ThrottleInput { get; set; }
        private float BrakeInput { get; set; }

        private void Start()
        {
            engine = new Engine.Engine(engineConfigSO);
            automaticTransmission = new AutomaticTransmission(automaticTransmissionConfigSO);
            engine.Start();
        }

        private void FixedUpdate()
        {
            HandleUserInput();
            UpdateTransmission();
            ApplyEnginePower();
            UpdateUI();
        }

        private void HandleUserInput()
        {
            var targetThrottle = Input.GetKey(KeyCode.W) && !automaticTransmission.IsShifting ? 1f : 0f;
            var targetBrake = Input.GetKey(KeyCode.S) ? 1f : 0f;

            ThrottleInput = Mathf.MoveTowards(ThrottleInput, targetThrottle,
                Time.fixedDeltaTime * CalculateInputSpeed(targetThrottle));
            BrakeInput = Mathf.MoveTowards(BrakeInput, targetBrake,
                Time.fixedDeltaTime * CalculateInputSpeed(targetBrake));
        }

        private void UpdateTransmission()
        {
            var currentSpeedKMH = rigidBody.velocity.magnitude * 3.6f;
            automaticTransmission.HandleTransmission(currentSpeedKMH, engine.CurrentRPM);
        }

        private void ApplyEnginePower()
        {
            var tractionRPM = CalculateTractionRPM();
            engine.UpdateEngineRPM(tractionRPM, ThrottleInput, automaticTransmission.ClutchInput,
                automaticTransmission.CurrentGearRatio);
            DistributeTorque();
        }

        private void UpdateUI()
        {
            rpmGaugeController.UpdateRPM(engine.CurrentRPM);
            rpmGaugeController.UpdateSpeed(rigidBody.velocity.magnitude * 3.6f);
            rpmGaugeController.UpdateGearText(automaticTransmission.CurrentGearIndex + 1);
        }

        private float CalculateTractionRPM() =>
            wheelControllers.Where(wheel => wheel.canPower).Sum(wheel => Mathf.Abs(wheel.GetRPM()));

        private int GetPoweredWheelCount() => wheelControllers.Count(wheel => wheel.canPower);

        private float CalculateInputSpeed(float input) => input == 1f ? 2 : 5;

        private void DistributeTorque()
        {
            var torquePerWheel = engine.GetTorque(automaticTransmission.CurrentGearRatio) / GetPoweredWheelCount();
            foreach (var wheel in wheelControllers)
            {
                if(wheel.canPower)
                    wheel.ApplyMotorTorque(torquePerWheel * ThrottleInput * (1 - automaticTransmission.ClutchInput));
                
                wheel.ApplyBrakeTorque(2500f * BrakeInput);
            }
        }
    }
}