using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectContributorsApiController : BaseBrowsingApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public ObjectContributorsApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectContributor";
        _attType = "object contributor"; _attTypes = "object contributors";
    }

    /****************************************************************
    * FETCH ALL contributors for a specified object
    ****************************************************************/

    [HttpGet("data-objects/{sdOid}/contributors")]
    [SwaggerOperation(Tags = new []{"Browsing - Object contributors endpoint"})]
    
    public async Task<IActionResult> GetObjectContributors(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objectContributors = await _objectService.GetObjectContributors(sdOid);
            return objectContributors != null
                ? Ok(ListSuccessResponse(objectContributors.Count, objectContributors))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }
        
    /****************************************************************
    * FETCH A SINGLE object contributor
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Browsing - Object contributors endpoint"})]
    
    public async Task<IActionResult> GetObjectContributor(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objContrib = await _objectService.GetObjectContributor(id);
            return objContrib != null
                ? Ok(SingleSuccessResponse(new List<ObjectContributor>() { objContrib }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}