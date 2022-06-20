using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly string _attType, _attTypes;
    
    public DupApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _attType = "DUP"; _attTypes = "DUPs";
    }
    
    /****************************************************************
    * FETCH ALL DUP records
    ****************************************************************/ 

    [HttpGet("data-uses/processes")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDupList()
    {
        var allDdups = await _dupService.GetAllDupsAsync();
        return allDdups != null
            ? Ok(ListSuccessResponse(allDdups.Count, allDdups))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH most recent DUP records
    ****************************************************************/ 
    
    [HttpGet("data-uses/processes/recent/{number:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetRecentDup(int n)
    {
        var recentDups = await _dupService.GetRecentDupsAsync(n);
        return recentDups != null
            ? Ok(ListSuccessResponse(recentDups.Count, recentDups))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH specified DUP
    ****************************************************************/ 

    [HttpGet("data-uses/processes/{dup_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDup(int dup_id)
    {
        if (await _dupService.DupExistsAsync(dup_id)) {
            var dup = await _dupService.GetDupAsync(dup_id);
            return dup != null
                ? Ok(SingleSuccessResponse(new List<Dup>() { dup }))
                : Ok(ErrorResponse("r", _attType, "", dup_id.ToString(), dup_id.ToString()));
        }
        return Ok(NoEntityResponse(_attType, dup_id.ToString()));
    }
    
    /****************************************************************
    * CREATE new DUP
    ****************************************************************/ 
    
    [HttpPost("data-uses/processes")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> CreateDup([FromBody] Dup dupContent)
    {
        var newDup = await _dupService.CreateDupAsync(dupContent);
        return newDup != null
            ? Ok(SingleSuccessResponse(new List<Dup>() { newDup }))
            : Ok(ErrorResponse("c", _attType, "", "(not created)", "(not created)"));
    }
    
    /****************************************************************
    * UPDATE specified DUP
    ****************************************************************/ 
    
    [HttpPut("data-uses/processes/{dup_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> UpdateDup(int dup_id, [FromBody] Dup dupContent)
    {
        if (await _dupService.DupExistsAsync(dup_id)) {
            var updatedDup = await _dupService.UpdateDupAsync(dup_id, dupContent);
            return (updatedDup != null)
                ? Ok(SingleSuccessResponse(new List<Dup>() { updatedDup }))
                : Ok(ErrorResponse("u", _attType, "", dup_id.ToString(), dup_id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, dup_id.ToString()));
    }
   
    /****************************************************************
    * DELETE specified DUP
    ****************************************************************/ 
    
    [HttpDelete("data-uses/processes/{dup_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> DeleteDup(int dup_id)
    {
        if (await _dupService.DupExistsAsync(dup_id)) {
            var count = await _dupService.DeleteDupAsync(dup_id);
            return (count > 0)
                ? Ok(DeletionSuccessResponse(count, _attType, "", dup_id.ToString()))
                : Ok(ErrorResponse("d", _attType, "", dup_id.ToString(), dup_id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, dup_id.ToString()));
    }
    
    
    /****************************************************************
    * Get DUP statistics 
    ****************************************************************/

    [HttpGet("data-uses/processes/total")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]

    public async Task<IActionResult> GetDupTotalNumber()
    {
        var stats = await _dupService.GetTotalDups();
        return stats.StatValue > 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", _attType, "", "", "total numbers"));
    }
    
    [HttpGet("data-uses/processes/by_completion")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDupCompletionNumbers()
    {
        var stats = await _dupService.GetDupsByCompletion();
        return stats.Count == 2
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "completion numbers"));
    }
    
    
    [HttpGet("data-uses/processes/by_status")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]

    public async Task<IActionResult> GetDupsByStatus()
    {
        var stats = await _dupService.GetDupsByStatus();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by status"));
    }

}