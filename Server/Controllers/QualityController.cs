using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WorkingWithFiles.Server.Service;
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


        [HttpGet("GetFiles/{path}")]
        public ActionResult<List<FileModel>> GetFiles(string path)
        {
            return _service.GetFiles(path);
        }


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

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromBody] SaveFile saveFile)
        {
            var fileExists = await _service.UploadFile(saveFile);
            if (fileExists.Count == 0)
            {
                return Ok();
            }
            else
            {
                return Conflict("Файлы: " + string.Join(" ", fileExists) + " уже существуют в каталоге, сначала удалите предыдущие версии");
            }
        }
    }
}
