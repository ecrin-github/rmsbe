using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectIdentifiersApiController : BaseApiController
{
    private readonly IObjectService _objectService;
    
    public ObjectIdentifiersApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }
    
    /****************************************************************
    * FETCH ALL identifiers for a specified object
    ****************************************************************/

    [HttpGet("data-objects/{sd_oid}/identifiers")]
    [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
    
    public async Task<IActionResult> GetObjectIdentifiers(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectIdentifier>);
        }
        var objIdentifiers = await _objectService.GetObjectIdentifiersAsync(sd_oid);
        if (objIdentifiers == null|| objIdentifiers.Count == 0)
        {
            return Ok(NoAttributesResponse<ObjectIdentifier>("No object identifiers were found."));
        }
        return Ok(new ApiResponse<ObjectIdentifier>()
        {
            Total = objIdentifiers.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = objIdentifiers
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE object identifier
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
    
    public async Task<IActionResult> GetObjectIdentifier(string sd_oid, int id)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectIdentifier>);
        }
        var objIdentifier = await _objectService.GetObjectIdentifierAsync(id);
        if (objIdentifier == null) 
        {
            return Ok(NoAttributesResponse<ObjectIdentifier>("No object identifier with that id found."));
        }
        return Ok(new ApiResponse<ObjectIdentifier>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectIdentifier>() { objIdentifier }
        });
    }

    /****************************************************************
    * CREATE a new identifier for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/identifiers")]
    [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
    
    public async Task<IActionResult> CreateObjectIdentifier(string sd_oid,
        [FromBody] ObjectIdentifier objIdentContent)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectIdentifier>);
        }
        objIdentContent.SdOid = sd_oid; 
        var objIdent = await _objectService.CreateObjectIdentifierAsync(objIdentContent);
        if (objIdent == null) 
        {
            return Ok(ErrorInActionResponse<ObjectIdentifier>("Error during object identifier creation."));
        } 
        return Ok(new ApiResponse<ObjectIdentifier>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data =  new List<ObjectIdentifier>() { objIdent }
        });
    }
    
    /****************************************************************
    * UPDATE a single specified object identifier
    ****************************************************************/

    [HttpPut("data-objects/{sd_oid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
    
    public async Task<IActionResult> UpdateObjectIdentifier(string sd_oid, int id, 
        [FromBody] ObjectIdentifier objIdentContent)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectIdentifier", id))
        {
            return Ok(ErrorInActionResponse<ObjectIdentifier>("No identifier with that id found for specified object."));
        }
        var updatedObjectIdentifier = await _objectService.UpdateObjectIdentifierAsync(id, objIdentContent);
        if (updatedObjectIdentifier == null)
        {
            return Ok(ErrorInActionResponse<ObjectDate>("Error during object identifier update."));
        }
        return Ok(new ApiResponse<ObjectIdentifier>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectIdentifier>() { updatedObjectIdentifier }
        });
    }
    
    /****************************************************************
    * DELETE a single specified object identifier
    ****************************************************************/
    
    [HttpDelete("data-objects/{sd_oid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
    
    public async Task<IActionResult> DeleteObjectIdentifier(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectIdentifier", id))
        {
            return Ok(ErrorInActionResponse<ObjectIdentifier>("No identifier with that id found for specified object."));
        }
        var count = await _objectService.DeleteObjectIdentifierAsync(id);
        return Ok(new ApiResponse<ObjectIdentifier>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Object identifier has been removed." }, Data = null
        });
    }
}
