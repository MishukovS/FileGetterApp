using FileGetterApp.Business.Interfases;
using System.Text;
using System.Threading;

namespace FileGetterApp.Business.Servises
{
    public class LongRunningReader : ILongRunningReader
    {
        public byte[] Read(string fileName)
        {
            Thread.Sleep(2000);
            var text = "Тут много букв...";
            return Encoding.UTF8.GetBytes(text);
        }

    }
}
