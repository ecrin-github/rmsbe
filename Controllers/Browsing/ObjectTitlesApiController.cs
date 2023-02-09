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
    [SwaggerOperation(Tags = new []{"Browsing - Object titles endpoint"})]
    
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
    [SwaggerOperation(Tags = new []{"Browsing - Object titles endpoint"})]
    
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
}