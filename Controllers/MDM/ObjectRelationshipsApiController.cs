using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectRelationshipsApiController : BaseApiController
{
    private readonly IObjectService _objectService;

    public ObjectRelationshipsApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }
    
    /****************************************************************
    * FETCH ALL relationships for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/relationships")]
    [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
    
    public async Task<IActionResult> GetObjectRelationships(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectRelationship>());
        }
        var objRels = await _objectService.GetObjectRelationshipsAsync(sd_oid);
        if (objRels == null|| objRels.Count == 0)
        {
            return Ok(NoAttributesResponse<ObjectRelationship>("No object relationships were found."));
        }
        return Ok(new ApiResponse<ObjectRelationship>()
        {
            Total = objRels.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = objRels
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE object relationship
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
    
    public async Task<IActionResult> GetObjectRelationship(string sd_oid, int id)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectRelationship>());
        }
        var objRel = await _objectService.GetObjectRelationshipAsync(id);
        if (objRel == null) 
        {
            return Ok(NoAttributesResponse<ObjectRelationship>("No object relationship with that id found."));
        }    
        return Ok(new ApiResponse<ObjectRelationship>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectRelationship>() { objRel }
        });
    }

    /****************************************************************
    * CREATE a new relationship for a specified object
    ****************************************************************/

    [HttpPost("data-objects/{sd_oid}/relationships")]
    [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
    
    public async Task<IActionResult> CreateObjectRelationship(string sd_oid,
        [FromBody] ObjectRelationship objRelationshipContent)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectRelationship>());
        }
        objRelationshipContent.SdOid = sd_oid; 
        var objRel = await _objectService.CreateObjectRelationshipAsync(objRelationshipContent);
        if (objRel == null)
        {
            return Ok(ErrorInActionResponse<ObjectRelationship>("Error during object relationship creation."));
        }    
        return Ok(new ApiResponse<ObjectRelationship>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectRelationship>() { objRel }
        });
    }

    /****************************************************************
    * UPDATE a single specified object relationship
    ****************************************************************/

    [HttpPut("data-objects/{sd_oid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
    
    public async Task<IActionResult> UpdateObjectRelationship(string sd_oid, int id, 
        [FromBody] ObjectRelationship objRelationshipContent)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectRelationship", id))
        {
            return Ok(ErrorInActionResponse<ObjectRelationship>("No relationship with that id found for specified object."));
        }
        var updatedObjectRel = await _objectService.UpdateObjectRelationshipAsync(id, objRelationshipContent);
        if (updatedObjectRel == null) 
        {
            return Ok(ErrorInActionResponse<ObjectRelationship>("Error during object relationship update."));
        }    
        return Ok(new ApiResponse<ObjectRelationship>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectRelationship>() { updatedObjectRel }
        });
    }
    
    /****************************************************************
    * DELETE a single specified object relationship
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
    
    public async Task<IActionResult> DeleteObjectRelationship(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectRelationship", id))
        {
            return Ok(ErrorInActionResponse<ObjectRelationship>("No relationship with that id found for specified object."));
        }
        var count = await _objectService.DeleteObjectRelationshipAsync(id);
        return Ok(new ApiResponse<ObjectRelationship>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Object relationship has been removed." }, Data = null
        });
    }
}