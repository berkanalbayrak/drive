using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectDrive.EventBus;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ProjectDrive.UI
{
    public class RaceCountdown : MonoBehaviour
    {
        [FormerlySerializedAs("countDownImageHiddenTransform")] [SerializeField] private RectTransform countdownImageHiddenTransform;
        [SerializeField] private Image countdownImage; 
        [SerializeField] private Sprite[] countdownSprites; // Array of Sprites for countdown.

        [Header("Tween Settings")]
        [SerializeField] private float showDuration;
        [SerializeField] private Ease showEase;
        
        [SerializeField] private float hideDuration;
        [SerializeField] private Ease hideEase;
        
        private EventBinding<StartCountdownEvent> _startCountdownEventBinding;

        private Vector2 _countDownImageShownPosition;
        
        private void Start()
        {
            _countDownImageShownPosition = countdownImage.transform.position;
            countdownImage.transform.position = countdownImageHiddenTransform.position; 
        }

        private void OnEnable()
        {
            _startCountdownEventBinding = new EventBinding<StartCountdownEvent>(Show);
            EventBus<StartCountdownEvent>.Register(_startCountdownEventBinding);
        }

        private void OnDisable()
        {
            EventBus<StartCountdownEvent>.Deregister(_startCountdownEventBinding);
        }

        private async UniTaskVoid BeginCountDown()
        {
            for (var i = 0; i < countdownSprites.Length; i++)
            {
                countdownImage.sprite = countdownSprites[i];
                if(i != countdownSprites.Length - 1)
                    await UniTask.Delay(1000);
            }
            
            EventBus<RaceStartEvent>.Raise(new RaceStartEvent());
            Hide();
        }

        private void Hide()
        {
            countdownImage.transform.DOMove(countdownImageHiddenTransform.position, hideDuration).SetEase(hideEase);
        }

        private void Show()
        {
            countdownImage.transform.DOMove(_countDownImageShownPosition, showDuration).SetEase(showEase)
                .OnComplete(() => BeginCountDown().Forget());
        }

    }
}