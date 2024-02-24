using ProjectDrive.EventBus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectDrive.UI
{
    public abstract class VehicleInputButton<T> : MonoBehaviour, IPointerDownHandler, IPointerUpHandler where T : InputEvent, 
         new()
    {
        [SerializeField] private float increaseSpeed;
        [SerializeField] private float decaySpeed;
        
        private T _event;

        private float _value;
        private float _targetValue; 
        
        public void OnPointerUp(PointerEventData _) => _targetValue = 0f;

        public void OnPointerDown(PointerEventData _) => _targetValue = 1f;

        private float CalculateInputSpeed(float input) => input == 1f ? increaseSpeed : decaySpeed;
        
        private void Update()
        {
            _value = Mathf.MoveTowards(_value, _targetValue, CalculateInputSpeed(_targetValue) * Time.deltaTime);
            EventBus<T>.Raise(new T()
            {
                Value = _value
            });
        }
    }
}