using FileGetterApp.Business.Interfases;
using FileGetterApp.Business.Servises;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileGetterAppTests.Business
{
    public class FileGetterWithQueueTests
    {

        private IFileGetterWithQueue fileGetter;
        private Mock<ILongRunningReader> readerMock;

        [SetUp]
        public void Setup()
        {
            readerMock = new Mock<ILongRunningReader>();
            readerMock
                .Setup(x => x.ReadAsync(It.IsAny<string>()))
                .Returns(async () =>
                {
                    await Task.Delay(500);
                    return new byte[1];
                });
            fileGetter = new FileGetterWithQueue(readerMock.Object);
        }

        [Test]
        public async Task Get_WhenEqualName_ThenReadOnce()
        {
            var names = new List<string> { "1", "1", "1", "1", "1" };
            var tasks = names.Select(x => fileGetter.GetAsync(x)).ToList();
            await Task.WhenAll();

            readerMock.Verify(x => x.ReadAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Get_WhenDifferentName_ThenEverytime()
        {
            var names = new List<string> { "1", "2", "3", "4", "5" };
            var tasks = names.Select(x => fileGetter.GetAsync(x)).ToList();
            await Task.WhenAll();

            readerMock.Verify(x => x.ReadAsync(It.IsAny<string>()), Times.AtLeast(5));
        }

        [Test]
        public async Task Get_WhenDifferentAndEqualName_ThenForEachUniqueName()
        {
            var names = new List<string> { "1", "2", "1", "2", "3" };
            var tasks = names.Select(x => fileGetter.GetAsync(x)).ToList();
            await Task.WhenAll();

            readerMock.Verify(x => x.ReadAsync(It.IsAny<string>()), Times.AtLeast(3));
        }
    }
}
