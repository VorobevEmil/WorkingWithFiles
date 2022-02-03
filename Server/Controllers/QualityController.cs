using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WorkingWithFiles.Server.Service;

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
        public Dictionary<string, List<string>> GetFiles(string path)
        {
            return _service.GetFiles(path);
        }

        public void DownloadFile(string path)
        {

        }

        [HttpGet]
        [Route("DeleteFile")]
        public IActionResult DeleteFile(string fileName)
        {
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Не удалось удалить файл, причина: " + ex.Message);
            }
        }
    }
}
