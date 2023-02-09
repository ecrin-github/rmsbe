using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectDatesApiController : BaseBrowsingApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public ObjectDatesApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectDate";
        _attType = "object date"; _attTypes = "object dates";
    }

    /****************************************************************
    * FETCH ALL dates for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/dates")]
    [SwaggerOperation(Tags = new[] { "Browsing - Object dates endpoint" })]
    
    public async Task<IActionResult> GetObjectDates(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objDates = await _objectService.GetObjectDates(sdOid);
            return objDates != null
                ? Ok(ListSuccessResponse(objDates.Count, objDates))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }

    /****************************************************************
    * FETCH A SINGLE object date
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/dates/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Browsing - Object dates endpoint" })]
    
    public async Task<IActionResult> GetObjectDate(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objDate = await _objectService.GetObjectDate(id);
            return objDate != null
                ? Ok(SingleSuccessResponse(new List<ObjectDate>() { objDate }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}