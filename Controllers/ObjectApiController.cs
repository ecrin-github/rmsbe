using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class ObjectApiController : BaseApiController
{
    private readonly IObjectDataService _objectService;

    public ObjectApiController(IObjectDataService objectDataService)
    {
        _objectService = objectDataService ?? throw new ArgumentNullException(nameof(objectDataService));
    }
    
    /****************************************************************
    * FETCH ALL data objects (including attribute data)
    ****************************************************************/
    
    [HttpGet("data-objects")]
    [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
    
    public async Task<IActionResult> GetAllDataObjects()
    {
        var fullDataObjects = await _objectService.GetAllFullObjectsAsync();
        if (fullDataObjects == null || fullDataObjects.Count == 0)
        {
            return Ok(NoAttributesResponse<FullDataObject>("No data objects have been found."));
        }
        return Ok(new ApiResponse<FullDataObject>()
        {
            Total = fullDataObjects.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = fullDataObjects
        });
    }
    
    /****************************************************************
    * FETCH a specific data object (including attribute data)
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
    
    public async Task<IActionResult> GetObjectById(string sd_oid)
    {
        var fullDdataObject = await _objectService.GetFullObjectByIdAsync(sd_oid);
        if (fullDdataObject == null) 
        {
            return Ok(NoAttributesResponse<FullDataObject>("No data object found with that id."));
        }   
        return Ok(new ApiResponse<FullDataObject>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<FullDataObject>() { fullDdataObject }
        });
    }
    
    /****************************************************************
    * DELETE a specific data object (including all attribute data)
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
    
    public async Task<IActionResult> DeleteDataObject(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<FullDataObject>);
        }
        var count = await _objectService.DeleteFullObjectAsync(sd_oid);
        return Ok(new ApiResponse<FullDataObject>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Full data object has been removed." }, Data = null
        });
    }
}