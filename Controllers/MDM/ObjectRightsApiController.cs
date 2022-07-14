using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectRightsApiController : BaseApiController
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
    
    [HttpGet("data-objects/{sd_oid}/rights")]
    [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
    
    public async Task<IActionResult> GetObjectRights(string sd_oid)
    {
        if (await _objectService.ObjectExists(sd_oid)) {
            var objRights = await _objectService.GetObjectRights(sd_oid);
            return objRights != null
                ? Ok(ListSuccessResponse(objRights.Count, objRights))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object right
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/rights/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
    
    public async Task<IActionResult> GetObjectRight(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sd_oid, _entityType, id)) {
            var objRight = await _objectService.GetObjectRight(id);
            return objRight != null
                ? Ok(SingleSuccessResponse(new List<ObjectRight>() { objRight }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
   
    /****************************************************************
    * CREATE a new right for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/rights")]
    [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
    
    public async Task<IActionResult> CreateObjectRight(string sd_oid,
                 [FromBody] ObjectRight objectRightContent)
    {
        if (await _objectService.ObjectExists(sd_oid)) {
            objectRightContent.SdOid = sd_oid;
            var objRight = await _objectService.CreateObjectRight(objectRightContent);
            return objRight != null
                ? Ok(SingleSuccessResponse(new List<ObjectRight>() { objRight }))
                : Ok(ErrorResponse("c", _attType, _parType, sd_oid, sd_oid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
     
    /****************************************************************
    * UPDATE a single specified object right
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/rights/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
    
    public async Task<IActionResult> UpdateObjectRight(string sd_oid, int id, 
                 [FromBody] ObjectRight objectRightContent)
    {
        if (await _objectService.ObjectAttributeExists(sd_oid, _entityType, id)) {
            var updatedObjRight = await _objectService.UpdateObjectRight(id, objectRightContent);
            return updatedObjRight != null
                ? Ok(SingleSuccessResponse(new List<ObjectRight>() { updatedObjRight }))
                : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }

    /****************************************************************
    * DELETE a single specified object right
    ****************************************************************/
    
    [HttpDelete("data-objects/{sd_oid}/rights/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
    
    public async Task<IActionResult> DeleteObjectRight(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sd_oid, _entityType, id)) {
            var count = await _objectService.DeleteObjectRight(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}