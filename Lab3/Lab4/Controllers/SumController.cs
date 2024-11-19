using Lab4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SumController : ControllerBase
    {

        private const string CountCookieName = "RequestCount";
        private const string SumXCookieName = "SumX";
        private const string SumYCookieName = "SumY";

        [HttpPost]
        public IActionResult Post([FromBody] SumModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int requestCount = 1;
            int sumX = model.X;
            int sumY = model.Y;

            if (Request.Cookies.TryGetValue(CountCookieName, out string countStr) &&
                int.TryParse(countStr, out int count))
            {
                requestCount = count + 1;
            }

            if (Request.Cookies.TryGetValue(SumXCookieName, out string sumXStr) &&
                int.TryParse(sumXStr, out int x))
            {
                sumX += x;
            }

            if (Request.Cookies.TryGetValue(SumYCookieName, out string sumYStr) &&
                int.TryParse(sumYStr, out int y))
            {
                sumY += y;
            }

            if (requestCount % 5 == 0)
            {
                var result = new { sx = sumX, sy = sumY };

                Response.Cookies.Append(CountCookieName, requestCount.ToString());
                Response.Cookies.Append(SumXCookieName, "0");
                Response.Cookies.Append(SumYCookieName, "0");

                return Ok(result);
            }

            Response.Cookies.Append(CountCookieName, requestCount.ToString());
            Response.Cookies.Append(SumXCookieName, sumX.ToString());
            Response.Cookies.Append(SumYCookieName, sumY.ToString());

            return Ok(new { sx = sumX, sy = sumY });
        }
    }
}
