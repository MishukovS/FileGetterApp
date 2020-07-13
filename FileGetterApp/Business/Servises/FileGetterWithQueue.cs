using FileGetterApp.Business.Interfases;
using FileGetterApp.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FileGetterApp.Business.Servises
{
    public class FileGetterWithQueue : IFileGetterWithQueue
    {
        private readonly IList<string> readingFiles = new List<string>();
        private readonly object locker = new object();
        private readonly BufferBlock<FileData> dataQueue = new BufferBlock<FileData>();
        private readonly ILongRunningReader fileReader;

        public FileGetterWithQueue(ILongRunningReader fileReader)
        {
            this.fileReader = fileReader;
        }

        public async Task<byte[]> GetAsync(string fileName)
        {
            bool isReading = false;
            lock (locker)
            {
                isReading = readingFiles.Contains(fileName);
            }
            if (isReading)
            {
                return await GetReadingAsync(fileName);

            }

            return await ReadAndSendToQueueAsync(fileName);

        }


        private async Task<byte[]> ReadAndSendToQueueAsync(string fileName)
        {
            lock (locker)
            {
                readingFiles.Add(fileName);
            }

            var data = await fileReader.ReadAsync(fileName);

            await dataQueue.SendAsync(new FileData { Name = fileName, Data = data });

            lock (locker)
            {
                readingFiles.Remove(fileName);
            }
            return data;
        }

        private async Task<byte[]> GetReadingAsync(string fileName)
        {

            bool isDataReceived = false;
            FileData result = new FileData();
            while (!isDataReceived)
            {
                result = await dataQueue.ReceiveAsync();
                isDataReceived = result.Name == fileName;
            }

            lock (locker)
            {
                readingFiles.Remove(fileName);
            }

            return result.Data;
        }
    }
}
