using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly IUriService _uriService;
    private readonly string _attType, _attTypes;
    
    public DupApiController(IDupService dupService, IUriService uriService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        _attType = "DUP"; _attTypes = "DUPs";
    }
    
    /****************************************************************
    * FETCH DUP records
    ****************************************************************/
    
    [HttpGet("data-uses/data")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDupData( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedDupData = await _dupService.GetPaginatedDupData(validFilter);
            if (pagedDupData != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _dupService.GetTotalDups()).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedDupData,
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
            var allDupData = await _dupService.GetAllDups();
            return allDupData != null
                ? Ok(ListSuccessResponse(allDupData.Count, allDupData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH DUP entries (id, org_id, display_name)
    ****************************************************************/
    
    [HttpGet("data-uses/entries")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDupEntries( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedDupEntries = await _dupService.GetPaginatedDupEntries(validFilter);
            if (pagedDupEntries != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _dupService.GetTotalDups()).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedDupEntries,
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
            var allDupEntries = await _dupService.GetAllDupEntries();
            return allDupEntries != null
                ? Ok(ListSuccessResponse(allDupEntries.Count, allDupEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered DUP set
    ****************************************************************/
    
    [HttpGet("data-uses/data/title_contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDupDataFiltered ( string titleFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredData = await _dupService.GetPaginatedFilteredDupRecords(titleFilter, validFilter);
            if (pagedFilteredData != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _dupService.GetTotalFilteredDups(titleFilter)).StatValue ?? 0;
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
            var filteredData = await _dupService.GetFilteredDupRecords(titleFilter);
            return filteredData != null
                ? Ok(ListSuccessResponse(filteredData.Count, filteredData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered DUP entries (id, org_id, display_name)
    ****************************************************************/
    
    [HttpGet("data-uses/entries/title_contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]  
    
    public async Task<IActionResult> GetDupEntriesFiltered ( string titleFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredEntries = await _dupService.GetPaginatedFilteredDupEntries(titleFilter, validFilter);
            if (pagedFilteredEntries != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _dupService.GetTotalFilteredDups(titleFilter)).StatValue ?? 0;
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
            var filteredEntries = await _dupService.GetFilteredDupEntries(titleFilter);
            return filteredEntries != null
                ? Ok(ListSuccessResponse(filteredEntries.Count, filteredEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH DUP records linked to an organisation
    ****************************************************************/ 

    [HttpGet("data-uses/processes/by_org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDtpsByOrg(int orgId)
    {
        var dupsByOrg = await _dupService.GetDupsByOrg(orgId);
        return dupsByOrg != null
            ? Ok(ListSuccessResponse(dupsByOrg.Count, dupsByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH DUP entries (id, org_id, display_name) linked to an organisation
    ****************************************************************/
    
    [HttpGet("data-uses/entries/by_org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDtpEntriesByOrg(int orgId)
    {
        var dupEntriesByOrg = await _dupService.GetDupEntriesByOrg(orgId);
        return dupEntriesByOrg != null
            ? Ok(ListSuccessResponse(dupEntriesByOrg.Count, dupEntriesByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }

    /****************************************************************
    * FETCH most recent DUP records
    ****************************************************************/ 
    
    [HttpGet("data-uses/processes/recent/{number:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetRecentDup(int n)
    {
        var recentDups = await _dupService.GetRecentDups(n);
        return recentDups != null
            ? Ok(ListSuccessResponse(recentDups.Count, recentDups))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH n MOST RECENT DUP entries (id, org_id, display_name)
    ****************************************************************/
    
    [HttpGet("data-uses/entries/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetRecentDupPEntries(int n)
    {
        var recentDupEntries = await _dupService.GetRecentDupEntries(n);
        return recentDupEntries != null
            ? Ok(ListSuccessResponse(recentDupEntries.Count, recentDupEntries))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH data for a single study (including attribute data)
    ****************************************************************/
    
    [HttpGet("data-uses/full/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetFullDup(int id)
    {
        var fullDup = await _dupService.GetFullDupById(id);
        return fullDup != null
            ? Ok(SingleSuccessResponse(new List<FullDup>() { fullDup }))
            : Ok(NoEntityResponse(_attType, id.ToString()));
    }
    
    /****************************************************************
    * DELETE an entire study record (with attributes)
    ****************************************************************/

    [HttpDelete("data-uses/full/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> DeleteFullDup(int id)
    {
        if (await _dupService.DupExists(id)) {
            var count = await _dupService.DeleteFullDup(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, "", id.ToString()))
                : Ok(ErrorResponse("d", _attType, "", "", id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, id.ToString()));
    }

    /****************************************************************
    * Get DUP Total number
    ****************************************************************/

    [HttpGet("data-uses/processes/total")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]

    public async Task<IActionResult> GetDupTotalNumber()
    {
        var stats = await _dupService.GetTotalDups();
        return stats.StatValue > 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", _attType, "", "", "total numbers"));
    }
    
    /****************************************************************
    * Get DUP Completed number
    ****************************************************************/
    
    [HttpGet("data-uses/processes/by_completion")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDupCompletionNumbers()
    {
        var stats = await _dupService.GetDupsByCompletion();
        return stats.Count == 2
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "completion numbers"));
    }
    
    /****************************************************************
    * Get DUP numbers by status
    ****************************************************************/
    
    [HttpGet("data-uses/processes/by_status")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]

    public async Task<IActionResult> GetDupsByStatus()
    {
        var stats = await _dupService.GetDupsByStatus();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by status"));
    }
    
    /****************************************************************
    * FETCH specified DUP
    ****************************************************************/ 

    [HttpGet("data-uses/processes/{dup_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> GetDup(int dup_id)
    {
        if (await _dupService.DupExists(dup_id)) {
            var dup = await _dupService.GetDup(dup_id);
            return dup != null
                ? Ok(SingleSuccessResponse(new List<Dup>() { dup }))
                : Ok(ErrorResponse("r", _attType, "", dup_id.ToString(), dup_id.ToString()));
        }
        return Ok(NoEntityResponse(_attType, dup_id.ToString()));
    }
    
    /****************************************************************
    * CREATE new DUP
    ****************************************************************/ 
    
    [HttpPost("data-uses/processes")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> CreateDup([FromBody] Dup dupContent)
    {
        var newDup = await _dupService.CreateDup(dupContent);
        return newDup != null
            ? Ok(SingleSuccessResponse(new List<Dup>() { newDup }))
            : Ok(ErrorResponse("c", _attType, "", "(not created)", "(not created)"));
    }
    
    /****************************************************************
    * UPDATE specified DUP
    ****************************************************************/ 
    
    [HttpPut("data-uses/processes/{dup_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> UpdateDup(int dup_id, [FromBody] Dup dupContent)
    {
        if (await _dupService.DupExists(dup_id)) {
            var updatedDup = await _dupService.UpdateDup(dup_id, dupContent);
            return (updatedDup != null)
                ? Ok(SingleSuccessResponse(new List<Dup>() { updatedDup }))
                : Ok(ErrorResponse("u", _attType, "", dup_id.ToString(), dup_id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, dup_id.ToString()));
    }
   
    /****************************************************************
    * DELETE specified DUP
    ****************************************************************/ 
    
    [HttpDelete("data-uses/processes/{dup_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process endpoint"})]
    
    public async Task<IActionResult> DeleteDup(int dup_id)
    {
        if (await _dupService.DupExists(dup_id)) {
            var count = await _dupService.DeleteDup(dup_id);
            return (count > 0)
                ? Ok(DeletionSuccessResponse(count, _attType, "", dup_id.ToString()))
                : Ok(ErrorResponse("d", _attType, "", dup_id.ToString(), dup_id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, dup_id.ToString()));
    }
    
}