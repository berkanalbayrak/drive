using UnityEngine;

namespace ProjectDrive.Car.Transmission.Config
{
    [CreateAssetMenu(fileName = "New Transmission Config", menuName = "CarComponents/Transmission")]
    public class AutomaticTransmissionConfigSO : ScriptableObject
    {
        public float gearShiftUpRPM = 6500f;
        public float gearShiftDownRPM = 3500f;
        public float gearShiftingDelay = 0.35f;
        public Gear[] gears;
    }
}