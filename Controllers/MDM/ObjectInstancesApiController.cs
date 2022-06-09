using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectInstancesApiController : BaseApiController
{
    private readonly IObjectService _objectService;

    public ObjectInstancesApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }
    
    /****************************************************************
    * FETCH ALL instances for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/instances")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> GetObjectInstances(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectInstance>);
        }
        var objInstances = await _objectService.GetObjectInstancesAsync(sd_oid);
        if (objInstances == null || objInstances.Count == 0)
        {
            return Ok(NoAttributesResponse<ObjectDate>("No object instances were found."));
        }   
        return Ok(new ApiResponse<ObjectInstance>()
        {
            Total = objInstances.Count, StatusCode = Ok().StatusCode, Messages = null, 
            Data = objInstances
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE object instance
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/instances/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> GetObjectInstance(string sd_oid, int id)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectInstance>);
        }
        var objInstance = await _objectService.GetObjectInstanceAsync(id);
        if (objInstance == null) 
        {
            return Ok(NoAttributesResponse<ObjectInstance>("No object instance with that id found."));
        }    
        return Ok(new ApiResponse<ObjectInstance>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectInstance>() { objInstance }
        });
    }

    /****************************************************************
    * CREATE a new instance for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/instances")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> CreateObjectInstance(string sd_oid,
        [FromBody] ObjectInstance objInstanceContent)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectInstance>);
        }
        objInstanceContent.SdOid = sd_oid; 
        var objInstance = await _objectService.CreateObjectInstanceAsync(objInstanceContent);
        if (objInstance == null) 
        {
            return Ok(ErrorInActionResponse<ObjectInstance>("Error during object instance creation."));
        }
        return Ok(new ApiResponse<ObjectInstance>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectInstance>() { objInstance }
        });
    }

    /****************************************************************
    * UPDATE a single specified object instance
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/instances/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> UpdateObjectInstance(string sd_oid, int id, 
        [FromBody] ObjectInstance objInstanceContent)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectInstance", id))
        {
            return Ok(ErrorInActionResponse<ObjectInstance>("No instance with that id found for specified object."));
        }
        var updatedObjInst = await _objectService.UpdateObjectInstanceAsync(id, objInstanceContent);
        if (updatedObjInst == null)
        {
            return Ok(ErrorInActionResponse<ObjectInstance>("Error during object instance update."));
        }    
        return Ok(new ApiResponse<ObjectInstance>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectInstance>() { updatedObjInst }
        });
    }
    
    /****************************************************************
    * DELETE a single specified object instance
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}/instances/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> DeleteObjectInstance(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectInstance", id))
        {
            return Ok(ErrorInActionResponse<ObjectInstance>("No instance with that id found for specified object."));
        }
        var count = await _objectService.DeleteObjectInstanceAsync(id);
        return Ok(new ApiResponse<ObjectInstance>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Object instance has been removed." }, Data = null
        });
    }
}
