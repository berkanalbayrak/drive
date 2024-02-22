using UnityEngine;

namespace ProjectDrive.Vehicle.Engine.Piston
{
    public class Piston : IPiston
    {
        public float Position { get; private set; }
        
        public void Move(float cycleProgress)
        {
            //Simplified logic
            Position = Mathf.Sin(cycleProgress * Mathf.PI * 2);
        }
    }
}