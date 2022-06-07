using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class ObjectTopicsApiController : BaseApiController
{
    private readonly IObjectDataService _objectService;

    public ObjectTopicsApiController(IObjectDataService objectDataService)
    {
        _objectService = objectDataService ?? throw new ArgumentNullException(nameof(objectDataService));
    }
    
    /****************************************************************
    * FETCH ALL topics for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/topics")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> GetObjectTopics(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectTopic>);
        }
        var objTopics = await _objectService.GetObjectTopicsAsync(sd_oid);
        if (objTopics == null || objTopics.Count == 0)
        {
            return Ok(NoAttributesResponse<ObjectTopic>("No object topics were found."));
        }
        return Ok(new ApiResponse<ObjectTopic>()
        {
            Total = objTopics.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = objTopics
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE object topic
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> GetObjectTopic(string sd_oid, int id)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectTopic>);
        }
        var objTopic = await _objectService.GetObjectTopicAsync(id);
        if (objTopic == null) 
        {
            return Ok(NoAttributesResponse<ObjectTopic>("No object topic with that id found."));
        }     
        return Ok(new ApiResponse<ObjectTopic>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectTopic>() { objTopic }
        });
    }
    
    /****************************************************************
    * CREATE a new topic for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/topics")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> CreateObjectTopic(string sd_oid,
        [FromBody] ObjectTopic objTopicContent)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectTopic>);
        }
        objTopicContent.SdOid = sd_oid; 
        var objTopic = await _objectService.CreateObjectTopicAsync(objTopicContent);
        if (objTopic == null) 
        {
            return Ok(ErrorInActionResponse<ObjectTopic>("Error during object topic creation."));
        }    
        return Ok(new ApiResponse<ObjectTopic>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectTopic>() { objTopic }
        });
    }
    
    /****************************************************************
    * UPDATE a single specified object topic
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> UpdateObjectTopic(string sd_oid, int id, 
        [FromBody] ObjectTopic objTopicContent)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectTopic", id))
        {
            return Ok(ErrorInActionResponse<ObjectTopic>("No topic with that id found for specified object."));
        }
        var updatedObjectTopic = await _objectService.UpdateObjectTopicAsync(id, objTopicContent);
        if (updatedObjectTopic == null)
        {
            return Ok(ErrorInActionResponse<ObjectTopic>("Error during object topic update."));
        }    
        return Ok(new ApiResponse<ObjectTopic>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectTopic>() { updatedObjectTopic }
        });
    }
    
    /****************************************************************
    * DELETE a single specified object topic
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
    
    public async Task<IActionResult> DeleteObjectTopic(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectTopic", id))
        {
            return Ok(ErrorInActionResponse<ObjectTopic>("No topic with that id found for specified object."));
        }
        var count = await _objectService.DeleteObjectTopicAsync(id);
        return Ok(new ApiResponse<ObjectTopic>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Object topic has been removed." }, Data = null
        });
    }
}