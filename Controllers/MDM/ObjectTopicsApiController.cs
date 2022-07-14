using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectTopicsApiController : BaseApiController
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
    
    [HttpGet("data-objects/{sd_oid}/topics")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> GetObjectTopics(string sd_oid)
    {
        if (await _objectService.ObjectExists(sd_oid)) {
            var objTopics = await _objectService.GetObjectTopics(sd_oid);
            return objTopics != null
                ? Ok(ListSuccessResponse(objTopics.Count, objTopics))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object topic
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> GetObjectTopic(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sd_oid, _entityType, id)) {
            var objTopic = await _objectService.GetObjectTopic(id);
            return objTopic != null
                ? Ok(SingleSuccessResponse(new List<ObjectTopic>() { objTopic }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new topic for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/topics")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> CreateObjectTopic(string sd_oid,
                 [FromBody] ObjectTopic objTopicContent)
    {
        if (await _objectService.ObjectExists(sd_oid)) {
            objTopicContent.SdOid = sd_oid; 
            var objTopic = await _objectService.CreateObjectTopic(objTopicContent);
            return objTopic != null
                ? Ok(SingleSuccessResponse(new List<ObjectTopic>() { objTopic }))
                : Ok(ErrorResponse("c", _attType, _parType, sd_oid, sd_oid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object topic
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> UpdateObjectTopic(string sd_oid, int id, 
                 [FromBody] ObjectTopic objTopicContent)
    {
        if (await _objectService.ObjectAttributeExists(sd_oid, _entityType, id)) {
            var updatedObjectTopic = await _objectService.UpdateObjectTopic(id, objTopicContent);
            return updatedObjectTopic != null
                ? Ok(SingleSuccessResponse(new List<ObjectTopic>() { updatedObjectTopic }))
                : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
   
    /****************************************************************
    * DELETE a single specified object topic
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> DeleteObjectTopic(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sd_oid, _entityType, id)) {
            var count = await _objectService.DeleteObjectTopic(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}