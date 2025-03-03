using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MVCTutorial.Models;

namespace MVCTutorial.Controllers
{
    public class ImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ConnectionDB _connection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ImageController> _logger;

        public ImageController(ConnectionDB connection, IWebHostEnvironment webHostEnvironment, ILogger<ImageController> logger)
        {
            _connection = connection;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] ProductViewModel model)
        {
            try
            {
                var file = model.ImageFile;
                if (file == null)
                    return BadRequest("Nenhum arquivo foi enviado.");

                byte[] imageByte;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imageByte = memoryStream.ToArray();
                }

                var img = new ImageStore
                {
                    ImageName = file.FileName,
                    ImageByte = imageByte,
                    ImagePath = Path.Combine("UploadedImage", file.FileName),
                    IsDeleted = false
                };

                _connection.ImageStore.Add(img);
                _connection.SaveChanges();

                return Json(new { imgID = img.ImageID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer upload da imagem");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        [HttpGet]
        public IActionResult ImageRetrieve(int imgID)
        {
            var img = _connection.ImageStore.FirstOrDefault(x => x.ImageID == imgID);
            if (img == null || img.ImageByte == null)
                return NotFound();

            return File(img.ImageByte, GetContentType(img.ImageName));
        }
        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(fileName, out string contentType))
            {
                return contentType;
            }
            return "application/octet-stream";
        }
    }
}
