using ProjectDrive.Vehicle.Engine.Crankshaft;
using ProjectDrive.Vehicle.Engine.Piston;
using UnityEngine;

namespace ProjectDrive.Vehicle.Engine
{
    public class Engine : IEngine
    {
        private IPiston piston; //Single piston for simplicity
        private ICrankshaft crankshaft;
        private IFuelInjector fuelInjector;
        private bool isRunning = false;
        private float powerLevel = 0.0f; // 0.0f (no power) to 1.0f (full power)
        private float responsivenessFactor = 0.5f; //To simulate different throttle responsiveness

        public bool IsRunning => isRunning;

        public Engine(IPiston piston, ICrankshaft crankshaft, IFuelInjector fuelInjector)
        {
            this.piston = piston;
            this.crankshaft = crankshaft;
            this.fuelInjector = fuelInjector;
        }

        public void Start()
        {
            if (isRunning) return;
            
            isRunning = true;
            fuelInjector.InjectFuel(0.1f); // Small amount to simulate fuel priming
        }

        public void Stop()
        {
            if (!isRunning) return;
            
            isRunning = false;
            powerLevel = 0.0f;
            fuelInjector.InjectFuel(0f); // Stop fuel injection
        }

        public void IncreasePower(float amount)
        {
            if (!isRunning) return;
            
            powerLevel = Mathf.Clamp(powerLevel + amount, 0.0f, 1.0f);
        }

        public void DecreasePower(float amount)
        {
            if (!isRunning) return;
            
            powerLevel = Mathf.Clamp(powerLevel - amount, 0.0f, 1.0f);
        }

        public void AdjustPowerBasedOnPedal(float pedalPercentage)
        {
            var targetPowerLevel = pedalPercentage;
            var powerAdjustment = (targetPowerLevel - powerLevel) * responsivenessFactor;
            powerLevel += powerAdjustment;
            powerLevel = Mathf.Clamp(powerLevel, 0.0f, 1.0f);

            // Simulate the effects of the current power level on the engine components
            SimulateEngineCycle(powerLevel);
        }

        private void SimulateEngineCycle(float currentPowerLevel)
        {
            // For simplicity, assumed the power level directly affects fuel injection rate.
            
            fuelInjector.InjectFuel(currentPowerLevel * 1.0f); // Simulate fuel injection based on power level
            
            piston.Move(currentPowerLevel); // Simplified piston movement
            crankshaft.Rotate(currentPowerLevel * 360.0f); // Simplified crankshaft rotation
        }
    }
}