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
    private readonly ImageDbService _imageDbService;
    private readonly StadiumDbService _stadiumDbService;

    public HomeController(ILogger<HomeController> logger, FirebaseService firebaseService, ImageDbService imageDbService, StadiumDbService stadiumDbService)
    {
        _logger = logger;
        _firebaseService = firebaseService;
        _imageDbService = imageDbService;
        _stadiumDbService = stadiumDbService;
    }

    public async Task<IActionResult> Index()
    {
        var stadiums = await _stadiumDbService.GetStadiumsAsync();

        var stadiumViewModels = new List<StadiumViewModel>();
        foreach (var stadium in stadiums)
        {
            string mainImageUrl = null;
            if (!string.IsNullOrEmpty(stadium.MainImageUrlFilename))
            {
                try
                {
                    mainImageUrl = _firebaseService.GetImageUrl(stadium.MainImageUrlFilename);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting main image URL for stadium {StadiumName} with filename {Filename}", stadium.StadiumName, stadium.MainImageUrlFilename);
                }
            }

            stadiumViewModels.Add(new StadiumViewModel
            {
                StadiumId = stadium.StadiumId,
                StadiumName = stadium.StadiumName,
                City = stadium.City,
                Country = stadium.Country,
                MainImageUrl = mainImageUrl
            });
        }

        return View(stadiumViewModels);
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
