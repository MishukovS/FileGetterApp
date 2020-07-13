using System.Threading.Tasks;

namespace FileGetterApp.Business.Interfases
{
    public interface ILongRunningReader
    {
        Task<byte[]> ReadAsync(string fileName);
    }
}
