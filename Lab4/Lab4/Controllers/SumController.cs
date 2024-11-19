using Lab4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SumController : ControllerBase
    {
        private const string CountSessionKey = "RequestCount";
        private const string SumXSessionKey = "SumX";
        private const string SumYSessionKey = "SumY";

        [HttpPost]
        public IActionResult Post([FromBody] SumModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int requestCount = HttpContext.Session.GetInt32(CountSessionKey) ?? 0;
            int sumX = HttpContext.Session.GetInt32(SumXSessionKey) ?? 0;
            int sumY = HttpContext.Session.GetInt32(SumYSessionKey) ?? 0;

            requestCount++;

            if (requestCount % 5 == 1)
            {
                sumX = model.X;
                sumY = model.Y;
            }
            else
            {
                sumX += model.X;
                sumY += model.Y;
            }

            HttpContext.Session.SetInt32(CountSessionKey, requestCount);
            HttpContext.Session.SetInt32(SumXSessionKey, sumX);
            HttpContext.Session.SetInt32(SumYSessionKey, sumY);

            return Ok(new { sx = sumX, sy = sumY });
        }
    }
}
