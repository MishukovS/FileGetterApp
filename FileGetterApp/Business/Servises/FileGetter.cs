using FileGetterApp.Business.Interfases;
using FileGetterApp.Infrastructure.Interfaces;
using System.Text;
using System.Threading.Tasks;

namespace FileGetterApp.Business.Servises
{
    public class FileGetter : IFileGetter
    {
        private readonly ILockRunner lockRunner;

        public FileGetter(ILockRunner lockRunner)
        {
            this.lockRunner = lockRunner;
        }

        public async Task<byte[]> GetAsync(string fileName)
        {
            var text = await lockRunner.LockRunAsync<string>(
                fileName,
                async () =>
                {
                    await Task.Delay(2000);
                    return "Тут много букв...";
                }
                );

            return Encoding.UTF8.GetBytes(text);
        }
    }
}
