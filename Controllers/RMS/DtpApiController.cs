using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _attType, _attTypes;
    
    public DtpApiController(IDtpService rmsService)
    {
        _dtpService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
        _attType = "DTP"; _attTypes = "DTPs";
    }
    
    /****************************************************************
    * FETCH ALL DTP records
    ****************************************************************/ 
    
    [HttpGet("data-transfers/processes")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpList()
    {
        var allDdtps = await _dtpService.GetAllDtpsAsync();
        return allDdtps != null
            ? Ok(ListSuccessResponse(allDdtps.Count, allDdtps))
            : Ok(NoAttributesResponse(_attTypes));
    }
     
    /****************************************************************
    * FETCH most recent DTP records
    ****************************************************************/ 

    [HttpGet("data-transfers/processes/recent/{number:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetRecentDtp(int n)
    {
        var recentDtps = await _dtpService.GetRecentDtpsAsync(n);
        return recentDtps != null
            ? Ok(ListSuccessResponse(recentDtps.Count, recentDtps))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH specified DTP
    ****************************************************************/ 
    
    [HttpGet("data-transfers/processes/{dtp_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtp(int dtp_id)
    {
        if (await _dtpService.DtpExistsAsync(dtp_id)) {
            var dtp = await _dtpService.GetDtpAsync(dtp_id);
            return dtp != null
                ? Ok(SingleSuccessResponse(new List<Dtp>() { dtp }))
                : Ok(ErrorResponse("r", _attType, "", dtp_id.ToString(), dtp_id.ToString()));
        }
        return Ok(NoEntityResponse(_attType, dtp_id.ToString()));
    }
    
    /****************************************************************
    * CREATE new DTP
    ****************************************************************/ 
    
    [HttpPost("data-transfers/processes")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> CreateDtp([FromBody] Dtp dtpContent)
    {
        var newDtp = await _dtpService.CreateDtpAsync(dtpContent);
        return newDtp != null
            ? Ok(SingleSuccessResponse(new List<Dtp>() { newDtp }))
            : Ok(ErrorResponse("c", _attType, "", "(not created)", "(not created)"));
    }
    
    /****************************************************************
    * UPDATE specified DTP
    ****************************************************************/ 

    [HttpPut("data-transfers/processes/{dtp_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> UpdateDtp(int dtp_id, [FromBody] Dtp dtpContent)
    {
        if (await _dtpService.DtpExistsAsync(dtp_id)) {
            var updatedDtp = await _dtpService.UpdateDtpAsync(dtp_id, dtpContent);
            return (updatedDtp != null)
                ? Ok(SingleSuccessResponse(new List<Dtp>() { updatedDtp }))
                : Ok(ErrorResponse("u", _attType, "", dtp_id.ToString(), dtp_id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, dtp_id.ToString()));
    }
    
    /****************************************************************
    * DELETE specified DTP
    ****************************************************************/ 

    [HttpDelete("data-transfers/processes/{dtp_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> DeleteDtp(int dtp_id)
    {
        if (await _dtpService.DtpExistsAsync(dtp_id)) {
            var count = await _dtpService.DeleteDtpAsync(dtp_id);
            return (count > 0)
                ? Ok(DeletionSuccessResponse(count, _attType, "", dtp_id.ToString()))
                : Ok(ErrorResponse("d", _attType, "", dtp_id.ToString(), dtp_id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, dtp_id.ToString()));
    }
    
    
    /****************************************************************
    * Get DTP statistics 
    ****************************************************************/

    [HttpGet("data-transfers/processes/total")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]

    public async Task<IActionResult> GetDtpTotalNumber()
    {
        var stats = await _dtpService.GetTotalDtps();
        return stats.StatValue > 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", _attType, "", "", "total numbers"));
    }
    
    [HttpGet("data-transfers/processes/by_completion")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpCompletionNumbers()
    {
        var stats = await _dtpService.GetDtpsByCompletion();
        return stats.Count == 2
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "completion numbers"));
    }
    
    
    [HttpGet("data-transfers/processes/by_status")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]

    public async Task<IActionResult> GetDtpsByStatus()
    {
        var stats = await _dtpService.GetDtpsByStatus();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by status"));
    }
    
}