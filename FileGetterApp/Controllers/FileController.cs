using FileGetterApp.Business.Interfases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TestFileReaderApp.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string contentType = "text/plain";
        private readonly IFileGetterWithQueue fileGetterWithQueue;
        private readonly IFileGetterWithCache fileGetterWithCache;

        public FileController(IFileGetterWithQueue fileGetterWithQueue, IFileGetterWithCache fileGetterWithCache)
        {
            this.fileGetterWithQueue = fileGetterWithQueue;
            this.fileGetterWithCache = fileGetterWithCache;
        }

        /// <summary>
        /// Получает данные из файла, если файл в данный момент читается то берется результат чтения из другого потока
        /// используется кэш с ленивой инициализацией
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
        /// Получает данные из файла, если файл в данный момент читается то берется результат чтения из другого потока
        /// используется Dataflow
        /// </summary>        
        [HttpGet("GetAsync")]
        public async Task<IActionResult> GetAsync(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return BadRequest("filename is empty");
            }

            var fileData = await fileGetterWithQueue.GetAsync(filename);
            return File(fileData, contentType, filename);
        }
    }
}
