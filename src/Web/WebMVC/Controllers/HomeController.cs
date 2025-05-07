using System.Diagnostics;
using Catalog.Application.Dtos;
using Catalog.WebMVC.Models;
using Newtonsoft.Json;
using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://172.18.0.4:80/api/plate";

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
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

        public async Task<IActionResult> Plates()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(_apiBaseUrl);

                var plateDtos = JsonConvert.DeserializeObject<List<PlateDto>>(response);

                if (plateDtos == null || !plateDtos.Any())
                {
                    _logger.LogWarning("No plates found in the API response.");
                }

                var plateViewModels = plateDtos.Select(plateDto => new PlateViewModel
                {
                    Registration = plateDto.Registration,
                    PurchasePrice = plateDto.PurchasePrice,
                    SalePrice = plateDto.SalePrice
                }).ToList();

                return View(plateViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching plates: {ex.Message}");
                return View("Error");
            }
        }
        [HttpGet]
        public IActionResult AddPlate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPlate(PlateViewModel plate)
        {
            if (!ModelState.IsValid)
                return View(plate);

            var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl, plate);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Plates");
            }

            ModelState.AddModelError("", "Failed to add plate.");
            return View(plate);
        }
    }
}