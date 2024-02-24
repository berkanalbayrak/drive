using System.Linq;
using Cysharp.Threading.Tasks;
using ProjectDrive.Car.Transmission.Config;
using UnityEngine;

namespace ProjectDrive.Car.Transmission
{
    public class AutomaticTransmission
    {
        public float ClutchInput { get; private set; } = 1f;
        public int CurrentGearIndex { get; private set; }
        public bool IsShifting { get; private set; }
        
        public float CurrentGearRatio => CurrentGearIndex >= 0 ? _gears[CurrentGearIndex].ratio : 0f;
        
        public float TargetSpeed => CurrentGearIndex >= 0 ? _gears[CurrentGearIndex].targetSpeed : Mathf.Infinity;
        
        private readonly Gear[] _gears;
        private readonly float _gearShiftingDelay;
        private readonly float _gearShiftUpRpm;
        private readonly float _gearShiftDownRpm;
        
        public AutomaticTransmission(AutomaticTransmissionConfigSO config)
        {
            _gears = config.gears;
            _gearShiftingDelay = config.gearShiftingDelay;
            _gearShiftUpRpm = config.gearShiftUpRPM;
            _gearShiftDownRpm = config.gearShiftDownRPM;
            CurrentGearIndex = -1;
        }

        public void HandleTransmission(float carSpeed, float engineRPM)
        {
            if (ShouldShiftUp(carSpeed, engineRPM))
            {
                ShiftUp();
            }
            else if (ShouldShiftDown(carSpeed, engineRPM))
            {
                ShiftDown();
            }
        }
        
        public void ShiftUp() => ShiftGear(CurrentGearIndex + 1).Forget();
        private void ShiftDown() => ShiftGear(CurrentGearIndex - 1).Forget();

        
        private bool ShouldShiftUp(float carSpeed, float engineRPM)
        {
            return CurrentGearIndex < _gears.Length - 1 
                   && !IsShifting 
                   && carSpeed >= TargetSpeed
                   && engineRPM >= _gearShiftUpRpm;
        }

        private bool ShouldShiftDown(float carSpeed, float engineRPM)
        {
            return CurrentGearIndex > 0 
                   && !IsShifting 
                   && carSpeed < TargetSpeed 
                   && engineRPM <= _gearShiftDownRpm;
        }


        private async UniTask ShiftGear(int targetGearIndex)
        {
            IsShifting = true;
            ClutchInput = 1; 

            await UniTask.Delay((int)(_gearShiftingDelay * 1000)); // Convert seconds to milliseconds.

            CurrentGearIndex = targetGearIndex;
            ClutchInput = 0; 

            IsShifting = false;
        }
    }
}