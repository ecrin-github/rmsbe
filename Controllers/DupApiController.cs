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
    
    
    [HttpGet("data-uses/processes")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    public async Task<IActionResult> GetDupList()
    {
        var dupList = await _rmsService.GetAllDup();
        if (dupList == null)
            return Ok(new ApiResponse<Dup>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No DUPs have been found." },
                Data = null
            });
        return Ok(new ApiResponse<Dup>()
        {
            Total = dupList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupList
        });
    }
    
    [HttpGet("data-uses/processes/recent/{number:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    public async Task<IActionResult> GetRecentDup(int number)
    {
        var recentData = await _rmsService.GetRecentDup(number);
        if (recentData == null) return Ok(new ApiResponse<Dup>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        return Ok(new ApiResponse<Dup>()
        {
            Total = recentData.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = recentData
        });
    }
    
    [HttpGet("data-uses/processes/{dupId:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    public async Task<IActionResult> GetDup(int dupId)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<Dup>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        var dupList = new List<Dup>() { dup };
        return Ok(new ApiResponse<Dup>()
        {
            Total = dupList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupList
        });
    }

    [HttpPost("data-uses/processes")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    public async Task<IActionResult> CreateDup([FromBody] Dup dup)
    {
        var dup = await _rmsService.CreateDup(dup);
        if (dup == null)
            return Ok(new ApiResponse<Dup>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during DUP creation." },
                Data = null
            });
        var dupList = new List<Dup>() { dup };
        return Ok(new ApiResponse<Dup>()
        {
            Total = dupList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupList
        });
    }
    
    [HttpPut("data-uses/processes/{dupId:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    public async Task<IActionResult> UpdateDup(int dupId, [FromBody] Dup dup)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<Dup>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        var updatedDup = await _rmsService.UpdateDup(dup);
        if (updatedDup == null) return Ok(new ApiResponse<Dup>()
        {
            Total = 0,
            StatusCode = BadRequest().StatusCode,
            Messages = new List<string>() { "Error during DUP update." },
            Data = null
        });
        var dupList = new List<Dup>() { updatedDup };
        return Ok(new ApiResponse<Dup>()
        {
            Total = dupList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupList
        });
    }

    [HttpDelete("data-uses/processes/{dupId:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    public async Task<IActionResult> DeleteDup(int dupId)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<Dup>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        
        var count = await _rmsService.DeleteDup(dupId);
        return Ok(new ApiResponse<Dup>()
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "DUP has been removed." },
            Data = null
        });
    }
    
}