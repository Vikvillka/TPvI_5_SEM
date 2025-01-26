using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Controllers
{
    [Route("cache")]
    public class CacheController : Controller
    {
        [HttpGet("image")]
        public IActionResult GetAction([FromQuery] string cache_parm)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "example.png");

            if (!System.IO.File.Exists(imagePath))
                return NotFound("Image not found");
            
            SetCacheHeaders(cache_parm);
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/png");
        }

        private void SetCacheHeaders(string cacheParam)
        {
            switch (cacheParam?.ToLower())
            {
                case "last-modified":
                    Response.Headers["Last-Modified"] = System.DateTime.UtcNow.AddDays(-1).ToString("R");
                    break;
                case "etag":
                    var etagValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                    Response.Headers["ETag"] = $"\"{etagValue}\"";
                    break;
                case "expired":
                    Response.Headers["Expires"] = System.DateTime.UtcNow.AddMinutes(1).ToString("R");
                    break;
                case "cache-control-max-age":
                    Response.Headers["Cache-Control"] = "max-age=3600";
                    break;
                case "cache-control-no-store":
                    Response.Headers["Cache-Control"] = "no-store";
                    break;
                default:
                    Response.Headers["Cache-Control"] = "no-cache";
                    break;
            }
        }

        [HttpGet("script")]
        public IActionResult GetScript([FromQuery] string cache_parm)
        {
            SetCacheHeaders(cache_parm);

            string script = "console.log('Hello from script');";

            return Content(script, "application/javascript");
        }

        [HttpGet("css")]
        public IActionResult GetCss([FromQuery] string cache_parm)
        {
            SetCacheHeaders(cache_parm);

            string css = "body { color: red; }";

            return Content(css, "text/css");
        }
    }
}
