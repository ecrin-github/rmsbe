using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class ObjectContributorsApiController : BaseApiController
{
    private readonly IObjectDataService _objectService;

    public ObjectContributorsApiController(IObjectDataService objectDataService)
    {
        _objectService = objectDataService ?? throw new ArgumentNullException(nameof(objectDataService));
    }

    /****************************************************************
    * FETCH ALL contributors for a specified object
    ****************************************************************/

    [HttpGet("data-objects/{sd_oid}/contributors")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> GetObjectContributors(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectContributor>);
        }
        var objectContributors = await _objectService.GetObjectContributorsAsync(sd_oid);
        if (objectContributors == null|| objectContributors.Count == 0)
        {
            return Ok(NoAttributesResponse<ObjectDate>("No object contributors were found."));
        }
        return Ok(new ApiResponse<ObjectContributor>()
        {
            Total = objectContributors.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = objectContributors
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE object contributor
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> GetObjectContributor(string sd_oid, int id)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectContributor>);
        }
        var objContrib = await _objectService.GetObjectContributorAsync(id);
        if (objContrib == null) 
        {
            return Ok(NoAttributesResponse<ObjectDate>("No object contributor with that id found."));
        }    
        return Ok(new ApiResponse<ObjectContributor>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectContributor>() { objContrib }
        });
    }
    
    /****************************************************************
    * CREATE a new contributor for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/contributors")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> CreateObjectContributor(string sd_oid,
        [FromBody] ObjectContributor objectContributorContent)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectContributor>);
        }
        objectContributorContent.SdOid = sd_oid; 
        var objContrib = await _objectService.CreateObjectContributorAsync(objectContributorContent);
        if (objContrib == null)
        {
            return Ok(ErrorInActionResponse<ObjectContributor>("Error during object contributor creation."));
        }
        return Ok(new ApiResponse<ObjectContributor>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectContributor>() { objContrib }
        });
    }

    /****************************************************************
    * UPDATE a single specified object contributor
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> UpdateObjectContributor(string sd_oid, int id, 
        [FromBody] ObjectContributor objectContributorContent)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectContributor", id))
        {
            return Ok(ErrorInActionResponse<ObjectContributor>("No contributor with that id found for specified object."));
        }
        var updatedObjContrib = await _objectService.UpdateObjectContributorAsync(id, objectContributorContent);
        if (updatedObjContrib == null)
        {
            return Ok(ErrorInActionResponse<ObjectContributor>("Error during object contributor update."));
        }    
       return Ok(new ApiResponse<ObjectContributor>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectContributor>() { updatedObjContrib }
        });
    }
    
    /****************************************************************
    * DELETE a single specified object contributor
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object contributors endpoint"})]
    
    public async Task<IActionResult> DeleteObjectContributor(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectContributor", id))
        {
            return Ok(ErrorInActionResponse<ObjectContributor>("No contributor with that id found for specified object."));
        }
        var count = await _objectService.DeleteObjectContributorAsync(id);
        return Ok(new ApiResponse<ObjectContributor>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Object contributor has been removed." }, Data = null
        });
    }
}