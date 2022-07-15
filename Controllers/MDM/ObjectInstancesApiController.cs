using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectInstancesApiController : BaseApiController
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
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
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
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
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
    
    /****************************************************************
    * CREATE a new instance for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sdOid}/instances")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> CreateObjectInstance(string sdOid,
                 [FromBody] ObjectInstance objInstanceContent)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            objInstanceContent.SdOid = sdOid; 
            var objInstance = await _objectService.CreateObjectInstance(objInstanceContent);
            return objInstance != null
                ? Ok(SingleSuccessResponse(new List<ObjectInstance>() { objInstance }))
                : Ok(ErrorResponse("c", _attType, _parType, sdOid, sdOid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object instance
    ****************************************************************/
    
    [HttpPut("data-objects/{sdOid}/instances/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> UpdateObjectInstance(string sdOid, int id, 
                 [FromBody] ObjectInstance objInstanceContent)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var updatedObjInst = await _objectService.UpdateObjectInstance(id, objInstanceContent);
            return updatedObjInst != null
                ? Ok(SingleSuccessResponse(new List<ObjectInstance>() { updatedObjInst }))
                : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }

    /****************************************************************
    * DELETE a single specified object instance
    ****************************************************************/

    [HttpDelete("data-objects/{sdOid}/instances/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> DeleteObjectInstance(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var count = await _objectService.DeleteObjectInstance(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}