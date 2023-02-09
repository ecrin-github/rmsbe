using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectDatasetsApiController : BaseBrowsingApiController
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
    
    [HttpGet("data-objects/{sdOid}/datasets")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> GetObjectDatasets(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objDatasets = await _objectService.GetObjectDatasets(sdOid);
            return objDatasets != null
                ? Ok(ListSuccessResponse(objDatasets.Count, objDatasets))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }
    
    /****************************************************************
    * FETCH A SINGLE object dataset
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> GetObjectDatasets(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objDataset = await _objectService.GetObjectDataset(id);
            return objDataset != null
                ? Ok(SingleSuccessResponse(new List<ObjectDataset>() { objDataset }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new dataset for a specified object
    ****************************************************************/

    [HttpPost("data-objects/{sdOid}/datasets")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> CreateObjectDataset(string sdOid,
                 [FromBody] ObjectDataset objectDatasetContent)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            objectDatasetContent.SdOid = sdOid;    // ensure this is the case
            var objDataset = await _objectService.CreateObjectDataset(objectDatasetContent);
            return objDataset != null
                ? Ok(SingleSuccessResponse(new List<ObjectDataset>() { objDataset }))
                : Ok(ErrorResponse("c", _attType, _parType, sdOid, sdOid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
       
    /****************************************************************
    * UPDATE a single specified object dataset
    ****************************************************************/

    [HttpPut("data-objects/{sdOid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> UpdateObjectDataset(string sdOid, int id, 
                 [FromBody] ObjectDataset objectDatasetContent)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            objectDatasetContent.SdOid = sdOid;  // ensure this is the case
            objectDatasetContent.Id = id;
            var updatedObjDataset = await _objectService.UpdateObjectDataset(objectDatasetContent);
            return updatedObjDataset != null
                ? Ok(SingleSuccessResponse(new List<ObjectDataset>() { updatedObjDataset }))
                : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }

    /****************************************************************
    * DELETE a single specified object dataset
    ****************************************************************/
    
    [HttpDelete("data-objects/{sdOid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
    
    public async Task<IActionResult> DeleteObjectDataset(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var count = await _objectService.DeleteObjectDataset(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}