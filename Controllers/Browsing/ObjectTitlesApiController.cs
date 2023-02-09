using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectTitlesApiController : BaseBrowsingApiController
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
    
    [HttpGet("data-objects/{sdOid}/titles")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> GetObjectTitles(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objTitles = await _objectService.GetObjectTitles(sdOid);
            return objTitles != null
                ? Ok(ListSuccessResponse(objTitles.Count, objTitles))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object title
    ****************************************************************/

    [HttpGet("data-objects/{sdOid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> GetObjectTitle(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objTitle = await _objectService.GetObjectTitle(id);
            return objTitle != null
                ? Ok(SingleSuccessResponse(new List<ObjectTitle>() { objTitle }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new title for a specified object
    ****************************************************************/

    [HttpPost("data-objects/{sdOid}/titles")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> CreateObjectTitle(string sdOid,
                 [FromBody] ObjectTitle objTitleContent)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            objTitleContent.SdOid = sdOid;   // ensure this is the case
            var objTitle = await _objectService.CreateObjectTitle(objTitleContent);
            return objTitle != null
                ? Ok(SingleSuccessResponse(new List<ObjectTitle>() { objTitle }))
                : Ok(ErrorResponse("c", _attType, _parType, sdOid, sdOid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object title
    ****************************************************************/

    [HttpPut("data-objects/{sdOid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> UpdateObjectTitle(string sdOid, int id, 
                 [FromBody] ObjectTitle objTitleContent)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            objTitleContent.SdOid = sdOid;  // ensure this is the case
            objTitleContent.Id = id;
            var updatedObjectTitle = await _objectService.UpdateObjectTitle(objTitleContent);
            return updatedObjectTitle != null
                    ? Ok(SingleSuccessResponse(new List<ObjectTitle>() { updatedObjectTitle }))
                    : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
        
    /****************************************************************
    * DELETE a single specified object title
    ****************************************************************/

    [HttpDelete("data-objects/{sdOid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> DeleteObjectTitle(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
             var count = await _objectService.DeleteObjectTitle(id);
             return count > 0
                 ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                 : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}