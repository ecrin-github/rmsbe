using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupObjectsApiController : BaseApiController
{
    private readonly IDupService _dupService;

    public DupObjectsApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
    }
    
    /****************************************************************
    * FETCH ALL objects linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/objects")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> GetDupObjectList(int dup_id)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupObject>());
        }
        var dupObjects = await _dupService.GetAllDupObjectsAsync(dup_id);
        if (dupObjects == null || dupObjects.Count == 0)
        {
            return Ok(NoAttributesResponse<DtpObject>("No objects were found for the specified DUP."));
        }   
        return Ok(new ApiResponse<DupObject>()
        {
            Total = dupObjects.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = dupObjects
        });
    }

    /****************************************************************
    * FETCH a particular object, linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> GetDupObject(int dup_id, int id)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupObject>());
        }
        var dupObj = await _dupService.GetDupObjectAsync(id);
        if (dupObj == null) 
        {
            return Ok(NoAttributesResponse<DupObject>("No DUP object with that id found."));
        }        
        return Ok(new ApiResponse<DupObject>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DupObject>() { dupObj }
        });
    }

    /****************************************************************
    * CREATE a new object, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dup_id:int}/objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> CreateDupObject(int dup_id, string sd_oid,
        [FromBody] DupObject dupObjectContent)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupObject>());
        }
        dupObjectContent.DupId = dup_id;
        dupObjectContent.ObjectId = sd_oid;
        var dupObj = await _dupService.CreateDupObjectAsync(dupObjectContent);
        if (dupObj == null)
        {
            return Ok(ErrorInActionResponse<DupObject>("Error during DUP object creation."));
        }       
        return Ok(new ApiResponse<DupObject>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DupObject>() { dupObj }
        });
    }

    /****************************************************************
    * UPDATE an object, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dup_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> UpdateDupObject(int dup_id, int id, 
        [FromBody] DupObject dupObjectContent)
    {
        if (await _dupService.DupAttributeDoesNotExistAsync(dup_id, "DUPObject", id))
        {
            return Ok(ErrorInActionResponse<DupObject>("No object with that id found for specified DUP."));
        }
        var updatedDupObject = await _dupService.UpdateDupObjectAsync(dup_id, dupObjectContent);
        if (updatedDupObject == null) 
        {
            return Ok(ErrorInActionResponse<DupObject>("Error during DUP object update."));
        }        
        return Ok(new ApiResponse<DupObject>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DupObject>() { updatedDupObject }
        });    
    }

    /****************************************************************
    * DELETE a specified object, linked to a specified DUP
    ****************************************************************/
    
    [HttpDelete("data-uses/{dup_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> DeleteDupObject(int dup_id, int id)
    {
        if (await _dupService.DupAttributeDoesNotExistAsync(dup_id, "DUPObject", id))
        {
            return Ok(ErrorInActionResponse<DupObject>("No object with that id found for specified DUP."));
        }
        var count = await _dupService.DeleteDupObjectAsync(id);
        return Ok(new ApiResponse<DupObject>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "DUP object has been removed." }, Data = null
        });
    }
}