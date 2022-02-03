using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("UploadFiles")]
        public IActionResult UploadFiles(List<UploadedFile> uploadedFiles)
        {
            string badRequest = string.Empty;
            foreach (var uploadedFile in uploadedFiles)
            {
                try
                {
                    _service.UploadFile(uploadedFile);
                }
                catch (Exception ex)
                {
                    badRequest += ex.Message + "\n";
                }
            }

            if (string.IsNullOrEmpty(badRequest))
                return Ok("Файлы загружены");
            else
                return BadRequest(badRequest);
        }
    }
}
