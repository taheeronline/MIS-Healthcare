using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.UI.DTOs.Appointment;
using MIS_Healthcare.UI.Models;
using System.Diagnostics;
using System.Text.Json;

namespace MIS_Healthcare.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient, ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7147/api/"); // Replace with your actual API URL
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("Appointments");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var appointments = JsonSerializer.Deserialize<List<AppointmentToRead>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return View(appointments);
                }

                ViewBag.ErrorMessage = "An error occurred while fetching appointments.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }
    }
}
