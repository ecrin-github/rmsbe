using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectApiController : BaseApiController
{
    private readonly IObjectService _objectService;
    private readonly IUriService _uriService;
    private readonly string _attType, _fattType, _attTypes;

    public ObjectApiController(IObjectService objectService, IUriService uriService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        _attType = "data object"; _fattType = "full data object"; _attTypes = "objects";
    }
    
    /****************************************************************
    * FETCH study records (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("data-objects/data")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetObjectData( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedObjectData = await _objectService.GetPaginatedObjectData(validFilter);
            if (pagedObjectData != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _objectService.GetTotalObjects()).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedObjectData,
                    validFilter, _uriService, totalRecords, route);
                return Ok(pagedResponse);
            }
            else
            {
                return Ok(NoAttributesResponse(_attTypes));
            }
        }
        else
        {
            var allObjectData = await _objectService.GetAllObjectsData();
            return allObjectData != null
                ? Ok(ListSuccessResponse(allObjectData.Count, allObjectData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH object entries (id, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("data-objects/entries")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetObjectEntries( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedObjectEntries = await _objectService.GetPaginatedObjectEntries(validFilter);
            if (pagedObjectEntries != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _objectService.GetTotalObjects()).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedObjectEntries,
                    validFilter, _uriService, totalRecords, route);
                return Ok(pagedResponse);
            }
            else
            {
                return Ok(NoAttributesResponse(_attTypes));
            }
        }
        else
        {
            var allObjectEntries = await _objectService.GetAllObjectEntries();
            return allObjectEntries != null
                ? Ok(ListSuccessResponse(allObjectEntries.Count, allObjectEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered object set
    ****************************************************************/
    
    [HttpGet("data-objects/data/title_contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetObjectDataFiltered ( string titleFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredData = await _objectService.GetPaginatedFilteredObjectRecords(titleFilter, validFilter);
            if (pagedFilteredData != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _objectService.GetTotalFilteredObjects(titleFilter)).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedFilteredData,
                    validFilter, _uriService, totalRecords, route);
                return Ok(pagedResponse);
            }
            else
            {
                return Ok(NoAttributesResponse(_attTypes));
            }
        }
        else
        {
            var filteredData = await _objectService.GetFilteredObjectRecords(titleFilter);
            return filteredData != null
                ? Ok(ListSuccessResponse(filteredData.Count, filteredData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered object entries (id, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("data-objects/entries/title_contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]  
    
    public async Task<IActionResult> GetObjectEntriesFiltered ( string titleFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredEntries = await _objectService.GetPaginatedFilteredObjectEntries(titleFilter, validFilter);
            if (pagedFilteredEntries != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _objectService.GetTotalFilteredObjects(titleFilter)).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedFilteredEntries,
                    validFilter, _uriService, totalRecords, route);
                return Ok(pagedResponse);
            }
            else
            {
                return Ok(NoAttributesResponse(_attTypes));
            }
        }
        else
        {
            var filteredEntries = await _objectService.GetFilteredObjectEntries(titleFilter);
            return filteredEntries != null
                ? Ok(ListSuccessResponse(filteredEntries.Count, filteredEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    
    /****************************************************************
    * FETCH ALL data objects (without attributes)
    ****************************************************************/
    
    [HttpGet("data-objects/data")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetObjectData()
    {
        var allObjectData = await _objectService.GetAllObjectsData();
        return allObjectData != null
            ? Ok(ListSuccessResponse(allObjectData.Count, allObjectData))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH Study records linked to an organisation
    ****************************************************************/ 

    [HttpGet("data-objects/data/by_org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetDtpsByOrg(int orgId)
    {
        var objectsByOrg = await _objectService.GetObjectsByOrg(orgId);
        return objectsByOrg != null
            ? Ok(ListSuccessResponse(objectsByOrg.Count, objectsByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH Study entries (id, sd_sid, name) linked to an organisation
    ****************************************************************/
    
    [HttpGet("data-objects/entries/by_org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetDtpEntriesByOrg(int orgId)
    {
        var objectEntriesByOrg = await _objectService.GetObjectEntriesByOrg(orgId);
        return objectEntriesByOrg != null
            ? Ok(ListSuccessResponse(objectEntriesByOrg.Count, objectEntriesByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH n RECENT data objects (without attributes)
    ****************************************************************/
    
    [HttpGet("data-objects/data/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetRecentObjectData(int n)
    {
        var recentObjectData = await _objectService.GetRecentObjectsData(n);
        return recentObjectData != null
            ? Ok(ListSuccessResponse(recentObjectData.Count, recentObjectData))
            : Ok(NoAttributesResponse(_attTypes));
    }
 
    
    /****************************************************************
    * FETCH n MOST RECENT object entries (id, sd_oid, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("data-objects/entries/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetRecentObjectEntries(int n)
    {
        var recentObjectEntries = await _objectService.GetRecentObjectEntries(n);
        return recentObjectEntries != null
            ? Ok(ListSuccessResponse(recentObjectEntries.Count, recentObjectEntries))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH a single specified data object (without attributes)
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/data")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetObjectData(string sd_oid)
    {
        if (await _objectService.ObjectExists(sd_oid)) {
            var dataObject = await _objectService.GetObjectData(sd_oid);
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
        var newDataObj = await _objectService.CreateDataObjectData(dataObjectContent);
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
        if (await _objectService.ObjectExists(sd_oid)) {
            var updatedDataObject = await _objectService.UpdateDataObjectData(dataObjectContent);
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
        if (await _objectService.ObjectExists(sd_oid)) {
             var count = await _objectService.DeleteDataObject(sd_oid);
             return (count > 0)
                 ? Ok(DeletionSuccessResponse(count, _attType, "", sd_oid))
                 : Ok(ErrorResponse("d", _attType, "", sd_oid, sd_oid));
        } 
        return Ok(NoEntityResponse(_attType, sd_oid));
    }
    
    
    /****************************************************************
    * FETCH a specific data object (including attribute data)
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
    
    public async Task<IActionResult> GetObjectById(string sd_oid)
    {
        var fullDataObject = await _objectService.GetFullObjectById(sd_oid);
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
        if (await _objectService.ObjectExists(sd_oid)) {
            var count = await _objectService.DeleteFullObject(sd_oid);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _fattType, "", sd_oid))
                : Ok(ErrorResponse("d", _fattType, "", "", sd_oid));
        } 
        return Ok(NoEntityResponse(_fattType, sd_oid));
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

    public async Task<IActionResult> GetObjectsByType()
    {
        var stats = await _objectService.GetObjectsByType();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by type"));
    }


}