using System.Threading.Tasks;

namespace FileGetterApp.Business.Interfases
{
    public interface IFileGetter
    {
        Task<byte[]> GetAsync(string fileName);
    }
}
