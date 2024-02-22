namespace ProjectDrive.Vehicle.Engine.FuelInjector
{
    public class FuelInjector : IFuelInjector
    {
        public bool IsInjecting { get; private set; }
        
        public void InjectFuel(float amount)
        {
            IsInjecting = amount > 0;
        }
    }
}