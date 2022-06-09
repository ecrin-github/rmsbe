using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class DupPrereqsApiController : BaseApiController
{
    private readonly IRmsService _rmsService;

    public DupPrereqsApiController(IRmsService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
    
    
    [HttpGet("data-uses/{dupId:int}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    public async Task<IActionResult> GetDupPrereqList(int dupId)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });

        var dupPrereqs = await _rmsService.GetDupPrereqs(dupId);
        if (dupPrereqs == null)
            return Ok(new ApiResponse<DupPrereq>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No DUP prereqs have been found." },
                Data = null
            });
        
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = dupPrereqs.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupPrereqs
        });
    }

    [HttpGet("data-uses/{dupId:int}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    public async Task<IActionResult> GetDupPrereq(int dupId, int id)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });

        var dupPrereq = await _rmsService.GetDupPrereq(id);
        if (dupPrereq == null) return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP prereq has been found." },
            Data = null
        });

        var dupPrereqList = new List<DupPrereq>() { dupPrereq };
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = dupPrereqList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupPrereqList
        });
    }

    [HttpPost("data-uses/{dupId:int}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    public async Task<IActionResult> CreateDupPrereq(int dupId, [FromBody] DupPrereq dupPrereq)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });

        var dupPrereq = await _rmsService.CreateDupPrereq(dupId, dupPrereq);
        if (dupPrereq == null)
            return Ok(new ApiResponse<DupPrereq>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during DUP prereq creation." },
                Data = null
            });

        var dupPrereqList = new List<DupPrereq>() { dupPrereq };
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = dupPrereqList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupPrereqList
        });
    }

    [HttpPut("data-uses/{dupId:int}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    public async Task<IActionResult> UpdateDupPrereq(int dupId, int id, [FromBody] DupPrereq dupPrereq)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        
        var dupPrereq = await _rmsService.GetDupPrereq(id);
        if (dupPrereq == null) return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP prereq has been found." },
            Data = null
        });

        var updatedDupPrereq = await _rmsService.UpdateDupPrereq(dupPrereq);
        if (updatedDupPrereq == null) return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 0,
            StatusCode = BadRequest().StatusCode,
            Messages = new List<string>() { "Error during DUP prereq update." },
            Data = null
        });

        var dupPrereqList = new List<DupPrereq>() { updatedDupPrereq };
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = dupPrereqList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupPrereqList
        });
    }

    [HttpDelete("data-uses/{dupId:int}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    public async Task<IActionResult> DeleteDupPrereq(int dupId, int id)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        
        var dupPrereq = await _rmsService.GetDupPrereq(id);
        if (dupPrereq == null) return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP prereq has been found." },
            Data = null
        });
        
        var count = await _rmsService.DeleteDupPrereq(id);
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "DUP prereq has been removed." },
            Data = null
        });
    }

    [HttpDelete("data-uses/{dupId:int}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    public async Task<IActionResult> DeleteAllDupPrereqs(int dupId)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        
        var count = await _rmsService.DeleteAllDupPrereqs(dupId);
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "All DUP prereqs have been removed." },
            Data = null
        });
    }
    
}
