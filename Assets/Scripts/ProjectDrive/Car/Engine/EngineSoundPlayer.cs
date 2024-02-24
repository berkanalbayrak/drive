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

        private float _minRPM;
        private float _maxRPM;

        private float _currentRpm;
        private bool _isGasOn;

        private Engine _engine;
        
        private void Update()
        {
            UpdateEngineSound();
        }
        
        public void Initialize(Engine engine)
        {
            _engine = engine;
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

            AdjustPitchBasedOnRPM();
        }

        private AudioClip SelectClipBasedOnRPM()
        {
            if (_currentRpm <= 800) return idleClip;
            if (_currentRpm < _maxRPM / 3) return _isGasOn ? lowOnClip : lowOffClip;
            if (_currentRpm < _maxRPM * 2 / 3) return _isGasOn ? mediumOnClip : mediumOffClip;
            return _isGasOn ? highOnClip : highOffClip;
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

        private void AdjustPitchBasedOnRPM()
        {
            engineAudioSource.pitch = Mathf.Lerp(0.75f, 1.25f, Mathf.InverseLerp(_engine.IdleRPM, _engine.MaxRPM, _currentRpm));
        }
    }
}