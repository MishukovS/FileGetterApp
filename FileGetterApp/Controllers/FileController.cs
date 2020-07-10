using FileGetterApp.Business.Interfases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TestFileReaderApp.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string contentType = "text/plain";
        private readonly IFileGetter fileGetter;
        private readonly IFileGetterWithCache fileGetterWithCache;

        public FileController(IFileGetter fileGetter, IFileGetterWithCache fileGetterWithCache)
        {
            this.fileGetter = fileGetter;
            this.fileGetterWithCache = fileGetterWithCache;
        }

        /// <summary>
        /// Получает данные из файла, если файл в данный момент читается то берется результат чтения из другого потока 
        /// </summary>       
        [HttpGet("Get")]
        public IActionResult Get(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return BadRequest("filename is empty");
            }

            var fileData = fileGetterWithCache.Get(filename);
            return File(fileData, contentType, filename);
        }

        /// <summary>
        /// Получает данные из файла, без конкурентного доступа к одному и тому же файлу
        /// </summary>       
        [HttpGet("GetSeq")]
        public async Task<IActionResult> GetSeqAsync(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return BadRequest("filename is empty");
            }

            var fileData = await fileGetter.GetAsync(filename);
            return File(fileData, contentType, filename);
        }
    }
}
