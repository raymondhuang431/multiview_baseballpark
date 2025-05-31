using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mutiview_BaseballPark.Models;
using Mutiview_BaseballPark.Data;
using System.Linq;
using Mutiview_BaseballPark.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mutiview_BaseballPark.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FirebaseService _firebaseService;
    private readonly ApplicationDbContext _context;
    private readonly ImageDbService _imageDbService;

    public HomeController(ILogger<HomeController> logger, FirebaseService firebaseService, ApplicationDbContext context, ImageDbService imageDbService)
    {
        _logger = logger;
        _firebaseService = firebaseService;
        _context = context;
        _imageDbService = imageDbService;
    }

    public IActionResult Index()
    {
        var stadiums = _context.Stadiums.ToList();
        return View(stadiums);
    }

    public async Task<IActionResult> ViewImages(int stadiumId)
    {
        var images = await _imageDbService.GetImagesByStadiumIdAsync(stadiumId);

        var viewModels = new List<ImageViewModel>();
        foreach (var image in images)
        {
            try
            {
                string imageUrl = _firebaseService.GetImageUrl(image.Filename);

                viewModels.Add(new ImageViewModel
                {
                    Id = image.Id,
                    StadiumId = image.StadiumId,
                    Filename = image.Filename,
                    UploadDate = image.UploadDate,
                    Section = image.Section,
                    Row = image.Row,
                    SeatNumber = image.SeatNumber,
                    CreatedBy = image.CreatedBy,
                    ImageUrl = imageUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting image URL for file: {Filename}", image.Filename);
            }
        }

        return View(viewModels);
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
