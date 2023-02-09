using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectIdentifiersApiController : BaseBrowsingApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public ObjectIdentifiersApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectIdentifier";
        _attType = "object identifier"; _attTypes = "object identifiers";
    }
    
    /****************************************************************
    * FETCH ALL identifiers for a specified object
    ****************************************************************/

    [HttpGet("data-objects/{sdOid}/identifiers")]
    [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
    
    public async Task<IActionResult> GetObjectIdentifiers(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objIdentifiers = await _objectService.GetObjectIdentifiers(sdOid);
            return objIdentifiers != null
                ? Ok(ListSuccessResponse(objIdentifiers.Count, objIdentifiers))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }

    /****************************************************************
    * FETCH A SINGLE object identifier
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
    
    public async Task<IActionResult> GetObjectIdentifier(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objIdentifier = await _objectService.GetObjectIdentifier(id);
            return objIdentifier != null
                ? Ok(SingleSuccessResponse(new List<ObjectIdentifier>() { objIdentifier }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new identifier for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sdOid}/identifiers")]
    [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
    
    public async Task<IActionResult> CreateObjectIdentifier(string sdOid,
                 [FromBody] ObjectIdentifier objIdentContent)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            objIdentContent.SdOid = sdOid;   // ensure this is the case
            var objIdent = await _objectService.CreateObjectIdentifier(objIdentContent);
            return objIdent != null
                ? Ok(SingleSuccessResponse(new List<ObjectIdentifier>() { objIdent }))
                : Ok(ErrorResponse("c", _attType, _parType, sdOid, sdOid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
   
    /****************************************************************
    * UPDATE a single specified object identifier
    ****************************************************************/

    [HttpPut("data-objects/{sdOid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
    
    public async Task<IActionResult> UpdateObjectIdentifier(string sdOid, int id, 
                 [FromBody] ObjectIdentifier objIdentContent)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            objIdentContent.SdOid = sdOid;  // ensure this is the case
            objIdentContent.Id = id;
            var updatedObjectIdent = await _objectService.UpdateObjectIdentifier(objIdentContent);
            return updatedObjectIdent != null
                ? Ok(SingleSuccessResponse(new List<ObjectIdentifier>() { updatedObjectIdent }))
                : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }

    /****************************************************************
    * DELETE a single specified object identifier
    ****************************************************************/
    
    [HttpDelete("data-objects/{sdOid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
    
    public async Task<IActionResult> DeleteObjectIdentifier(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var count = await _objectService.DeleteObjectIdentifier(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}