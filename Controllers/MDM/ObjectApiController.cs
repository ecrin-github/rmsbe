using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectApiController : BaseApiController
{
    private readonly IObjectService _objectService;
    private readonly IStudyService _studyService;
    private readonly IUriService _uriService;
    private readonly string _attType, _fattType, _attTypes;

    public ObjectApiController(IObjectService objectService, IStudyService studyService, IUriService uriService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
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
            var pagedObjectData = await _objectService.GetPaginatedObjectRecords(validFilter);
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
            var allObjectData = await _objectService.GetAllObjectRecords();
            return allObjectData != null
                ? Ok(ListSuccessResponse(allObjectData.Count, allObjectData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH object entries (id, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("data-objects/list")]
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
    
    [HttpGet("data-objects/data/title-contains/{titleFilter}")]
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
    
    [HttpGet("data-objects/list/title-contains/{titleFilter}")]
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
    * FETCH Object records linked to an organisation
    ****************************************************************/ 

    [HttpGet("data-objects/data/by-org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetDtpsByOrg(int orgId)
    {
        var objectsByOrg = await _objectService.GetObjectRecordsByOrg(orgId);
        return objectsByOrg != null
            ? Ok(ListSuccessResponse(objectsByOrg.Count, objectsByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH Object entries (id, sd_sid, name) linked to an organisation
    ****************************************************************/
    
    [HttpGet("data-objects/list/by-org/{orgId:int}")]
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
        var recentObjectData = await _objectService.GetRecentObjectRecords(n);
        return recentObjectData != null
            ? Ok(ListSuccessResponse(recentObjectData.Count, recentObjectData))
            : Ok(NoAttributesResponse(_attTypes));
    }
 
    
    /****************************************************************
    * FETCH n MOST RECENT object entries (id, sd_oid, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("data-objects/list/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetRecentObjectEntries(int n)
    {
        var recentObjectEntries = await _objectService.GetRecentObjectEntries(n);
        return recentObjectEntries != null
            ? Ok(ListSuccessResponse(recentObjectEntries.Count, recentObjectEntries))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    
    /****************************************************************
    * FETCH a specific data object (including attribute data)
    ****************************************************************/
    
    [HttpGet("data-objects/full/{sdOid}")]
    [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
    
    public async Task<IActionResult> GetFullObjectById(string sdOid)
    {
        var fullDataObject = await _objectService.GetFullObjectById(sdOid);
        return fullDataObject != null
            ? Ok(SingleSuccessResponse(new List<FullDataObject>() { fullDataObject }))
            : Ok(NoEntityResponse(_fattType, sdOid));
    }
    
    /****************************************************************
    * DELETE a specific data object (including all attribute data)
    ****************************************************************/

    [HttpDelete("data-objects/full/{sdOid}")]
    [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
    
    public async Task<IActionResult> DeleteFullObject(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var count = await _objectService.DeleteFullObject(sdOid);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _fattType, "", sdOid))
                : Ok(ErrorResponse("d", _fattType, "", "", sdOid));
        } 
        return Ok(NoEntityResponse(_fattType, sdOid));
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
    
    
    [HttpGet("data-objects/by-type")]
    [SwaggerOperation(Tags = new[] { "Object data endpoint" })]

    public async Task<IActionResult> GetObjectsByType()
    {
        var stats = await _objectService.GetObjectsByType();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by type"));
    }

    /****************************************************************
    * FETCH involvement statistics - number of DTPs, DUPs
    * an object is / has been included within
    ****************************************************************/

    [HttpGet("data-objects/{sdOid}/involvement")]
    [SwaggerOperation(Tags = new[] { "Object data endpoint" })]
    
    public async Task<IActionResult> GetObjectInvolvement(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var stats = await _objectService.GetObjectInvolvement(sdOid);
            return Ok(ListSuccessResponse(stats.Count, stats));
        }
        return Ok(NoEntityResponse(_attType, sdOid));
    }
    
    /****************************************************************
    * FETCH a single specified data object (without attributes)
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetObjectData(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var dataObject = await _objectService.GetObjectData(sdOid);
            return dataObject != null
                ? Ok(SingleSuccessResponse(new List<DataObjectData>() { dataObject }))
                : Ok(ErrorResponse("r", _attType, "", sdOid, sdOid));
        }
        return Ok(NoEntityResponse(_attType, sdOid)); 
    }

    /****************************************************************
    * CREATE a single specified data object (without attributes)
    ****************************************************************/
    
    [HttpPost("data-objects/{sdSid}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> CreateObjectData(string sdSid, 
        [FromBody] DataObjectData dataObjectContent)
    {
        if (await _studyService.StudyExists(sdSid)) {
            dataObjectContent.SdSid = sdSid;
            var newDataObj = await _objectService.CreateDataObjectData(dataObjectContent);
            return newDataObj != null
                ? Ok(SingleSuccessResponse(new List<DataObjectData>() { newDataObj }))
                : Ok(ErrorResponse("c", _attType, "", sdSid, sdSid));
        } 
        return Ok(NoEntityResponse("parent study", sdSid));
    }
    
    /****************************************************************
    * UPDATE a single specified data object (without attributes)
    ****************************************************************/

    [HttpPut("data-objects/{sdOid}")]
    [SwaggerOperation(Tags = new[] { "Object data endpoint" })]
    
    public async Task<IActionResult> UpdateObjectData(string sdOid, 
        [FromBody] DataObjectData dataObjectContent)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            dataObjectContent.SdOid = sdOid;            // ensure this is the case
            var updatedDataObject = await _objectService.UpdateDataObjectData(dataObjectContent);
            return (updatedDataObject != null)
                ? Ok(SingleSuccessResponse(new List<DataObjectData>() { updatedDataObject }))
                : Ok(ErrorResponse("u", _attType, "", sdOid, sdOid));
        } 
        return Ok(NoEntityResponse(_attType, sdOid));
    }
    
    /****************************************************************
    * DELETE a single specified data object (without attributes)
    ****************************************************************/
    
    [HttpDelete("data-objects/{sdOid}")]
    [SwaggerOperation(Tags = new[] { "Object data endpoint" })]

    public async Task<IActionResult> DeleteStudyData(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
             var count = await _objectService.DeleteDataObject(sdOid);
             return (count > 0)
                 ? Ok(DeletionSuccessResponse(count, _attType, "", sdOid))
                 : Ok(ErrorResponse("d", _attType, "", sdOid, sdOid));
        } 
        return Ok(NoEntityResponse(_attType, sdOid));
    }

}