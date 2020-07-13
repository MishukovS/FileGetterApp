using System.Threading.Tasks;

namespace FileGetterApp.Business.Interfases
{
    public interface IFileGetterWithQueue
    {
        Task<byte[]> GetAsync(string fileName);
    }
}
