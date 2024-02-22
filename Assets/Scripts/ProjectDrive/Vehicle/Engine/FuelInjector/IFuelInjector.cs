public interface IFuelInjector
{
    public bool IsInjecting { get; }    
    public void InjectFuel(float amount);
}
