namespace ProjectDrive.Vehicle.Engine.Piston
{
    public interface IPiston
    {
        public float Position { get; }
        public void Move(float cycleProgress);
    }
}