using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class ObjectDescriptionsApiController : BaseApiController
{
    private readonly IObjectService _objectService;

    public ObjectDescriptionsApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }
    
    /****************************************************************
    * FETCH ALL descriptions for a specified object
    ****************************************************************/

    [HttpGet("data-objects/{sd_oid}/descriptions")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
    public async Task<IActionResult> GetObjectDescriptions(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectDescription>);
        }
        var objDescriptions = await _objectService.GetObjectDescriptionsAsync(sd_oid);
        if (objDescriptions == null|| objDescriptions.Count == 0)
        {
            return Ok(NoAttributesResponse<ObjectDate>("No object descriptions were found."));
        }
        return Ok(new ApiResponse<ObjectDescription>()
        {
            Total = objDescriptions.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = objDescriptions
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE object description
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/descriptions/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
    public async Task<IActionResult> GetObjectDescription(string sd_oid, int id)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectDescription>);
        }
        var objDesc = await _objectService.GetObjectDescriptionAsync(id);
        if (objDesc == null) 
        {
            return Ok(NoAttributesResponse<ObjectDate>("No object description with that id found."));
        }    
        return Ok(new ApiResponse<ObjectDescription>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectDescription>() { objDesc }
        });
    }
    
    /****************************************************************
    * CREATE a new description for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/descriptions")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
    public async Task<IActionResult> CreateObjectDescription(string sd_oid, ObjectDescription objectDescriptionContent)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectDescription>);
        }
        objectDescriptionContent.SdOid = sd_oid; 
        var objDesc = await _objectService.CreateObjectDescriptionAsync(objectDescriptionContent);
        if (objDesc == null) 
        {
            return Ok(ErrorInActionResponse<ObjectDescription>("Error during object description creation."));
        }   
        return Ok(new ApiResponse<ObjectDescription>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectDescription>() { objDesc }
        });
    }

    /****************************************************************
    * UPDATE a single specified object description
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/descriptions/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    public async Task<IActionResult> UpdateObjectDescription(string sd_oid, int id, [FromBody] ObjectDescription objectDescriptionContent)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectDescription", id))
        {
            return Ok(ErrorInActionResponse<ObjectDescription>("No description with that id found for specified object."));
        }
        var updatedObjDesc = await _objectService.UpdateObjectDescriptionAsync(id, objectDescriptionContent);
        if (updatedObjDesc == null) 
        {
            return Ok(ErrorInActionResponse<ObjectDescription>("Error during object description creation."));
        }       
        return Ok(new ApiResponse<ObjectDescription>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectDescription>() { updatedObjDesc }
        });
    }
    
    /****************************************************************
    * DELETE a single specified object description
    ****************************************************************/
    
    [HttpDelete("data-objects/{sd_oid}/descriptions/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
    public async Task<IActionResult> DeleteObjectDescription(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectDescription", id))
        {
            return Ok(ErrorInActionResponse<ObjectDescription>("No description with that id found for specified object."));
        }
       
        var count = await _objectService.DeleteObjectDescriptionAsync(id);
        return Ok(new ApiResponse<ObjectDescription>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Object description has been removed." }, Data = null
        });
    }
}