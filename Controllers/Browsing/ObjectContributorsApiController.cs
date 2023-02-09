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
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
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
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
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
    
    /****************************************************************
    * CREATE a new contributor for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sdOid}/contributors")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> CreateObjectContributor(string sdOid,
                 [FromBody] ObjectContributor objectContributorContent)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            objectContributorContent.SdOid = sdOid;    // ensure this is the case
            var objContrib = await _objectService.CreateObjectContributor(objectContributorContent);
            return objContrib != null
                ? Ok(SingleSuccessResponse(new List<ObjectContributor>() { objContrib }))
                : Ok(ErrorResponse("c", _attType, _parType, sdOid, sdOid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object contributor
    ****************************************************************/
    
    [HttpPut("data-objects/{sdOid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> UpdateObjectContributor(string sdOid, int id, 
                 [FromBody] ObjectContributor objectContContent)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            objectContContent.SdOid = sdOid;  // ensure this is the case
            objectContContent.Id = id;
            var objContrib = await _objectService.UpdateObjectContributor(objectContContent);
            return objContrib != null
                ? Ok(SingleSuccessResponse(new List<ObjectContributor>() { objContrib }))
                : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a single specified object contributor
    ****************************************************************/

    [HttpDelete("data-objects/{sdOid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> DeleteObjectContributor(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var count = await _objectService.DeleteObjectContributor(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}