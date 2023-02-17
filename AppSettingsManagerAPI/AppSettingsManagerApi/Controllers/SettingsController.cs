using Microsoft.AspNetCore.Mvc;

namespace AppSettingsManagerApi.Controllers;

[ApiController]
[Route("settings")]
public class SettingsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
