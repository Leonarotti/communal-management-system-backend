using CommunalManagementSystem.API.Mappers;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardBW _dashboardBW;

        public DashboardController(IDashboardBW dashboardBW)
        {
            _dashboardBW = dashboardBW;
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetDashboardMetrics()
        {
            var dashboardMetrics = await _dashboardBW.GetDashboardMetricsAsync();
            if (dashboardMetrics == null)
            {
                return NotFound("Dashboard metrics not found.");
            }
            var dashboardDTOMetrics = DashboardMapper.DashboardToDashboardDTO(dashboardMetrics);
            return Ok(dashboardDTOMetrics);
        }
    }
}
