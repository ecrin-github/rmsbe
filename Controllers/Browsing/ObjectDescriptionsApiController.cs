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
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
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
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
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
    
    /****************************************************************
    * CREATE a new description for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sdOid}/descriptions")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
    public async Task<IActionResult> CreateObjectDescription(string sdOid, 
                 [FromBody] ObjectDescription objectDescContent)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            objectDescContent.SdOid = sdOid;   // ensure this is the case
            var objDesc = await _objectService.CreateObjectDescription(objectDescContent);
            return objDesc != null
                ? Ok(SingleSuccessResponse(new List<ObjectDescription>() { objDesc }))
                : Ok(ErrorResponse("c", _attType, _parType, sdOid, sdOid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
   
    /****************************************************************
    * UPDATE a single specified object description
    ****************************************************************/
    
    [HttpPut("data-objects/{sdOid}/descriptions/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    public async Task<IActionResult> UpdateObjectDescription(string sdOid, int id, 
                 [FromBody] ObjectDescription objectDescContent)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            objectDescContent.SdOid = sdOid;  // ensure this is the case
            objectDescContent.Id = id;
             var objDesc = await _objectService.UpdateObjectDescription(objectDescContent);
             return objDesc != null
                 ? Ok(SingleSuccessResponse(new List<ObjectDescription>() { objDesc }))
                 : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }

    /****************************************************************
    * DELETE a single specified object description
    ****************************************************************/
    
    [HttpDelete("data-objects/{sdOid}/descriptions/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
    
    public async Task<IActionResult> DeleteObjectDescription(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var count = await _objectService.DeleteObjectDescription(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}