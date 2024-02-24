using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ProjectDrive.Car.Engine
{
    public class EngineSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource engineAudioSource;

        [Header("Engine Sound Clips")]
        [SerializeField] private AudioClip idleClip;
        [SerializeField] private AudioClip lowOnClip;
        [SerializeField] private AudioClip lowOffClip;
        [SerializeField] private AudioClip mediumOnClip;
        [SerializeField] private AudioClip mediumOffClip;
        [SerializeField] private AudioClip highOnClip;
        [SerializeField] private AudioClip highOffClip;
        [SerializeField] private AudioClip redlineClip;

        private Engine _engine;
        
        private float _minRPM;
        private float _maxRPM;

        private const float lowRPMMaxRange = 2000;
        private const float mediumRPMMaxRange = 4000;
        private const float highRPMMaxRange = 7000;
        
        private float _currentRpm;
        private bool _isGasOn;
        
        private float _defaultVolume;

        private void Awake()
        {
            _defaultVolume = engineAudioSource.volume;
        }

        private void Update()
        {
            UpdateEngineSound();
        }
        
        public void Initialize(Engine engine)
        {
            _engine = engine;
            _minRPM = engine.IdleRPM;
            _maxRPM = engine.MaxRPM;
        }
        
        public void UpdateEngineState(float rpm, bool carOn, float throttleInput, float brakeInput)
        {
            _currentRpm = rpm;
            _isGasOn = throttleInput > 0;
        }

        private void UpdateEngineSound()
        {
            var selectedClip = SelectClipBasedOnRPM();
            if (HasClipChanged(selectedClip))
            {
                PlayClip(selectedClip);
            }
            
            var rpmRange = GetRPMRangeForClip(selectedClip);
            
            if(selectedClip != redlineClip)
                AdjustPitchBasedOnRPM(rpmRange.Item1, rpmRange.Item2);
        }

        private AudioClip SelectClipBasedOnRPM()
        {
            if (_currentRpm <= 800) return idleClip;
            
            // if (_currentRpm < lowRPMMaxRange) return _isGasOn ? lowOnClip : lowOffClip;
            // if (_currentRpm < mediumRPMMaxRange) return _isGasOn ? mediumOnClip : mediumOffClip;
            // if (_currentRpm < highRPMMaxRange) return _isGasOn ? highOnClip : highOffClip;

            //For Testing Purposes
            if (_currentRpm < mediumRPMMaxRange) return _isGasOn ? lowOnClip : lowOffClip;
            if (_currentRpm < highRPMMaxRange) return _isGasOn ? highOnClip : highOffClip;
            //----------------------
            return redlineClip;
        }
        
        private Tuple<float, float> GetRPMRangeForClip(AudioClip clip)
        {
            // if(clip == idleClip)
            //     return new Tuple<float, float>(0, 800);
            // if(clip == lowOnClip || clip == lowOffClip)
            //     return new Tuple<float, float>(800, lowRPMMaxRange);
            // if(clip == mediumOnClip || clip == mediumOffClip)
            //     return new Tuple<float, float>(lowRPMMaxRange, mediumRPMMaxRange);
            // if(clip == highOnClip || clip == highOffClip)
            //     return new Tuple<float, float>(mediumRPMMaxRange, highRPMMaxRange);
            
            
            //For Testing Purposes
            if(clip == idleClip)
                return new Tuple<float, float>(0, 800);
            if(clip == lowOnClip || clip == lowOffClip)
                return new Tuple<float, float>(800, mediumRPMMaxRange);
            if(clip == highOnClip || clip == highOffClip)
                return new Tuple<float, float>(mediumRPMMaxRange, highRPMMaxRange);
            //----------------------

            
            return new Tuple<float, float>(highRPMMaxRange, _maxRPM);
        }

        private bool HasClipChanged(AudioClip newClip)
        {
            return engineAudioSource.clip != newClip;
        }

        private void PlayClip(AudioClip clip)
        {
            engineAudioSource.clip = clip;
            engineAudioSource.Play();
        }

        private void AdjustPitchBasedOnRPM(float minRPMRange, float maxRPMRange)
        {
            engineAudioSource.pitch = Mathf.Lerp(0.90f, 1.20f, Mathf.InverseLerp(minRPMRange, maxRPMRange, _currentRpm));
        }
    }
}