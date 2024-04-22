using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDF_Extraction_WebAPI.Services;
using System.Net.Http.Headers;
using PDF_Extraction_WebAPI.Models;

namespace PDF_Extraction_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        [HttpPost("UploadFile"), DisableRequestSizeLimit]
        public ActionResult<InvoiceModel> Upload()
        {
            try
            {
                var file = Request.Form.Files[0]; // Add null check here
                if (file != null && file.Length > 0)
                {
                    var folderName = Path.Combine("Resources", "Files");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    InvoiceModel value = FileUploadHelper.GetPDFContent(dbPath);
                    return Ok(value);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
