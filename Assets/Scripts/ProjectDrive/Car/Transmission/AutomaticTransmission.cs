using System.Linq;
using Cysharp.Threading.Tasks;
using ProjectDrive.Car.Transmission.Config;

namespace ProjectDrive.Car.Transmission
{
    public class AutomaticTransmission
    {
        public float ClutchInput { get; private set; } = 0f;
        public int CurrentGearIndex { get; private set; }
        public bool IsShifting { get; private set; }
        
        public float CurrentGearRatio => _gears[CurrentGearIndex].ratio;
        
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
            CurrentGearIndex = 0;
        }

        public void HandleTransmission(float carSpeed, float engineRPM)
        {
            if (ShouldShiftUp(carSpeed, engineRPM))
            {
                ShiftGear(CurrentGearIndex + 1).Forget();
            }
            else if (ShouldShiftDown(carSpeed, engineRPM))
            {
                ShiftGear(CurrentGearIndex - 1).Forget();
            }
        }

        private bool ShouldShiftUp(float carSpeed, float engineRPM)
        {
            return CurrentGearIndex < _gears.Length - 1 
                   && !IsShifting 
                   && carSpeed >= _gears[CurrentGearIndex].targetSpeed 
                   && engineRPM >= _gearShiftUpRpm;
        }

        private bool ShouldShiftDown(float carSpeed, float engineRPM)
        {
            return CurrentGearIndex > 0 
                   && !IsShifting 
                   && carSpeed < _gears[CurrentGearIndex - 1].targetSpeed 
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