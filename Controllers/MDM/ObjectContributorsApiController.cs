using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectContributorsApiController : BaseApiController
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

    [HttpGet("data-objects/{sd_oid}/contributors")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> GetObjectContributors(string sd_oid)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            var objectContributors = await _objectService.GetObjectContributorsAsync(sd_oid);
            return objectContributors != null
                ? Ok(ListSuccessResponse(objectContributors.Count, objectContributors))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));    
    }
        
    /****************************************************************
    * FETCH A SINGLE object contributor
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> GetObjectContributor(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var objContrib = await _objectService.GetObjectContributorAsync(id);
            return objContrib != null
                ? Ok(SingleSuccessResponse(new List<ObjectContributor>() { objContrib }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new contributor for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/contributors")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> CreateObjectContributor(string sd_oid,
                 [FromBody] ObjectContributor objectContributorContent)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            objectContributorContent.SdOid = sd_oid; 
            var objContrib = await _objectService.CreateObjectContributorAsync(objectContributorContent);
            return objContrib != null
                ? Ok(SingleSuccessResponse(new List<ObjectContributor>() { objContrib }))
                : Ok(ErrorResponse("c", _attType, _parType, sd_oid, sd_oid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object contributor
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> UpdateObjectContributor(string sd_oid, int id, 
                 [FromBody] ObjectContributor objectContContent)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var objContrib = await _objectService.UpdateObjectContributorAsync(id, objectContContent);
            return objContrib != null
                ? Ok(SingleSuccessResponse(new List<ObjectContributor>() { objContrib }))
                : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a single specified object contributor
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> DeleteObjectContributor(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var count = await _objectService.DeleteObjectContributorAsync(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}