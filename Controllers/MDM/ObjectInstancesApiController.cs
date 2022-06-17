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
    
    [HttpGet("data-objects/{sd_oid}/instances")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> GetObjectInstances(string sd_oid)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            var objInstances = await _objectService.GetObjectInstancesAsync(sd_oid);
            return objInstances != null
                ? Ok(ListSuccessResponse(objInstances.Count, objInstances))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object instance
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/instances/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> GetObjectInstance(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var objInstance = await _objectService.GetObjectInstanceAsync(id);
            return objInstance != null
                ? Ok(SingleSuccessResponse(new List<ObjectInstance>() { objInstance }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new instance for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/instances")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> CreateObjectInstance(string sd_oid,
                 [FromBody] ObjectInstance objInstanceContent)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            objInstanceContent.SdOid = sd_oid; 
            var objInstance = await _objectService.CreateObjectInstanceAsync(objInstanceContent);
            return objInstance != null
                ? Ok(SingleSuccessResponse(new List<ObjectInstance>() { objInstance }))
                : Ok(ErrorResponse("c", _attType, _parType, sd_oid, sd_oid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object instance
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/instances/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> UpdateObjectInstance(string sd_oid, int id, 
                 [FromBody] ObjectInstance objInstanceContent)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var updatedObjInst = await _objectService.UpdateObjectInstanceAsync(id, objInstanceContent);
            return updatedObjInst != null
                ? Ok(SingleSuccessResponse(new List<ObjectInstance>() { updatedObjInst }))
                : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }

    /****************************************************************
    * DELETE a single specified object instance
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}/instances/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
    
    public async Task<IActionResult> DeleteObjectInstance(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var count = await _objectService.DeleteObjectInstanceAsync(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}