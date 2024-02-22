public interface IEngine
{
    public void Start();
    public void Stop();
    public void IncreasePower(float amount);
    public void DecreasePower(float amount);
    public bool IsRunning { get; }
}
