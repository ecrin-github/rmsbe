using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectTitlesApiController : BaseApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public ObjectTitlesApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectTitle";
        _attType = "object title"; _attTypes = "object titles";
    }
    
    /****************************************************************
    * FETCH ALL titles for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/titles")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> GetObjectTitles(string sd_oid)
    {
        if (await _objectService.ObjectExists(sd_oid)) {
            var objTitles = await _objectService.GetObjectTitles(sd_oid);
            return objTitles != null
                ? Ok(ListSuccessResponse(objTitles.Count, objTitles))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object title
    ****************************************************************/

    [HttpGet("data-objects/{sd_oid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> GetObjectTitle(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sd_oid, _entityType, id)) {
            var objTitle = await _objectService.GetObjectTitle(id);
            return objTitle != null
                ? Ok(SingleSuccessResponse(new List<ObjectTitle>() { objTitle }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new title for a specified object
    ****************************************************************/

    [HttpPost("data-objects/{sd_oid}/titles")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> CreateObjectTitle(string sd_oid,
                 [FromBody] ObjectTitle objTitleContent)
    {
        if (await _objectService.ObjectExists(sd_oid)) {
            objTitleContent.SdOid = sd_oid; 
            var objTitle = await _objectService.CreateObjectTitle(objTitleContent);
            return objTitle != null
                ? Ok(SingleSuccessResponse(new List<ObjectTitle>() { objTitle }))
                : Ok(ErrorResponse("c", _attType, _parType, sd_oid, sd_oid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object title
    ****************************************************************/

    [HttpPut("data-objects/{sd_oid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> UpdateObjectTitle(string sd_oid, int id, 
                 [FromBody] ObjectTitle objTitleContent)
    {
        if (await _objectService.ObjectAttributeExists(sd_oid, _entityType, id)) {
            var updatedObjectTitle = await _objectService.UpdateObjectTitle(id, objTitleContent);
            return updatedObjectTitle != null
                    ? Ok(SingleSuccessResponse(new List<ObjectTitle>() { updatedObjectTitle }))
                    : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
        
    /****************************************************************
    * DELETE a single specified object title
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> DeleteObjectTitle(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sd_oid, _entityType, id)) {
             var count = await _objectService.DeleteObjectTitle(id);
             return count > 0
                 ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                 : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}