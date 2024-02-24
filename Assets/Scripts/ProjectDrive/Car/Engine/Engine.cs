using System;
using ProjectDrive.Car.Engine.Config;
using UnityEngine;

namespace ProjectDrive.Car.Engine
{
    public class Engine : IEngine
    {
        public float IdleRPM { get; private set; } 
        
        public float MaxRPM { get; private set; } 
        public float CurrentRPM { get; private set; }
        public bool IsRunning { get; private set; }
        
        private readonly AnimationCurve _torqueCurve; // Curve representing the torque output at different RPMs.
        private readonly float _inertia; // Inertia of the engine, affecting how quickly it can change RPM.
        private readonly float _finalDriveRatio; // Final drive ratio, part of the vehicle's drivetrain.
        
        private float _dampingRPM; // Used for smoothing RPM change.
        private float _velocity; // used in SmoothDamp function.
        
        public event Action<float, bool> OnEngineStatusChanged;
        
        public Engine(EngineConfigSO engineConfigSO)
        {
            _torqueCurve = engineConfigSO.TorqueCurve;
            MaxRPM = engineConfigSO.MaxRPM;
            IdleRPM = engineConfigSO.IdleRPM;
            _inertia = engineConfigSO.Inertia;
            _finalDriveRatio = engineConfigSO.FinalDriveRatio;
            Start();
            _dampingRPM = 0;
        }

        public void Start()
        {
            if (IsRunning) return;
            CurrentRPM = IdleRPM;
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
        }

        // Get the engine's torque output at the current RPM, modified by the current gear's max ratio.
        public float GetTorque(float currentGearMaxRatio)
        {
            return _torqueCurve.Evaluate(CurrentRPM) * currentGearMaxRatio * _finalDriveRatio;
        }

        public void UpdateEngineRPM(float tractionRPM, float throttleInput, float clutchInput, float currentGearMax)
        {
            // Calculate the effective inertia based on clutch input.
            var effectiveInertia = CalculateEffectiveInertia(clutchInput);
    
            // Determine the target RPM based on current conditions.
            var targetRPM = CalculateTargetRPM(tractionRPM, throttleInput, clutchInput, currentGearMax);
    
            // Smoothly transition the 'dampingRPM' towards the 'targetRPM', simulating inertia effect.
            _dampingRPM = Mathf.SmoothDamp(_dampingRPM, targetRPM, ref _velocity, effectiveInertia);
    
            // Smoothly update the 'CurrentRPM' to match the 'dampingRPM', simulating engine response.
            CurrentRPM = Mathf.Lerp(CurrentRPM, _dampingRPM, Time.fixedDeltaTime * Mathf.Clamp(1f - clutchInput, .25f, 1f) * 25f);
            
            OnEngineStatusChanged?.Invoke(CurrentRPM, IsRunning);
        }
        
        private float CalculateEffectiveInertia(float clutchInput)
        {
            var clutchEffect = clutchInput / 5f * _inertia;
            var clutchEffectModifier = Mathf.Lerp(1f, Mathf.Clamp(clutchInput, .75f, 1f), CurrentRPM / MaxRPM);
            return _inertia + clutchEffect * clutchEffectModifier;
        }
        
        private float CalculateTargetRPM(float tractionRPM, float throttleInput, float clutchInput, float gearRatio)
        {
            const float tractionFactor = 2f; 
    
            // Contributions to the target RPM from engine input and traction.
            var engineRPMContribution = Mathf.Lerp(IdleRPM, MaxRPM, throttleInput * clutchInput);
            var tractionContribution = (tractionRPM / tractionFactor) * _finalDriveRatio * gearRatio * (1f - clutchInput);
    
            // The resulting target RPM is the sum of contributions, clamped to the engine's operational range.
            return Mathf.Clamp(engineRPMContribution + tractionContribution, 0f, MaxRPM);
        }

        
        private float CalculateTractionContribution(float tractionRPM, float gearRatio, float clutchInput)
        {
            const float TractionFactor = 2f;
            return (tractionRPM / TractionFactor) * _finalDriveRatio * gearRatio * (1f - clutchInput);
        }
    }
}