using System;
using ProjectDrive.EventBus;
using TMPro;
using UnityEngine;

namespace ProjectDrive.UI
{
    public class RPMGaugeController : MonoBehaviour
    {
        [Header("RPM Gauge Settings")]
        public RectTransform needleTransform;
        public float maxRPM = 8000f;
        public float currentRPM = 0f;
        public float currentSpeed = 0f;
        private const float MIN_ANGLE = 180f;
        private const float MAX_ANGLE = -80;

        [Header("Text References")]
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI gearText;

        private EventBinding<PlayerCarUpdateEvent> _playerCarUpdateEventBinding;

        private void OnEnable()
        {
            _playerCarUpdateEventBinding = new EventBinding<PlayerCarUpdateEvent>(OnPlayerCarUpdate);
            EventBus<PlayerCarUpdateEvent>.Register(_playerCarUpdateEventBinding);
        }

        private void OnPlayerCarUpdate(PlayerCarUpdateEvent @event)
        {
            UpdateSpeed(@event.Speed);
            UpdateRPM(@event.RPM);
            UpdateGearText(@event.gearNumber);
        }

        private void OnDisable()
        {
            EventBus<PlayerCarUpdateEvent>.Deregister(_playerCarUpdateEventBinding);
        }

        private void Update()
        {
            currentRPM = Mathf.Clamp(currentRPM, 0, maxRPM);

            var rpmFraction = currentRPM / maxRPM;
            var needleAngle = Mathf.Lerp(MIN_ANGLE, MAX_ANGLE, rpmFraction);
            needleTransform.localEulerAngles = new Vector3(0, 0, needleAngle);

            speedText.text = $"{currentSpeed:N0} km/h";

            if (currentRPM >= maxRPM)
            {
                currentRPM = 0;
                currentSpeed = 0;
            }
        }

        private void UpdateRPM(float rpm)
        {
            currentRPM = rpm;
        }

        private void UpdateSpeed(float speed)
        {
            currentSpeed = speed;
        }

        private void UpdateGearText(int gearNumber)
        {
            gearText.text = gearNumber != 0 ? gearNumber.ToString() : "N";
        }
    }
}