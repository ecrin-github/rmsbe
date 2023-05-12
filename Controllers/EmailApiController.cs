using Microsoft.AspNetCore.Mvc;
using rmsbe.Contracts.Email.Request;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

[ApiController]
[Route("service-controller")]
public class EmailApiController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailApiController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequestBody emailRequestBody)
    {
        var res = _emailService.Send(emailRequestBody);
        return Ok(res);
    }
}