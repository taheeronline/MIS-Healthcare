using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MIS_Healthcare.UI.DTOs.Doctor;
using System.Text;
using System.Text.Json;

namespace MIS_Healthcare.UI.Controllers
{
    public enum Gender
    {
        M,
        F,
        O
    }

    public class DoctorsController : Controller
    {
        private readonly HttpClient _httpClient;

        public DoctorsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7147/api/"); // Replace with your actual API URL
        }

        // GET: Doctors
        public async Task<IActionResult> Index(string doctorType)
        {
            try
            {
                var response = await _httpClient.GetAsync("Doctors");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doctors = JsonSerializer.Deserialize<List<DoctorToRead>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (!string.IsNullOrEmpty(doctorType))
                    {
                        doctors = doctors.Where(d => d.DoctorType.Contains(doctorType, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    // Pass the search term to the view
                    ViewBag.DoctorType = doctorType;

                    return View(doctors);
                }

                ViewBag.ErrorMessage = "An error occurred while fetching doctors.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Doctors/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doctor = JsonSerializer.Deserialize<DoctorToRead>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(doctor);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // GET: Doctors/Create
        public IActionResult Create()
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

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] DoctorToRegister doctorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(doctorDto);
                }

                var jsonContent = new StringContent(JsonSerializer.Serialize(doctorDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Doctors", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "An error occurred while adding the doctor.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Doctors/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doctor = JsonSerializer.Deserialize<DoctorToUpdate>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Populate ViewBag with Gender enum values
                    ViewBag.GenderOptions = new SelectList(Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(g => new
                    {
                        Value = g.ToString(),
                        Text = g.ToString()
                    }), "Value", "Text", doctor.Gender.ToString());

                    return View(doctor);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] DoctorToUpdate doctorDto)
        {
            try
            {
                if (id != doctorDto.DoctorID)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return View(doctorDto);
                }

                var jsonContent = new StringContent(JsonSerializer.Serialize(doctorDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"Doctors/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "An error occurred while updating the doctor.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Doctors/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "An error occurred while deleting the doctor.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }
    }
}
