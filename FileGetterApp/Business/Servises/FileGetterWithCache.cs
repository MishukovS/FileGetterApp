using FileGetterApp.Business.Interfases;
using System;
using System.Collections.Concurrent;

namespace FileGetterApp.Business.Servises
{
    public class FileGetterWithCache : IFileGetterWithCache
    {
        private readonly ILongRunningReader fileReader;

        private readonly ConcurrentDictionary<string, int> readers = new ConcurrentDictionary<string, int>();
        private readonly ConcurrentDictionary<string, Lazy<byte[]>> cache = new ConcurrentDictionary<string, Lazy<byte[]>>();

        public FileGetterWithCache(ILongRunningReader fileReader)
        {
            this.fileReader = fileReader;
        }

        public byte[] Get(string fileName)
        {            
            readers.AddOrUpdate(fileName, 1, (k, v) => v++);
            var result = cache.GetOrAdd(fileName, id => new Lazy<byte[]>(() => ReadFile(id))).Value;
            readers.AddOrUpdate(fileName, 0, (k, v) => v--);

            bool isRemove = !readers.TryGetValue(fileName, out int count) || count <= 0;
            if (isRemove)
            {
                cache.TryRemove(fileName, out var str);
            }
            return result;
        }

        private byte[] ReadFile(string id)
        {
            var data = fileReader.ReadAsync(id).GetAwaiter().GetResult();
            return data;
        }
    }
}
