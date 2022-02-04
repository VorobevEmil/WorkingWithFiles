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
        /// <param name="path">Путь к папке(заранее зашифрованный через класс Cripto)</param>
        /// <returns></returns>
        [HttpGet("GetFiles/{path}")]
        public ActionResult<List<FileModel>> GetFiles(string path)
        {
            return _service.GetFiles(path);
        }


        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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
        /// <param name="saveFile"></param>
        /// <returns></returns>
        [HttpPost("UploadFiles/{path}")]
        public async Task<IActionResult> UploadFiles(string path, [FromBody] SaveFile saveFile)
        {
            // Список файлов которые есть на сервере, если список не пуст, то вернется код состояния 409 и вылезит уведомление
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
        /// <param name="path">путь к файлу(должен быть зашифрован через класс Cripto)</param>
        /// <returns></returns>
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
