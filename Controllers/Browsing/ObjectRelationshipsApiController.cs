using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectRelationshipsApiController : BaseBrowsingApiController
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
    [SwaggerOperation(Tags = new []{"Browsing - Object relationships endpoint"})]
    
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
    [SwaggerOperation(Tags = new []{"Browsing - Object relationships endpoint"})]
    
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
}