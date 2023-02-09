using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectTopicsApiController : BaseBrowsingApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public ObjectTopicsApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectTopic";
        _attType = "object topic"; _attTypes = "object topics";
    }
    
    /****************************************************************
    * FETCH ALL topics for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/topics")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> GetObjectTopics(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objTopics = await _objectService.GetObjectTopics(sdOid);
            return objTopics != null
                ? Ok(ListSuccessResponse(objTopics.Count, objTopics))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object topic
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> GetObjectTopic(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objTopic = await _objectService.GetObjectTopic(id);
            return objTopic != null
                ? Ok(SingleSuccessResponse(new List<ObjectTopic>() { objTopic }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new topic for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sdOid}/topics")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> CreateObjectTopic(string sdOid,
                 [FromBody] ObjectTopic objTopicContent)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            objTopicContent.SdOid = sdOid;   // ensure this is the case
            var objTopic = await _objectService.CreateObjectTopic(objTopicContent);
            return objTopic != null
                ? Ok(SingleSuccessResponse(new List<ObjectTopic>() { objTopic }))
                : Ok(ErrorResponse("c", _attType, _parType, sdOid, sdOid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object topic
    ****************************************************************/
    
    [HttpPut("data-objects/{sdOid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> UpdateObjectTopic(string sdOid, int id, 
                 [FromBody] ObjectTopic objTopicContent)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            objTopicContent.SdOid = sdOid;  // ensure this is the case
            objTopicContent.Id = id;
            var updatedObjectTopic = await _objectService.UpdateObjectTopic(objTopicContent);
            return updatedObjectTopic != null
                ? Ok(SingleSuccessResponse(new List<ObjectTopic>() { updatedObjectTopic }))
                : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
   
    /****************************************************************
    * DELETE a single specified object topic
    ****************************************************************/

    [HttpDelete("data-objects/{sdOid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> DeleteObjectTopic(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var count = await _objectService.DeleteObjectTopic(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}