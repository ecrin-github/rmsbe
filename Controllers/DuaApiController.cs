using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class DuaApiController : BaseApiController
{
    private readonly IRmsService _rmsService;

    public DuaApiController(IRmsService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
    
    
    
    [HttpGet("data-uses/{dupId:int}/accesses")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    public async Task<IActionResult> GetDuaList(int dupId)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<Dua>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>(){"No DUP has been found."},
            Data = null
        });

        var duaList = await _rmsService.GetAllDua(dupId);
        if (duaList == null)
            return Ok(new ApiResponse<Dua>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No DUA have been found." },
                Data = null
            });
        
        return Ok(new ApiResponse<Dua>()
        {
            Total = duaList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = duaList
        });
    }

    [HttpGet("data-uses/{dupId:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    public async Task<IActionResult> GetDua(int dupId, int id)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<Dua>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>(){"No DUP has been found."},
            Data = null
        });

        var dua = await _rmsService.GetDua(id);
        if (dua == null) return Ok(new ApiResponse<Dua>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>(){"No DUA has been found."},
            Data = null
        });
        var duaList = new List<Dua>() { dua };
        return Ok(new ApiResponse<Dua>()
        {
            Total = duaList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = duaList
        });
    }

    [HttpPost("data-uses/{dupId:int}/accesses")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    public async Task<IActionResult> CreateDua(int dupId, [FromBody] Dua dua)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<Dua>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>(){"No DUP has been found."},
            Data = null
        });

        var dua = await _rmsService.CreateDua(dupId, dua);
        if (dua == null) return Ok(new ApiResponse<Dua>()
        {
            Total = 0,
            StatusCode = BadRequest().StatusCode,
            Messages = new List<string>(){"Error during DUA creation."},
            Data = null
        });
        
        var duaList = new List<Dua>() { dua };
        return Ok(new ApiResponse<Dua>()
        {
            Total = duaList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = duaList
        });
    }

    [HttpPut("data-uses/{dupId:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    public async Task<IActionResult> UpdateDua(int dupId, int id, [FromBody] Dua dua)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<Dua>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>(){"No DUP has been found."},
            Data = null
        });

        var dua = await _rmsService.GetDua(id);
        if (dua == null) return Ok(new ApiResponse<Dua>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>(){"No DUA has been found."},
            Data = null
        });

        var updatedDua = await _rmsService.UpdateDua(dua);
        if (updatedDua == null)
            return Ok(new ApiResponse<Dua>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during DUA update." },
                Data = null
            });

        var duaList = new List<Dua>() { updatedDua };
        return Ok(new ApiResponse<Dua>()
        {
            Total = duaList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = duaList
        });
    }

    [HttpDelete("data-uses/{dupId:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    public async Task<IActionResult> DeleteDua(int dupId, int id)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<Dua>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>(){"No DUP has been found."},
            Data = null
        });

        var dua = await _rmsService.GetDua(id);
        if (dua == null) return Ok(new ApiResponse<Dua>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>(){"No DUA has been found."},
            Data = null
        });
        
        var count = await _rmsService.DeleteDua(id);
        return Ok(new ApiResponse<Dua>()
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DUA has been removed."},
            Data = null
        });
    }

    [HttpDelete("data-uses/{dupId:int}/accesses")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    public async Task<IActionResult> DeleteAllDua(int dupId)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<Dua>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>(){"No DUP has been found."},
            Data = null
        });
        
        var count = await _rmsService.DeleteAllDua(dupId);
        return Ok(new ApiResponse<Dua>()
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"All DUAs have been removed."},
            Data = null
        });
    }
    
}