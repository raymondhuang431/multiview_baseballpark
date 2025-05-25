using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mutiview_BaseballPark.Models;
using Mutiview_BaseballPark.Data;
using System.Linq;
using Mutiview_BaseballPark.Services;

namespace Mutiview_BaseballPark.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FirebaseService _firebaseService;

    public HomeController(ILogger<HomeController> logger, FirebaseService firebaseService)
    {
        _logger = logger;
        _firebaseService = firebaseService;
    }

    public IActionResult Index()
    {
        try
        {
            // 請將這裡的圖片路徑改為你實際上傳到 Firebase Storage 的圖片路徑
            string imageUrl = _firebaseService.GetImageUrl("test.jpg");
            ViewBag.ImageUrl = imageUrl;
            return View();
        }
        catch (Exception ex)
        {
            ViewBag.Error = "找不到圖片，請確認圖片路徑是否正確，或是否已上傳圖片到 Firebase Storage";
            return View();
        }
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
