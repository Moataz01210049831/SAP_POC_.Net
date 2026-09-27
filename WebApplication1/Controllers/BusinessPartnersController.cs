using Microsoft.AspNetCore.Mvc;
using WebApplication1.SapGenerated;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusinessPartnersController (SapBusinessPartnerClient _client): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int top = 10, CancellationToken ct = default)
    {
        try
        {
            var result = await _client.A_BusinessPartner.GetAsync(requestConfig =>
            {
                requestConfig.QueryParameters.Top = top;
            }, ct);

            return Ok(result?.D?.Results);
        }
        catch (Exception ex)
        {
            return StatusCode(502, new { error = "تعذر الاتصال بـ SAP", details = ex.Message });
        }
    }
}