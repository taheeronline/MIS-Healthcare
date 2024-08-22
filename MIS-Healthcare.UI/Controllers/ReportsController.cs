using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using MIS_Healthcare.UI.DTOs.Report;

namespace MIS_Healthcare.UI.Controllers
{
    public class ReportsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReportsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7147/api/"); // Replace with your actual API URL
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("Reports");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var reports = JsonSerializer.Deserialize<List<ReportToRead>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(reports);
                }

                ViewBag.ErrorMessage = "An error occurred while fetching reports.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Reports/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var report = JsonSerializer.Deserialize<ReportToRead>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(report);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ReportToRegister reportDto)
        {
            if (!ModelState.IsValid)
            {
                return View(reportDto);
            }

            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(reportDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Reports", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "An error occurred while adding the report.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Reports/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var report = JsonSerializer.Deserialize<ReportToUpdate>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(report);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // POST: Reports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] ReportToUpdate reportDto)
        {
            if (id != reportDto.ReportID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(reportDto);
            }

            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(reportDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"Reports/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "An error occurred while updating the report.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Reports/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "An error occurred while deleting the report.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
