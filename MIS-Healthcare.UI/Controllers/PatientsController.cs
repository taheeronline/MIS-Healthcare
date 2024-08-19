using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MIS_Healthcare.MVC.Controllers;
using MIS_Healthcare.UI.DTOs.Patient;
using System.Text;
using System.Text.Json;

namespace MIS_Healthcare.UI.Controllers
{
    public class PatientsController : Controller
    {
        private readonly HttpClient _httpClient;

        public PatientsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7147/api/"); // Replace with your actual API URL
        }

        // GET: Patients
        public async Task<IActionResult> Index(string contactNumber)
        {
            var response = await _httpClient.GetAsync("Patients");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var patients = JsonSerializer.Deserialize<List<PatientToRead>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (!string.IsNullOrEmpty(contactNumber))
                {
                    patients = patients.Where(d => d.ContactNumber.Contains(contactNumber, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Pass the search term to the view
                ViewBag.ContactNumber = contactNumber;

                return View(patients);
            }

            ViewBag.ErrorMessage = "An error occurred while fetching patients.";
            return View("Error");
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"Patients/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var patient = JsonSerializer.Deserialize<PatientToRead>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(patient);
            }

            return NotFound();
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] PatientToRegister patientDto)
        {
            if (!ModelState.IsValid)
            {
                return View(patientDto);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(patientDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Patients", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "An error occurred while adding the patient.";
            return View("Error");
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"Patients/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var patient = JsonSerializer.Deserialize<PatientToUpdate>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Populate ViewBag with Gender enum values
                ViewBag.GenderOptions = new SelectList(Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(g => new
                {
                    Value = g.ToString(),
                    Text = g.ToString()
                }), "Value", "Text", patient.Gender.ToString());

                return View(patient);
            }

            return NotFound();
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] PatientToUpdate patientDto)
        {
            if (id != patientDto.PatientID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(patientDto);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(patientDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"Patients/{id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "An error occurred while updating the patient.";
            return View("Error");
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"Patients/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var patient = JsonSerializer.Deserialize<PatientToRead>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(patient);
            }

            return NotFound();
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"Patients/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "An error occurred while deleting the patient.";
            return View("Error");
        }
    }
}
