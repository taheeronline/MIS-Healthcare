using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MIS_Healthcare.UI.DTOs.Appointment;
using MIS_Healthcare.UI.DTOs.Doctor;
using MIS_Healthcare.UI.DTOs.Patient;
using System.Text;
using System.Text.Json;

namespace MIS_Healthcare.UI.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly HttpClient _httpClient;

        public AppointmentsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7147/api/"); // Replace with your actual API URL
        }

        // GET: Appointments
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

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Appointments/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var appointment = JsonSerializer.Deserialize<AppointmentToRead>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(appointment);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // GET: Appointments/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var patientsResponse = await _httpClient.GetAsync("Patients/PatientList");
                var doctorsResponse = await _httpClient.GetAsync("Doctors/DoctorList");

                if (patientsResponse.IsSuccessStatusCode && doctorsResponse.IsSuccessStatusCode)
                {
                    var patientsJson = await patientsResponse.Content.ReadAsStringAsync();
                    var doctorsJson = await doctorsResponse.Content.ReadAsStringAsync();

                    var patients = JsonSerializer.Deserialize<List<PatientList>>(patientsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var doctors = JsonSerializer.Deserialize<List<DoctorList>>(doctorsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    ViewBag.Patients = new SelectList(patients, "PatientID", "FullName");
                    ViewBag.Doctors = new SelectList(doctors, "DoctorID", "FullName");

                    var appointmentDto = new AppointmentToRegister
                    {
                        AppointmentDate = DateTime.Today,
                        PaymentStatus = "Pending",
                        AppointmentStatus = "Scheduled",
                        PaymentMode = "None"
                    };

                    return View(appointmentDto);
                }

                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] AppointmentToRegister appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return View(appointmentDto);
            }

            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(appointmentDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Appointments", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.ErrorMessage = "An error occurred while adding the appointment.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Appointments/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var appointment = JsonSerializer.Deserialize<AppointmentToUpdate>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return View(appointment);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] AppointmentToUpdate appointmentDto)
        {
            if (id != appointmentDto.AppointmentID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(appointmentDto);
            }

            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(appointmentDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"Appointments/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "An error occurred while updating the appointment.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseAppointment(int appointmentId, int doctorFees, string paymentMode)
        {
            try
            {
                var appointment = await _httpClient.GetFromJsonAsync<AppointmentToRead>($"Appointments/{appointmentId}");

                if (appointment == null)
                {
                    return NotFound();
                }

                appointment.AppointmentStatus = "Complete";
                appointment.DoctorFees = doctorFees;
                appointment.PaymentMode = paymentMode;
                appointment.PaymentStatus = "Paid";

                var jsonContent = new StringContent(JsonSerializer.Serialize(appointment), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"Appointments/{appointmentId}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.ErrorMessage = "An error occurred while closing the appointment.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Appointments/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMessage = "An error occurred while deleting the appointment.";
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
