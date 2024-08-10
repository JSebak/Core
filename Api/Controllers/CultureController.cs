namespace Api.Controllers
{
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using System.Globalization;

    [ApiController]
    [Route("[controller]")]
    public class CultureController : ControllerBase
    {
        [HttpPost("SetCulture")]
        public IActionResult SetCulture([FromQuery] string culture)
        {
            if (string.IsNullOrEmpty(culture))
            {
                return BadRequest(new { message = "Culture is required." });
            }

            try
            {
                var cultureInfo = new CultureInfo(culture);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;

                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureInfo)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

                return Ok(new { message = $"Culture set to {culture}" });
            }
            catch (CultureNotFoundException)
            {
                return BadRequest(new { message = "Invalid culture." });
            }
        }
    }

}
