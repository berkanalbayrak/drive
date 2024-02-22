namespace ProjectDrive.Vehicle.Engine.Crankshaft
{
    public class Crankshaft : ICrankshaft
    {
        public float RotationAngle { get; private set; }
        
        public void Rotate(float amount)
        {
            RotationAngle += amount;
        }
    }
}