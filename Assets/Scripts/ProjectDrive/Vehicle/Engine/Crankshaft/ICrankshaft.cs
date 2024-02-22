namespace ProjectDrive.Vehicle.Engine.Crankshaft
{
    public interface ICrankshaft
    {
        public float RotationAngle { get; }
        public void Rotate(float amount);
    }
}