using UnityEngine;

namespace ProjectDrive.Car.Engine.Config
{
    [CreateAssetMenu(fileName = "New Engine Config", menuName = "CarComponents/Engine")]
    public class EngineConfigSO : ScriptableObject
    {
        public AnimationCurve TorqueCurve;
        public float IdleRPM;
        public float MaxRPM;
        public float Inertia;
        public float FinalDriveRatio = 3.23f;
    }
}