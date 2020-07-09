using System;

namespace FileGetterApp.Infrastructure.Models
{
    public class LockSettings
    {
        public LockSettings(int attemptCount, TimeSpan delay)
        {
            AttemptCount = attemptCount;
            Delay = delay;
        }


        /// <summary> Количество попыток</summary>
        public int AttemptCount { get; }

        /// <summary> Время, через которое осуществлять повторные попытки</summary>
        public TimeSpan Delay { get; }
    }
}
