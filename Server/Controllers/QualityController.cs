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
        /// Получить все файлы с папки
        /// </summary>
        [HttpGet("GetFiles/{path}")]
        public ActionResult<List<FileModel>> GetFiles(string path)
        {
            return _service.GetFiles(path);
        }


        /// <summary>
        /// Удалить файл
        /// </summary>
        [HttpPost("DeleteFile")]
        public IActionResult DeleteFile(FileModel file)
        {
            try
            {
                _service.DeleteFile(file);
                return Ok("Файлы удаленны");
            }
            catch (Exception ex)
            {
                return BadRequest("Не удалось удалить файл, причина: " + ex.Message);
            }
        }


        /// <summary>
        /// Загрузить файлы на сервер
        /// </summary>
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost("UploadFiles/{path}")]
        public async Task<IActionResult> UploadFiles(string path, [FromBody] SaveFile saveFile)
        {
            var fileExists = await _service.UploadFiles(path, saveFile);
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
        [HttpGet("DownloadFile/{path}")]
        public async Task<IActionResult> DownloadFile(string path)
        {
            try
            {
                // Расшифровывает путь
                path = Cripto.EncodeDecrypt(path);

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
