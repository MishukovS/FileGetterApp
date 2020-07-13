using FileGetterApp.Business.Interfases;
using System.Text;
using System.Threading.Tasks;

namespace FileGetterApp.Business.Servises
{
    public class LongRunningReader : ILongRunningReader
    {
        public async Task<byte[]> ReadAsync(string fileName)
        {
            await Task.Delay(2000);
            var text = "Тут много букв...";
            return Encoding.UTF8.GetBytes(text);
        }

    }
}
