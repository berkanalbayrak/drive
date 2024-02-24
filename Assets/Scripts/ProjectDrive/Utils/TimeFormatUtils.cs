using System;

namespace ProjectDrive.Utils
{
    public static class TimeFormatUtils
    {
        public static string ToRaceTime(float timeInSeconds)
        {
            var time = TimeSpan.FromSeconds(timeInSeconds);

            var minutes = time.Minutes;
            var seconds = time.Seconds;
            var milliseconds = time.Milliseconds;

            return $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
        }
    }
}