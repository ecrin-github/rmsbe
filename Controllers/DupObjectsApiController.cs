using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class DupObjectsApiController : BaseApiController
{
    private readonly IRmsService _rmsService;

    public DupObjectsApiController(IRmsService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
    
    
    [HttpGet("data-uses/{dupId:int}/objects")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    public async Task<IActionResult> GetDupObjectList(int dupId)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupObject>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });

        var dupObjects = await _rmsService.GetDupObjects(dupId);
        if (dupObjects == null)
            return Ok(new ApiResponse<DupObject>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No DUP objects have been found." },
                Data = null
            });
        
        return Ok(new ApiResponse<DupObject>()
        {
            Total = dupObjects.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupObjects
        });
    }

    [HttpGet("data-uses/{dupId:int}/objects/{id}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    public async Task<IActionResult> GetDupObject(int dupId, int id)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupObject>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });

        var dupObj = await _rmsService.GetDupObject(id);
        if (dupObj == null) return Ok(new ApiResponse<DupObject>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP objects has been found." },
            Data = null
        });

        var dupObjectList = new List<DupObject>() { dupObj };
        return Ok(new ApiResponse<DupObject>()
        {
            Total = dupObjectList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupObjectList
        });
    }

    [HttpPost("data-uses/{dupId:int}/objects")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    public async Task<IActionResult> CreateDupObject(int dupId, [FromBody] DupObject dupObject)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupObject>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });

        var dupObj = await _rmsService.CreateDupObject(dupId, dupObject);
        if (dupObj == null)
            return Ok(new ApiResponse<DupObject>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during DUP object creation." },
                Data = null
            });

        var dupObjList = new List<DupObject>() { dupObj };
        return Ok(new ApiResponse<DupObject>()
        {
            Total = dupObjList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupObjList
        });
    }

    [HttpPut("data-uses/{dupId:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    public async Task<IActionResult> UpdateDupObject(int dupId, int id, [FromBody] DupObject dupObject)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupObject>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        
        var dupObj = await _rmsService.GetDupObject(id);
        if (dupObj == null) return Ok(new ApiResponse<DupObject>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP object has been found." },
            Data = null
        });

        var updatedDupObject = await _rmsService.UpdateDupObject(dupObject);
        if (updatedDupObject == null) return Ok(new ApiResponse<DupObject>()
        {
            Total = 0,
            StatusCode = BadRequest().StatusCode,
            Messages = new List<string>() { "Error during DUP object update." },
            Data = null
        });

        var dupObjList = new List<DupObject>() { updatedDupObject };
        return Ok(new ApiResponse<DupObject>()
        {
            Total = dupObjList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = dupObjList
        });
    }

    [HttpDelete("data-uses/{dupId:int}/objects/{id}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    public async Task<IActionResult> DeleteDupObject(int dupId, int id)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupObject>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        
        var dupObj = await _rmsService.GetDupObject(id);
        if (dupObj == null) return Ok(new ApiResponse<DupObject>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP object has been found." },
            Data = null
        });
        
        var count = await _rmsService.DeleteDupObject(id);
        return Ok(new ApiResponse<DupObject>()
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "DUP object has been removed." },
            Data = null
        });
    }

    [HttpDelete("data-uses/{dupId:int}/objects")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    public async Task<IActionResult> DeleteAllDupObjects(int dupId)
    {
        var dup = await _rmsService.GetDup(dupId);
        if (dup == null) return Ok(new ApiResponse<DupObject>()
        {
            Total = 0,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() { "No DUP has been found." },
            Data = null
        });
        
        var count = await _rmsService.DeleteAllDupObjects(dupId);
        return Ok(new ApiResponse<DupObject>()
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "All DUP objects have been removed." },
            Data = null
        });
    }
    
    
}