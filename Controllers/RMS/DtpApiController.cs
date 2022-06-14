using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpApiController : BaseApiController
{
    private readonly IRmsTransferService _rmsService;

    public DtpApiController(IRmsTransferService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
    
    /****************************************************************
    * FETCH ALL DTP records
    ****************************************************************/ 
    
    [HttpGet("data-transfers/processes")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    public async Task<IActionResult> GetDtpList()
    {
        var dtps = await _rmsService.GetAllDtpsAsync();
        if (dtps == null || dtps.Count == 0)
        {
            return Ok(NoAttributesResponse<Dtp>("No DTP records were found."));
        }
        return Ok(new ApiResponse<Dtp>()
        {
            Total = dtps.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = dtps
        });
    }
    
    /****************************************************************
    * FETCH most recent DTP records
    ****************************************************************/ 

    [HttpGet("data-transfers/processes/recent/{number:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetRecentDtp(int n)
    {
        var recentDtps = await _rmsService.GetRecentDtpsAsync(n);
        if (recentDtps == null || recentDtps.Count == 0)
        {
            return Ok(NoAttributesResponse<Dtp>("No DTP{ records were found."));
        }
        return Ok(new ApiResponse<Dtp>()
        {
            Total = recentDtps.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = recentDtps
        });
    }
    
    /****************************************************************
    * FETCH specified DTP
    ****************************************************************/ 
    
    [HttpGet("data-transfers/processes/{dtp_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtp(int dtp_id)
    {
        var dtp = await _rmsService.GetDtpAsync(dtp_id);
        if (dtp == null) 
        {
            return Ok(NoAttributesResponse<Dtp>("No DTP found with that id."));
        }
        return Ok(new ApiResponse<Dtp>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dtp>() { dtp }
        });
    }
    
    /****************************************************************
    * CREATE new DTP
    ****************************************************************/ 
    
    [HttpPost("data-transfers/processes")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> CreateDtp([FromBody] Dtp dtpContent)
    {
        var dtp = await _rmsService.CreateDtpAsync(dtpContent);
        if (dtp == null)
        {
            return Ok(ErrorInActionResponse<Dtp>("Error during DTP creation."));
        }   
        return Ok(new ApiResponse<Dtp>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dtp>() { dtp }
        });
    }
       
    /****************************************************************
    * UPDATE specified DTP
    ****************************************************************/ 

    [HttpPut("data-transfers/processes/{dtp_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> UpdateDtp(int dtp_id, [FromBody] Dtp dtpContent)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<Dtp>);
        }
        var updatedDtp = await _rmsService.UpdateDtpAsync(dtp_id, dtpContent);
        if (updatedDtp == null)
        {
            return Ok(ErrorInActionResponse<Dtp>("Error during DTP update."));
        }    
        return Ok(new ApiResponse<Dtp>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dtp>() { updatedDtp }
        });
    }
       
    /****************************************************************
    * DELETE specified DTP
    ****************************************************************/ 

    [HttpDelete("data-transfers/processes/{dtp_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> DeleteDtp(int dtp_id)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<Dtp>());
        }
        var count = await _rmsService.DeleteDtpAsync(dtp_id);
        return Ok(new ApiResponse<Dtp>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTP has been removed."}, Data = null
        });
    }
}