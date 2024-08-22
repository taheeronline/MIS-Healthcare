using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.API.Data.DTOs.Report;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Middleware;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly iReportRepo _reportRepository;

        public ReportsController(iReportRepo reportRepository)
        {
            _reportRepository = reportRepository;
        }

        // GET: api/Reports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportToRead>>> GetReports()
        {
            try
            {
                var reports = await _reportRepository.GetAllReportsAsync();
                var reportDtos = reports.Select(r => new ReportToRead
                {
                    ReportID = r.ReportID,
                    AppointmentID = r.AppointmentID,
                    PatientID = r.PatientID,
                    PatientName = $"{r.Patient.FirstName} {r.Patient.LastName}",
                    DoctorID = r.DoctorID,
                    DoctorName = $"{r.Doctor.FirstName} {r.Doctor.LastName}",
                    MedicinePrescribed = r.MedicinePrescribed,
                    DoctorComment = r.DoctorComment
                }).ToList();

                return Ok(reportDtos);
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching reports.", details = ex.Message });
            }
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportToRead>> GetReport(int id)
        {
            try
            {
                var report = await _reportRepository.GetReportByIdAsync(id);
                if (report == null)
                {
                    return NotFound();
                }

                var reportDto = new ReportToRead
                {
                    ReportID = report.ReportID,
                    AppointmentID = report.AppointmentID,
                    MedicinePrescribed = report.MedicinePrescribed,
                    DoctorComment = report.DoctorComment
                };

                return Ok(reportDto);
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the report.", details = ex.Message });
            }
        }

        // POST: api/Reports
        [HttpPost]
        public async Task<ActionResult<ReportToRead>> PostReport([FromBody] ReportToRegister reportDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var report = new Report
                {
                    AppointmentID = reportDto.AppointmentID,
                    MedicinePrescribed = reportDto.MedicinePrescribed,
                    DoctorComment = reportDto.DoctorComment
                };

                await _reportRepository.AddReportAsync(report);

                var createdReportDto = new ReportToRead
                {
                    ReportID = report.ReportID,
                    AppointmentID = report.AppointmentID,
                    MedicinePrescribed = report.MedicinePrescribed,
                    DoctorComment = report.DoctorComment
                };

                return CreatedAtAction(nameof(GetReport), new { id = report.ReportID }, createdReportDto);
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the report.", details = ex.Message });
            }
        }

        // PUT: api/Reports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReport(int id, [FromBody] ReportToUpdate reportDto)
        {
            if (id != reportDto.ReportID)
            {
                return BadRequest();
            }

            try
            {
                var report = new Report
                {
                    ReportID = reportDto.ReportID,
                    AppointmentID = reportDto.AppointmentID,
                    MedicinePrescribed = reportDto.MedicinePrescribed,
                    DoctorComment = reportDto.DoctorComment
                };

                var updated = await _reportRepository.UpdateReportAsync(report);
                if (!updated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the report.", details = ex.Message });
            }
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            try
            {
                var deleted = await _reportRepository.DeleteReportAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the report.", details = ex.Message });
            }
        }
    }
}
