using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectDatesApiController : BaseApiController
{
    private readonly IObjectService _objectService;

    public ObjectDatesApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }

    /****************************************************************
    * FETCH ALL dates for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/dates")]
    [SwaggerOperation(Tags = new[] { "Object dates endpoint" })]
    
    public async Task<IActionResult> GetObjectDates(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectDate>());
        }
        var objDates = await _objectService.GetObjectDatesAsync(sd_oid);
        if (objDates == null || objDates.Count == 0)
        {
            return Ok(NoAttributesResponse<ObjectDate>("No object dates were found."));
        }
        return Ok(new ApiResponse<ObjectDate>()
        {
            Total = objDates.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = objDates
        });
    }

    /****************************************************************
    * FETCH A SINGLE object date
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/dates/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Object dates endpoint" })]
    
    public async Task<IActionResult> GetObjectDate(string sd_oid, int id)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectDate>());
        }
        var objDate = await _objectService.GetObjectDateAsync(id);
        if (objDate == null)
        {
            return Ok(NoAttributesResponse<ObjectDate>("No object date with that id found."));
        }
        return Ok(new ApiResponse<ObjectDate>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectDate>() { objDate }
        });
    }

    /****************************************************************
    * CREATE a new date for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/dates")]
    [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
    
    public async Task<IActionResult> CreateObjectDate(string sd_oid, 
        [FromBody] ObjectDate objDateContent)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectDate>());
        }
        objDateContent.SdOid = sd_oid; 
        var objDate = await _objectService.CreateObjectDateAsync(objDateContent);
        if (objDate == null)
        {
            return Ok(ErrorInActionResponse<ObjectDate>("Error during object date creation."));
        }
        return Ok(new ApiResponse<ObjectDate>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectDate>() { objDate }
        });
    }  
    
    /****************************************************************
    * UPDATE a single specified object date
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/dates/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
    
    public async Task<IActionResult> UpdateObjectDate(string sd_oid, int id, 
        [FromBody] ObjectDate objDateContent)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectDate", id))
        {
            return Ok(ErrorInActionResponse<ObjectDate>("No date with that id found for specified object."));
        }
        var updatedObjDate = await _objectService.UpdateObjectDateAsync(id, objDateContent);
        if (updatedObjDate == null)
        {
            return Ok(ErrorInActionResponse<ObjectDate>("Error during object date update."));
        }
        return Ok(new ApiResponse<ObjectDate>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectDate>() { updatedObjDate }
        });
    }
    
    /****************************************************************
    * DELETE a single specified object date
    ****************************************************************/
    
    [HttpDelete("data-objects/{sd_oid}/dates/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
    
    public async Task<IActionResult> DeleteObjectDate(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectDate", id))
        {
            return Ok(ErrorInActionResponse<ObjectDate>("No date with that id found for specified object."));
        }
        var count = await _objectService.DeleteObjectDateAsync(id);
        return Ok(new ApiResponse<ObjectDate>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Object date has been removed." }, Data = null
        });
    }
}