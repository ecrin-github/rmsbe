using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectDatasetsApiController : BaseApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public ObjectDatasetsApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectDataset";
        _attType = "object dataset"; _attTypes = "object datasets";
    }
    
    /****************************************************************
    * FETCH ALL datasets for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/datasets")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> GetObjectDatasets(string sd_oid)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            var objDatasets = await _objectService.GetObjectDatasetsAsync(sd_oid);
            return objDatasets != null
                ? Ok(ListSuccessResponse(objDatasets.Count, objDatasets))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object dataset
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> GetObjectDatasets(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var objDataset = await _objectService.GetObjectDatasetAsync(id);
            return objDataset != null
                ? Ok(SingleSuccessResponse(new List<ObjectDataset>() { objDataset }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new dataset for a specified object
    ****************************************************************/

    [HttpPost("data-objects/{sd_oid}/datasets")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> CreateObjectDataset(string sd_oid,
                 [FromBody] ObjectDataset objectDatasetContent)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            objectDatasetContent.SdOid = sd_oid; 
            var objDataset = await _objectService.CreateObjectDatasetAsync(objectDatasetContent);
            return objDataset != null
                ? Ok(SingleSuccessResponse(new List<ObjectDataset>() { objDataset }))
                : Ok(ErrorResponse("c", _attType, _parType, sd_oid, sd_oid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
       
    /****************************************************************
    * UPDATE a single specified object dataset
    ****************************************************************/

    [HttpPut("data-objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> UpdateObjectDataset(string sd_oid, int id, 
                 [FromBody] ObjectDataset objectDatasetContent)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var updatedObjDataset = await _objectService.UpdateObjectDatasetAsync(id, objectDatasetContent);
            return updatedObjDataset != null
                ? Ok(SingleSuccessResponse(new List<ObjectDataset>() { updatedObjDataset }))
                : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }

    /****************************************************************
    * DELETE a single specified object dataset
    ****************************************************************/
    
    [HttpDelete("data-objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> DeleteObjectDataset(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var count = await _objectService.DeleteObjectDatasetAsync(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}