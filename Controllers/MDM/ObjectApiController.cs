using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectApiController : BaseApiController
{
    private readonly IObjectService _objectService;
    private readonly string _attType, _fattType, _attTypes;

    public ObjectApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _attType = "data object"; _fattType = "full data object"; _attTypes = "objects";
    }
    
    /****************************************************************
    * FETCH a specific data object (including attribute data)
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
    
    public async Task<IActionResult> GetObjectById(string sd_oid)
    {
        var fullDataObject = await _objectService.GetFullObjectByIdAsync(sd_oid);
        return fullDataObject != null
            ? Ok(SingleSuccessResponse(new List<FullDataObject>() { fullDataObject }))
            : Ok(NoEntityResponse(_fattType, sd_oid));
    }
    
    /****************************************************************
    * DELETE a specific data object (including all attribute data)
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
    
    public async Task<IActionResult> DeleteDataObject(string sd_oid)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            var count = await _objectService.DeleteFullObjectAsync(sd_oid);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _fattType, "", sd_oid))
                : Ok(ErrorResponse("d", _fattType, "", "", sd_oid));
        } 
        return Ok(NoEntityResponse(_fattType, sd_oid));
    }
    
    /****************************************************************
    * FETCH ALL data objects (without attributes)
    ****************************************************************/
    
    [HttpGet("data-objects/data")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetObjectData()
    {
        var allObjectData = await _objectService.GetAllObjectsDataAsync();
        return allObjectData != null
            ? Ok(ListSuccessResponse(allObjectData.Count, allObjectData))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH n RECENT data objects (without attributes)
    ****************************************************************/
    
    [HttpGet("data-objects/data/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetRecentObjectData(int n)
    {
        var recentObjectData = await _objectService.GetRecentObjectsDataAsync(n);
        return recentObjectData != null
            ? Ok(ListSuccessResponse(recentObjectData.Count, recentObjectData))
            : Ok(NoAttributesResponse(_attTypes));
    }

    /****************************************************************
    * FETCH a single specified data object (without attributes)
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/data")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetObjectData(string sd_oid)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            var dataObject = await _objectService.GetObjectDataAsync(sd_oid);
            return dataObject != null
                ? Ok(SingleSuccessResponse(new List<DataObjectData>() { dataObject }))
                : Ok(ErrorResponse("r", _attType, "", sd_oid, sd_oid));
        }
        return Ok(NoEntityResponse(_attType, sd_oid)); 
    }

    /****************************************************************
    * CREATE a single specified data object (without attributes)
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> CreateObjectData(string sd_sid, 
        [FromBody] DataObjectData dataObjectContent)
    {
        dataObjectContent.SdSid = sd_sid;
        var newDataObj = await _objectService.CreateDataObjectDataAsync(dataObjectContent);
        return newDataObj != null
            ? Ok(SingleSuccessResponse(new List<DataObjectData>() { newDataObj }))
            : Ok(ErrorResponse("c", _attType, "", sd_sid, sd_sid));
    }
    
    /****************************************************************
    * UPDATE a single specified data object (without attributes)
    ****************************************************************/

    [HttpPut("data-objects/{sd_oid}/data")]
    [SwaggerOperation(Tags = new[] { "Object data endpoint" })]
    
    public async Task<IActionResult> UpdateObjectData(string sd_oid, 
        [FromBody] DataObjectData dataObjectContent)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            var updatedDataObject = await _objectService.UpdateDataObjectDataAsync(dataObjectContent);
            return (updatedDataObject != null)
                ? Ok(SingleSuccessResponse(new List<DataObjectData>() { updatedDataObject }))
                : Ok(ErrorResponse("u", _attType, "", sd_oid, sd_oid));
        } 
        return Ok(NoEntityResponse(_attType, sd_oid));
    }
    
    /****************************************************************
    * DELETE a single specified data object (without attributes)
    ****************************************************************/
    
    [HttpDelete("data-objects/{sd_oid}/data")]
    [SwaggerOperation(Tags = new[] { "Object data endpoint" })]

    public async Task<IActionResult> DeleteStudyData(string sd_oid)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
             var count = await _objectService.DeleteDataObjectAsync(sd_oid);
             return (count > 0)
                 ? Ok(DeletionSuccessResponse(count, _attType, "", sd_oid))
                 : Ok(ErrorResponse("d", _attType, "", sd_oid, sd_oid));
        } 
        return Ok(NoEntityResponse(_attType, sd_oid));
    }
    
    /****************************************************************
    * Get object statistics 
    ****************************************************************/

    [HttpGet("data-objects/total")]
    [SwaggerOperation(Tags = new[] { "Object data endpoint" })]

    public async Task<IActionResult> GetObjectTotalNumber()
    {
        var stats = await _objectService.GetTotalObjects();
        return stats.StatValue > 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", _attType, "", "", "total numbers"));
    }
    
    
    [HttpGet("data-objects/by_type")]
    [SwaggerOperation(Tags = new[] { "Object data endpoint" })]

    public async Task<IActionResult> GetStudiesByType()
    {
        var stats = await _objectService.GetObjectsByType();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by type"));
    }


}