using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WorkingWithFiles.Server.Service;
using WorkingWithFiles.Shared;
using WorkingWithFiles.Shared.Model;

namespace WorkingWithFiles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QualityController : ControllerBase
    {
        private readonly FileManagerService _service;

        public QualityController(FileManagerService service)
        {
            _service = service;
        }


        /// <summary>
        /// Получить все файлы
        /// </summary>
        [HttpGet("GetFiles")]
        public ActionResult<List<string>> GetFiles()
        {
            return _service.GetFiles();
        }


        /// <summary>
        /// Удалить файлы
        /// </summary>
        [HttpPost("DeleteFile")]
        public IActionResult DeleteFile([FromBody] string filename)
        {
            try
            {
                _service.DeleteFile(filename);
                return Ok("Файлы удаленны");
            }
            catch (Exception ex)
            {
                return BadRequest("Не удалось удалить файл, причина: " + ex.Message);
            }
        }


        /// <summary>
        /// Загрузить файлы
        /// </summary>
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFiles([FromBody] List<FileData> saveFile)
        {
            var fileExists = await _service.UploadFiles(saveFile);
            if (fileExists.Count == 0)
            {
                return Ok();
            }
            else
            {
                return Conflict("Файлы: " + string.Join(" ", fileExists) + " уже существуют в каталоге, сначала удалите предыдущие версии");
            }
        }

        /// <summary>
        /// Скачать файл
        /// </summary>
        [HttpGet("DownloadFile/{filename}")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            try
            {
                var path = PathSelect.FOLDER_PATH[^1] == '\\' ? PathSelect.FOLDER_PATH : PathSelect.FOLDER_PATH + '\\';
                path = path + filename;

                var bytes = await System.IO.File.ReadAllBytesAsync(path);
                return File(bytes, "text/plain", Path.GetFileName(path));
            }
            catch (FileNotFoundException)
            {
                return BadRequest("Файл не найден");
            }
            catch (Exception)
            {
                return BadRequest("Не удалось скачать файл");
            }
        }
    }
}
