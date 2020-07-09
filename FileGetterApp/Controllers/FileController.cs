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

        public FileController(IFileGetter fileGetter)
        {
            this.fileGetter = fileGetter;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAsync(string filename)
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
