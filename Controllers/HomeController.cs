using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mutiview_BaseballPark.Models;
using Mutiview_BaseballPark.Data;
using System.Linq;

namespace Mutiview_BaseballPark.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class StadiumsController : Controller
{
    private readonly ApplicationDbContext _context;

    public StadiumsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var stadiums = _context.Stadiums.ToList();
        return View(stadiums);
    }
}
