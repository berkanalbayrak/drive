namespace ProjectDrive.Car.Engine
{
    public interface IEngine
    {
        public bool IsRunning { get; }
        public void Start();
        public void Stop();
    }
}
