using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectRelationshipsApiController : BaseApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public ObjectRelationshipsApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectRelationship";
        _attType = "object relationship"; _attTypes = "object relationships";
    }
    
    /****************************************************************
    * FETCH ALL relationships for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/relationships")]
    [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
    
    public async Task<IActionResult> GetObjectRelationships(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objRels = await _objectService.GetObjectRelationships(sdOid);
            return objRels != null
                ? Ok(ListSuccessResponse(objRels.Count, objRels))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object relationship
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
    
    public async Task<IActionResult> GetObjectRelationship(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objRel = await _objectService.GetObjectRelationship(id);
            return objRel != null
                ? Ok(SingleSuccessResponse(new List<ObjectRelationship>() { objRel }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new relationship for a specified object
    ****************************************************************/

    [HttpPost("data-objects/{sdOid}/relationships")]
    [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
    
    public async Task<IActionResult> CreateObjectRelationship(string sdOid,
                 [FromBody] ObjectRelationship objRelationshipContent)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            objRelationshipContent.SdOid = sdOid; 
            var objRel = await _objectService.CreateObjectRelationship(objRelationshipContent);
            return objRel != null
                ? Ok(SingleSuccessResponse(new List<ObjectRelationship>() { objRel }))
                : Ok(ErrorResponse("c", _attType, _parType, sdOid, sdOid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object relationship
    ****************************************************************/

    [HttpPut("data-objects/{sdOid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
    
    public async Task<IActionResult> UpdateObjectRelationship(string sdOid, int id, 
                 [FromBody] ObjectRelationship objRelationshipContent)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var updatedObjectRel = await _objectService.UpdateObjectRelationship(id, objRelationshipContent);
            return updatedObjectRel != null
                ? Ok(SingleSuccessResponse(new List<ObjectRelationship>() { updatedObjectRel }))
                : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a single specified object relationship
    ****************************************************************/

    [HttpDelete("data-objects/{sdOid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
    
    public async Task<IActionResult> DeleteObjectRelationship(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var count = await _objectService.DeleteObjectRelationship(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}