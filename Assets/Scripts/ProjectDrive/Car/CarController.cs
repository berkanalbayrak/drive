using System;
using System.Linq;
using ProjectDrive.Car.Engine;
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
        [SerializeField] private EngineSoundPlayer engineSoundPlayer;
        [SerializeField] private Rigidbody rigidBody;

        private Engine.Engine _engine;
        private AutomaticTransmission _automaticTransmission;
        
        private EventBinding<ThrottleInputEvent> _gasInputEventBinding;
        private EventBinding<BrakeInputEvent> _brakeInputEventBinding;
        private EventBinding<RaceStartEvent> _raceStartEventBinding;

        private const float BRAKE_TORQUE = 2500f;
        
        private float _currentSpeed => rigidBody.velocity.magnitude * 3.6f;
        
        private float _activeThrottleInput;
        private float _activeBreakInput;
        
        private void Start()
        {
            _engine = new Engine.Engine(engineConfigSO);
            engineSoundPlayer.Initialize(_engine);
            _automaticTransmission = new AutomaticTransmission(automaticTransmissionConfigSO);
            _engine.Start();
        }
        
        private void OnEnable()
        {
            _gasInputEventBinding = new EventBinding<ThrottleInputEvent>(OnThrottleInputChangeEvent);
            EventBus<ThrottleInputEvent>.Register(_gasInputEventBinding);
            
            _brakeInputEventBinding = new EventBinding<BrakeInputEvent>(OnBrakeInputChangeEvent);
            EventBus<BrakeInputEvent>.Register(_brakeInputEventBinding);

            _raceStartEventBinding = new EventBinding<RaceStartEvent>(OnRaceStartEvent);
            EventBus<RaceStartEvent>.Register(_raceStartEventBinding);
        }


        private void OnDisable()
        {
            EventBus<ThrottleInputEvent>.Deregister(_gasInputEventBinding);
            EventBus<BrakeInputEvent>.Deregister(_brakeInputEventBinding);
        }

        private void OnBrakeInputChangeEvent(BrakeInputEvent @event)
        {
            if (!_engine.IsRunning)
            {
                _activeBreakInput = 0;
                return;
            }
            
            _activeBreakInput = @event.Value;
        }

        private void OnThrottleInputChangeEvent(ThrottleInputEvent @event)
        {
            if (_automaticTransmission.IsShifting || !_engine.IsRunning)
            {
                _activeThrottleInput = 0;
                return;
            }

            _activeThrottleInput = @event.Value;
        }

        private void OnRaceStartEvent(RaceStartEvent @event)
        {
            _automaticTransmission.ShiftUp();
        }
        
        private void FixedUpdate()
        {
            UpdateTransmission();
            ApplyEnginePower();
            UpdateEngineSoundPlayer();
            RaiseCarUpdateEvent();
        }

        private void UpdateEngineSoundPlayer()
        {
            engineSoundPlayer.UpdateEngineState(_engine.CurrentRPM, _engine.IsRunning, _activeThrottleInput, _activeBreakInput);
        }

        private void RaiseCarUpdateEvent()
        {
            EventBus<PlayerCarUpdateEvent>.Raise(new PlayerCarUpdateEvent()
            {
                RPM = _engine.CurrentRPM,
                Speed = _currentSpeed,
                gearNumber = _automaticTransmission.CurrentGearIndex + 1
            });
        }

        private void UpdateTransmission()
        {
            var currentSpeedKMH = _currentSpeed;
            _automaticTransmission.HandleTransmission(currentSpeedKMH, _engine.CurrentRPM);
        }

        private void ApplyEnginePower()
        {
            var tractionRPM = CalculateTractionRPM();
            _engine.UpdateEngineRPM(tractionRPM, _activeThrottleInput, _automaticTransmission.ClutchInput,
                _automaticTransmission.CurrentGearRatio);
            DistributeTorque();
        }

        private float CalculateTractionRPM() =>
            wheelControllers.Where(wheel => wheel.canPower).Sum(wheel => Mathf.Abs(wheel.GetRPM()));

        private int GetPoweredWheelCount() => wheelControllers.Count(wheel => wheel.canPower);
        

        private void DistributeTorque()
        {
            var torquePerWheel = _engine.GetTorque(_automaticTransmission.CurrentGearRatio) / GetPoweredWheelCount();
            foreach (var wheel in wheelControllers)
            {
                if(wheel.canPower)
                    wheel.ApplyMotorTorque(torquePerWheel * _activeThrottleInput * (1 - _automaticTransmission.ClutchInput));
                
                wheel.ApplyBrakeTorque(BRAKE_TORQUE * _activeBreakInput);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish")) 
                EventBus<RaceEndEvent>.Raise(new RaceEndEvent());
        }
    }
}