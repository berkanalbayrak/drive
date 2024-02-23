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

        [Header("Speed Display Settings")]
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI gearText;

        // Update is called once per frame
        void Update()
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
        
        public void UpdateRPM(float rpm)
        {
            currentRPM = rpm;
        }
        
        public void UpdateSpeed(float speed)
        {
            currentSpeed = speed;
        }

        public void UpdateGearText(int gear)
        {
            gearText.text = gear.ToString();
        }
    }
}