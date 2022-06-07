using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class ObjectTitlesApiController : BaseApiController
{
    private readonly IObjectDataService _objectService;

    public ObjectTitlesApiController(IObjectDataService objectDataService)
    {
        _objectService = objectDataService ?? throw new ArgumentNullException(nameof(objectDataService));
    }
    
    /****************************************************************
    * FETCH ALL titles for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/titles")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> GetObjectTitles(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectTitle>);
        }
        var objTitles = await _objectService.GetObjectTitlesAsync(sd_oid);
        if (objTitles == null || objTitles.Count == 0)
        {
            return Ok(NoAttributesResponse<ObjectTitle>("No object titles were found."));
        }
        return Ok(new ApiResponse<ObjectTitle>()
        {
            Total = objTitles.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = objTitles
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE object title
    ****************************************************************/

    [HttpGet("data-objects/{sd_oid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> GetObjectTitle(string sd_oid, int id)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectTitle>);
        }
        var objTitle = await _objectService.GetObjectTitleAsync(id);
        if (objTitle == null) 
        {
            return Ok(NoAttributesResponse<ObjectTitle>("No object title with that id found."));
        }   
        return Ok(new ApiResponse<ObjectTitle>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectTitle>() { objTitle }
        });
    }

    /****************************************************************
    * CREATE a new title for a specified object
    ****************************************************************/

    [HttpPost("data-objects/{sd_oid}/titles")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> CreateObjectTitle(string sd_oid,
        [FromBody] ObjectTitle objTitleContent)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectTitle>);
        }
        objTitleContent.SdOid = sd_oid; 
        var objTitle = await _objectService.CreateObjectTitleAsync(objTitleContent);
        if (objTitle == null)
        {
            return Ok(ErrorInActionResponse<ObjectTitle>("Error during object title creation."));
        }    
        return Ok(new ApiResponse<ObjectTitle>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectTitle>() { objTitle }
        });
    }
    
    /****************************************************************
    * UPDATE a single specified object title
    ****************************************************************/

    [HttpPut("data-objects/{sd_oid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> UpdateObjectTitle(string sd_oid, int id, 
        [FromBody] ObjectTitle objTitleContent)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectTitle", id))
        {
            return Ok(ErrorInActionResponse<ObjectTitle>("No title with that id found for specified object."));
        }
        var updatedObjectTitle = await _objectService.UpdateObjectTitleAsync(id, objTitleContent);
        if (updatedObjectTitle == null)
        {
            return Ok(ErrorInActionResponse<ObjectTitle>("Error during object title update."));
        }    
        return Ok(new ApiResponse<ObjectTitle>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectTitle>() { updatedObjectTitle }
        });
    }
    
    /****************************************************************
    * DELETE a single specified object title
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object titles endpoint"})]
    
    public async Task<IActionResult> DeleteObjectTitle(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectTitle", id))
        {
            return Ok(ErrorInActionResponse<ObjectTitle>("No title with that id found for specified object."));
        }
        var count = await _objectService.DeleteObjectTitleAsync(id);
        return Ok(new ApiResponse<ObjectTitle>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Object title has been removed." }, Data = null
        });
    }
}