using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectDescriptionsApiController : BaseBrowsingApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public ObjectDescriptionsApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectDescription";
        _attType = "object description"; _attTypes = "object descriptions";
    }
    
    /****************************************************************
    * FETCH ALL descriptions for a specified object
    ****************************************************************/

    [HttpGet("data-objects/{sdOid}/descriptions")]
    [SwaggerOperation(Tags = new []{"Browsing - Object descriptions endpoint"})]
    
    public async Task<IActionResult> GetObjectDescriptions(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objDescriptions = await _objectService.GetObjectDescriptions(sdOid);
            return objDescriptions != null
                ? Ok(ListSuccessResponse(objDescriptions.Count, objDescriptions))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object description
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/descriptions/{id:int}")]
    [SwaggerOperation(Tags = new []{"Browsing - Object descriptions endpoint"})]
    
    public async Task<IActionResult> GetObjectDescription(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objDesc = await _objectService.GetObjectDescription(id);
            return objDesc != null
                ? Ok(SingleSuccessResponse(new List<ObjectDescription>() { objDesc }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}