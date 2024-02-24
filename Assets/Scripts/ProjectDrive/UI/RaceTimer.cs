using ProjectDrive.EventBus;
using ProjectDrive.Utils;
using TMPro;
using UnityEngine;

namespace ProjectDrive.UI
{
    public class RaceTimer : MonoBehaviour
    {
        public GameObject timerTextContainer;
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI bestTimeText;
        private float startTime;
        private bool isRunning = false;

        private EventBinding<RaceStartEvent> _raceStartEventBinding;
        private EventBinding<RaceEndEvent> _raceEndEventBinding;

        private void OnEnable()
        {
            _raceStartEventBinding = new EventBinding<RaceStartEvent>(OnRaceStartEvent);
            EventBus<RaceStartEvent>.Register(_raceStartEventBinding);
            
            _raceEndEventBinding = new EventBinding<RaceEndEvent>(OnRaceEndEvent);
            EventBus<RaceEndEvent>.Register(_raceEndEventBinding);
            
            Hide();
        }

        private void OnRaceEndEvent(RaceEndEvent @event)
        {
            StopTimer();
            var raceTimeSeconds = Time.time - startTime;
            if (raceTimeSeconds < PlayerPrefs.GetFloat("BestTime", Mathf.Infinity))
            {
                PlayerPrefs.SetFloat("BestTime", raceTimeSeconds);
                UpdateBestTime();
            }
        }

        private void OnRaceStartEvent(RaceStartEvent @event)
        {
            StartTimer();
        }

        public void Update()
        {
            if (isRunning)
            {
                var currentTime = Time.time - startTime;
                var formattedTime = TimeFormatUtils.ToRaceTime(currentTime);
                timerText.text = formattedTime;
                
            }
        }

        private void StartTimer()
        {
            if (!isRunning)
            {
                UpdateBestTime();

                startTime = Time.time;
                isRunning = true;
            }
        }

        private void Hide()
        {
            timerText.text = string.Empty;
            bestTimeText.text = string.Empty;
        }

        private void UpdateBestTime()
        {
            var bestTime = PlayerPrefs.GetFloat("BestTime", Mathf.Infinity);
            
            if(bestTime == Mathf.Infinity)
                bestTimeText.text = "Best : --:--:---";
            else
                bestTimeText.text = "Best : " + TimeFormatUtils.ToRaceTime(bestTime);
        }

        public void StopTimer() => isRunning = false;

        public void ResetTimer() => startTime = Time.time;

        
    }
}