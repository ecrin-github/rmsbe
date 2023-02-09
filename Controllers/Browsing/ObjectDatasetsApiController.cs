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
    [SwaggerOperation(Tags = new []{"Browsing - Object datasets endpoint"})]
    
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
    [SwaggerOperation(Tags = new []{"Browsing - Object datasets endpoint"})]
    
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
}