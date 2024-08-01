using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.API.Data.Models;
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
        public async Task<ActionResult<IEnumerable<Report>>> GetReports()
        {
            return Ok(await _reportRepository.GetAllReportsAsync());
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var report = await _reportRepository.GetReportByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            return Ok(report);
        }

        // POST: api/Reports
        [HttpPost]
        public async Task<ActionResult<Report>> PostReport(Report report)
        {
            await _reportRepository.AddReportAsync(report);
            return CreatedAtAction(nameof(GetReport), new { id = report.ReportID }, report);
        }

        // PUT: api/Reports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReport(int id, Report report)
        {
            if (id != report.ReportID)
            {
                return BadRequest();
            }

            var updated = await _reportRepository.UpdateReportAsync(report);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var deleted = await _reportRepository.DeleteReportAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
