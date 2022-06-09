using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class SecondaryUseApiController : BaseApiController
{
    private readonly IRmsService _rmsService;

    public SecondaryUseApiController(IRmsService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
    
    
    
    [HttpGet("data-uses/{dupId:int}/secondary-use")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    public async Task<IActionResult> GetSecondaryUseList(int dupId)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });

        var secUses = await _rmsService.GetSecondaryUses(dupId);
        if (secUses == null)
            return Ok(new ApiResponse<SecondaryUse>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No secondary uses have been found." },
                Data = null
            });
        
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = secUses.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = secUses
        });
    }

    [HttpGet("data-uses/{dupId:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    public async Task<IActionResult> GetSecondaryUse(int dupId, int id)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });

        var secUse = await _rmsService.GetSecondaryUse(id);
        if (secUse == null) return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No secondary use has been found." },
            Data = null
        });

        var secUseList = new List<SecondaryUse>() { secUse };
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = secUseList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = secUseList
        });
    }

    [HttpPost("data-uses/{dupId:int}/secondary-use")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    public async Task<IActionResult> CreateSecondaryUse(int dupId, [FromBody] SecondaryUse secondaryUse)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });

        var secUse = await _rmsService.CreateSecondaryUse(dupId, secondaryUse);
        if (secUse == null)
            return Ok(new ApiResponse<SecondaryUse>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during secondary use creation." },
                Data = null
            });

        var secUseList = new List<SecondaryUse>() { secUse };
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = secUseList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = secUseList
        });
    }

    [HttpPut("data-uses/{dupId:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    public async Task<IActionResult> UpdateSecondaryUse(int dupId, int id, [FromBody] SecondaryUse secondaryUse)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        
        var secUse = await _rmsService.GetSecondaryUse(id);
        if (secUse == null) return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No secondary use has been found." },
            Data = null
        });

        var updateSecUse = await _rmsService.UpdateSecondaryUse(secondaryUse);
        if (updateSecUse == null)
            return Ok(new ApiResponse<SecondaryUse>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during secondary use update." },
                Data = null
            });

        var secUseList = new List<SecondaryUse>() { updateSecUse };
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = secUseList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = secUseList
        });
    }

    [HttpDelete("data-uses/{dupId:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    public async Task<IActionResult> DeleteSecondaryUse(int dupId, int id)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        
        var secUse = await _rmsService.GetSecondaryUse(id);
        if (secUse == null) return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No secondary use has been found." },
            Data = null
        });
        
        var count = await _rmsService.DeleteSecondaryUse(id);
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Secondary use has been removed." },
            Data = null
        });
    }

    [HttpDelete("data-uses/{dupId:int}/secondary-use")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    public async Task<IActionResult> DeleteAllSecondaryUses(int dupId)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        
        var count = await _rmsService.DeleteAllSecondaryUses(dupId);
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "All secondary uses have been removed." },
            Data = null
        });
    }
}