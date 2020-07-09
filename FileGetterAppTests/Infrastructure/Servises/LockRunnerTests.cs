using FileGetterApp.Infrastructure.Interfaces;
using FileGetterApp.Infrastructure.Services;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FileGetterAppTests.Infrastructure.Servises
{
    public class LockRunnerTests
    {
        private readonly ILockRunner lockRunner = new LockRunner();
        private Stopwatch stopWatch;


        [SetUp]
        public void Setup()
        {
            stopWatch = new Stopwatch();
        }

        [Test]
        public async Task LockRunAsync_WhenDifferentKey_ThenRunParallel()
        {
            var keys = new[] { "1", "2", "3" };

            var tasks = keys.Select(key => lockRunner.LockRunAsync(key, async () =>
           {
               await Task.Delay(1000);
               return true;
           })).ToArray();

            stopWatch.Start();

            await Task.WhenAll(tasks);

            stopWatch.Stop();
            var ts = stopWatch.Elapsed;

            Assert.IsTrue(ts < TimeSpan.FromMilliseconds(1200));
        }

        [Test]
        public async Task LockRunAsync_WhenEqualKey_ThenRunSeries()
        {
            var keys = new[] { "1", "1", "1" };

            var tasks = keys.Select(key => lockRunner.LockRunAsync(key, async () =>
            {
                await Task.Delay(1000);
                return true;
            })).ToArray();

            stopWatch.Start();

            await Task.WhenAll(tasks);

            stopWatch.Stop();
            var ts = stopWatch.Elapsed;

            Assert.IsTrue(ts > TimeSpan.FromMilliseconds(3000));
        }
    }
}
