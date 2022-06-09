using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectDatasetsApiController : BaseApiController
{
    private readonly IObjectService _objectService;

    public ObjectDatasetsApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }
    
    /****************************************************************
    * FETCH ALL datasets for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/datasets")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> GetObjectDatasets(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectDataset>);
        }
        var objDatasets = await _objectService.GetObjectDatasetsAsync(sd_oid);
        if (objDatasets == null|| objDatasets.Count == 0)
        {
            return Ok(NoAttributesResponse<ObjectDate>("No object datasets were found."));
        }
        return Ok(new ApiResponse<ObjectDataset>()
        {
            Total = objDatasets.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = objDatasets
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE object dataset
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> GetObjectDatasets(string sd_oid, int id)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectDataset>);
        }
        var objDataset = await _objectService.GetObjectDatasetAsync(id);
        if (objDataset == null) 
        {
            return Ok(NoAttributesResponse<ObjectDate>("No object dataset with that id found."));
        }    
        return Ok(new ApiResponse<ObjectDataset>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectDataset>() { objDataset }
        });
    }
    
    /****************************************************************
    * CREATE a new dataset for a specified object
    ****************************************************************/

    [HttpPost("data-objects/{sd_oid}/datasets")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> CreateObjectDataset(string sd_oid,
        [FromBody] ObjectDataset objectDatasetContent)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<ObjectDataset>);
        }
        objectDatasetContent.SdOid = sd_oid; 
        var objDataset = await _objectService.CreateObjectDatasetAsync(objectDatasetContent);
        if (objDataset == null)
        {
            return Ok(ErrorInActionResponse<ObjectDataset>("Error during object dataset creation."));
        }    
        return Ok(new ApiResponse<ObjectDataset>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectDataset>() { objDataset }
        });
    }
    
    /****************************************************************
    * UPDATE a single specified object dataset
    ****************************************************************/

    [HttpPut("data-objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> UpdateObjectDataset(string sd_oid, int id, 
        [FromBody] ObjectDataset objectDatasetContent)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectDataset", id))
        {
            return Ok(ErrorInActionResponse<ObjectDataset>("No dataset with that id found for specified object."));
        }
        var updatedObjDataset = await _objectService.UpdateObjectDatasetAsync(id, objectDatasetContent);
        if (updatedObjDataset == null)
        {
            return Ok(ErrorInActionResponse<ObjectDataset>("Error during object dataset update."));
        }    
        return Ok(new ApiResponse<ObjectDataset>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<ObjectDataset>() { updatedObjDataset }
        });
    }
    
    /****************************************************************
    * DELETE a single specified object dataset
    ****************************************************************/
    
    [HttpDelete("data-objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> DeleteObjectDataset(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectContributor", id))
        {
            return Ok(ErrorInActionResponse<ObjectDataset>("No contributor with that id found for specified object."));
        }
        var count = await _objectService.DeleteObjectDatasetAsync(id);
        return Ok(new ApiResponse<ObjectDataset>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Object dataset has been removed." }, Data = null
        });
    }
}