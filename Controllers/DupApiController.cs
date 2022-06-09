using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class DupApiController : BaseApiController
{
    private readonly IRmsService _rmsService;

    public DupApiController(IRmsService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
    
    /****************************************************************
    * FETCH ALL DUP records
    ****************************************************************/ 

    [HttpGet("data-uses/processes")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDupList()
    {
        var dups = await _rmsService.GetAllDupsAsync();
        if (dups == null || dups.Count == 0)
        {
            return Ok(NoAttributesResponse<Dup>("No DUP records were found."));
        }
        return Ok(new ApiResponse<Dup>()
        {
            Total = dups.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = dups
        });
    }
    
    /****************************************************************
    * FETCH most recent DUP records
    ****************************************************************/ 
    
    [HttpGet("data-uses/processes/recent/{number:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetRecentDup(int n)
    {
        var recentDups = await _rmsService.GetRecentDupsAsync(n);
        if (recentDups == null || recentDups.Count == 0)
        {
            return Ok(NoAttributesResponse<Dup>("No DUP records were found."));
        }
        return Ok(new ApiResponse<Dup>()
        {
            Total = recentDups.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = recentDups
        });
    }
    
    /****************************************************************
    * FETCH specified DUP
    ****************************************************************/ 

    [HttpGet("data-uses/processes/{dup_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDup(int dup_id)
    {
        var dup = await _rmsService.GetDupAsync(dup_id);
        if (dup == null) 
        {
            return Ok(NoAttributesResponse<Dup>("No DUP found with that id."));
        }    
        return Ok(new ApiResponse<Dup>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dup>() { dup }
        });
    }
    
    /****************************************************************
    * CREATE new DUP
    ****************************************************************/ 
    
    [HttpPost("data-uses/processes")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> CreateDup([FromBody] Dup dupContent)
    {
        var dup = await _rmsService.CreateDupAsync(dupContent);
        if (dup == null)
        {
            return Ok(ErrorInActionResponse<Dup>("Error during DUP creation."));
        }       
        return Ok(new ApiResponse<Dup>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dup>() { dup }
        });
    }
    
    /****************************************************************
    * UPDATE specified DUP
    ****************************************************************/ 
    
    [HttpPut("data-uses/processes/{dup_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> UpdateDup(int dup_id, [FromBody] Dup dupContent)
    {
        if (await _rmsService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDUPResponse<Dup>);
        }
        var updatedDup = await _rmsService.UpdateDupAsync(dup_id, dupContent);
        if (updatedDup == null) 
        {
            return Ok(ErrorInActionResponse<Dup>("Error during DUP update."));
        }       
        return Ok(new ApiResponse<Dup>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dup>() { updatedDup }
        });
    }
   
    /****************************************************************
    * DELETE specified DUP
    ****************************************************************/ 
    
    [HttpDelete("data-uses/processes/{dup_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> DeleteDup(int dup_id)
    {
        if (await _rmsService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDUPResponse<Dup>);
        }
        var count = await _rmsService.DeleteDupAsync(dup_id);
        return Ok(new ApiResponse<Dup>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "DUP has been removed." }, Data = null
        });
    }
}