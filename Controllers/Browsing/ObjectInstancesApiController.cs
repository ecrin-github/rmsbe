using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectInstancesApiController : BaseBrowsingApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public ObjectInstancesApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectInstance";
        _attType = "object instance"; _attTypes = "object instances";
    }
    
    /****************************************************************
    * FETCH ALL instances for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/instances")]
    [SwaggerOperation(Tags = new []{"Browsing - Object instances endpoint"})]
    
    public async Task<IActionResult> GetObjectInstances(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objInstances = await _objectService.GetObjectInstances(sdOid);
            return objInstances != null
                ? Ok(ListSuccessResponse(objInstances.Count, objInstances))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object instance
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/instances/{id:int}")]
    [SwaggerOperation(Tags = new []{"Browsing - Object instances endpoint"})]
    
    public async Task<IActionResult> GetObjectInstance(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objInstance = await _objectService.GetObjectInstance(id);
            return objInstance != null
                ? Ok(SingleSuccessResponse(new List<ObjectInstance>() { objInstance }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}