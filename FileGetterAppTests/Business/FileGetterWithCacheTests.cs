using FileGetterApp.Business.Interfases;
using FileGetterApp.Business.Servises;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FileGetterAppTests.Business
{
    public class FileGetterWithCacheTests
    {
        private IFileGetterWithCache fileGetterWithCache;
        private Mock<ILongRunningReader> readerMock;

        [SetUp]
        public void Setup()
        {
            readerMock = new Mock<ILongRunningReader>();
            fileGetterWithCache = new FileGetterWithCache(readerMock.Object);
        }

        [Test]
        public void Get_WhenEqualName_ThenReadOnce()
        {
            var names = new List<string> { "1", "1", "1", "1", "1" };
            var threads = names.Select(x => new Thread(() => fileGetterWithCache.Get(x))).ToList();
            threads.ForEach(x => x.Start());

            readerMock.Verify(x => x.ReadAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Get_WhenDifferentName_ThenEverytime()
        {
            var names = new List<string> { "1", "2", "3", "4", "5" };
            var threads = names.Select(x => new Thread(() => fileGetterWithCache.Get(x))).ToList();
            threads.ForEach(x => x.Start());

            readerMock.Verify(x => x.ReadAsync(It.IsAny<string>()), Times.AtLeast(5));
        }

        [Test]
        public void Get_WhenDifferentAndEqualName_ThenForEachUniqueName()
        {
            var names = new List<string> { "1", "2", "1", "2", "3" };
            var threads = names.Select(x => new Thread(() => fileGetterWithCache.Get(x))).ToList();
            threads.ForEach(x => x.Start());

            readerMock.Verify(x => x.ReadAsync(It.IsAny<string>()), Times.AtLeast(3));
        }
    }
}
