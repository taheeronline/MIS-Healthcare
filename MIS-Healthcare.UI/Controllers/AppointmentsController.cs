﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MIS_Healthcare.UI.DTOs.Appointment;
using MIS_Healthcare.UI.DTOs.Doctor;
using MIS_Healthcare.UI.DTOs.Patient;
using System.Text;
using System.Text.Json;

namespace MIS_Healthcare.MVC.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly HttpClient _httpClient;

        public AppointmentsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7147/api/"); // Replace with your actual API URL
        }

        // GET: Appointmentss
        public async Task<IActionResult> Index()
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

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int id)
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

        // GET: Appointments/Create
        public async Task<IActionResult> Create()
        {
            // Fetch patients and doctors from the API or repository
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

                return View();
            }

            return View("Error");
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

            var jsonContent = new StringContent(JsonSerializer.Serialize(appointmentDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Appointments", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "An error occurred while adding the appointment.";
            return View("Error");
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int id)
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

            var jsonContent = new StringContent(JsonSerializer.Serialize(appointmentDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"Appointments/{id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "An error occurred while updating the appointment.";
            return View("Error");
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"Appointments/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "An error occurred while deleting the appointment.";
            return View("Error");
        }
    }
}
