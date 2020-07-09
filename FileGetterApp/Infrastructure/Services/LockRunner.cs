using FileGetterApp.Infrastructure.Interfaces;
using FileGetterApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileGetterApp.Infrastructure.Services
{
    public class LockRunner : ILockRunner
    {
        private readonly HashSet<string> lockSet = new HashSet<string>();
        private readonly object locker = new object();
        private readonly LockSettings defaultSetting = new LockSettings(1000, TimeSpan.FromMilliseconds(100));

        public async Task<T> LockRunAsync<T>(string key, Func<Task<T>> func, LockSettings setting = null)
        {
            setting = setting ?? defaultSetting; 

            var count = 0;

            while (count < setting.AttemptCount)
            {
                var isKeyFree = true;
                lock (locker)
                {
                    isKeyFree = !lockSet.Contains(key);

                }
                if (isKeyFree)
                {
                    T result;
                    lockSet.Add(key);
                    try
                    {
                        result = await func.Invoke().ConfigureAwait(false);
                    }
                    finally
                    {
                        lock (locker)
                        {
                            lockSet.Remove(key);
                        }
                    }

                    return result;

                }


                await Task.Delay(setting.Delay).ConfigureAwait(false);
                count++;
            }



            return default(T);
        }
    }
}
