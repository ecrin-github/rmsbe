using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectRightsApiController : BaseBrowsingApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public ObjectRightsApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectRight";
        _attType = "object right"; _attTypes = "object rights";
    }
    
    /****************************************************************
    * FETCH ALL rights for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/rights")]
    [SwaggerOperation(Tags = new []{"Browsing - Object rights endpoint"})]
    
    public async Task<IActionResult> GetObjectRights(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objRights = await _objectService.GetObjectRights(sdOid);
            return objRights != null
                ? Ok(ListSuccessResponse(objRights.Count, objRights))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object right
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/rights/{id:int}")]
    [SwaggerOperation(Tags = new []{"Browsing - Object rights endpoint"})]
    
    public async Task<IActionResult> GetObjectRight(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objRight = await _objectService.GetObjectRight(id);
            return objRight != null
                ? Ok(SingleSuccessResponse(new List<ObjectRight>() { objRight }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}