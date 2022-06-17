using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectDescriptionsApiController : BaseApiController
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

    [HttpGet("data-objects/{sd_oid}/descriptions")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
    public async Task<IActionResult> GetObjectDescriptions(string sd_oid)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            var objDescriptions = await _objectService.GetObjectDescriptionsAsync(sd_oid);
            return objDescriptions != null
                ? Ok(ListSuccessResponse(objDescriptions.Count, objDescriptions))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object description
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/descriptions/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
    public async Task<IActionResult> GetObjectDescription(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var objDesc = await _objectService.GetObjectDescriptionAsync(id);
            return objDesc != null
                ? Ok(SingleSuccessResponse(new List<ObjectDescription>() { objDesc }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new description for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/descriptions")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
    public async Task<IActionResult> CreateObjectDescription(string sd_oid, 
                 [FromBody] ObjectDescription objectDescContent)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            objectDescContent.SdOid = sd_oid; 
            var objDesc = await _objectService.CreateObjectDescriptionAsync(objectDescContent);
            return objDesc != null
                ? Ok(SingleSuccessResponse(new List<ObjectDescription>() { objDesc }))
                : Ok(ErrorResponse("c", _attType, _parType, sd_oid, sd_oid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
   
    /****************************************************************
    * UPDATE a single specified object description
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/descriptions/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    public async Task<IActionResult> UpdateObjectDescription(string sd_oid, int id, 
                 [FromBody] ObjectDescription objectDescContent)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
             var objDesc = await _objectService.UpdateObjectDescriptionAsync(id, objectDescContent);
             return objDesc != null
                 ? Ok(SingleSuccessResponse(new List<ObjectDescription>() { objDesc }))
                 : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }

    /****************************************************************
    * DELETE a single specified object description
    ****************************************************************/
    
    [HttpDelete("data-objects/{sd_oid}/descriptions/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
    public async Task<IActionResult> DeleteObjectDescription(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var count = await _objectService.DeleteObjectDescriptionAsync(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}