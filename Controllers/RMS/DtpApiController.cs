using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly IUriService _uriService;
    private readonly string _attType, _attTypes;
    
    public DtpApiController(IDtpService rmsService, IUriService uriService)
    {
        _dtpService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
        _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        _attType = "DTP"; _attTypes = "DTPs";
    }
    
    /****************************************************************
    * FETCH DTP records
    ****************************************************************/
    
    [HttpGet("data-transfers/processes")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpData( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedDtpData = await _dtpService.GetPaginatedDtpData(validFilter);
            if (pagedDtpData != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _dtpService.GetTotalDtps()).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedDtpData,
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
            var allDtpData = await _dtpService.GetAllDtps();
            return allDtpData != null
                ? Ok(ListSuccessResponse(allDtpData.Count, allDtpData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH DTP entries (id, org_id, display_name)
    ****************************************************************/
    
    [HttpGet("data-transfers/entries")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpEntries( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedDtpEntries = await _dtpService.GetPaginatedDtpEntries(validFilter);
            if (pagedDtpEntries != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _dtpService.GetTotalDtps()).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedDtpEntries,
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
            var allDtpEntries = await _dtpService.GetAllDtpEntries();
            return allDtpEntries != null
                ? Ok(ListSuccessResponse(allDtpEntries.Count, allDtpEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered DTP set
    ****************************************************************/
    
    [HttpGet("data-transfers/processes/title-contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpDataFiltered ( string titleFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredData = await _dtpService.GetPaginatedFilteredDtpRecords(titleFilter, validFilter);
            if (pagedFilteredData != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _dtpService.GetTotalFilteredDtps(titleFilter)).StatValue ?? 0;
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
            var filteredData = await _dtpService.GetFilteredDtpRecords(titleFilter);
            return filteredData != null
                ? Ok(ListSuccessResponse(filteredData.Count, filteredData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered DTP entries (id, org_id, display_name)
    ****************************************************************/
    
    [HttpGet("data-transfers/entries/title-contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]  
    
    public async Task<IActionResult> GetDtpEntriesFiltered ( string titleFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredEntries = await _dtpService.GetPaginatedFilteredDtpEntries(titleFilter, validFilter);
            if (pagedFilteredEntries != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _dtpService.GetTotalFilteredDtps(titleFilter)).StatValue ?? 0;
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
            var filteredEntries = await _dtpService.GetFilteredDtpEntries(titleFilter);
            return filteredEntries != null
                ? Ok(ListSuccessResponse(filteredEntries.Count, filteredEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH DTP records linked to an organisation
    ****************************************************************/ 

    [HttpGet("data-transfers/processes/by-org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpsByOrg(int orgId)
    {
        var dtpsByOrg = await _dtpService.GetDtpsByOrg(orgId);
        return dtpsByOrg != null
            ? Ok(ListSuccessResponse(dtpsByOrg.Count, dtpsByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH DTP entries (id, org_id, display_name) linked to an organisation
    ****************************************************************/
    
    [HttpGet("data-transfers/entries/by-org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpEntriesByOrg(int orgId)
    {
        var dtpEntriesByOrg = await _dtpService.GetDtpEntriesByOrg(orgId);
        return dtpEntriesByOrg != null
            ? Ok(ListSuccessResponse(dtpEntriesByOrg.Count, dtpEntriesByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH most recent DTP records
    ****************************************************************/ 

    [HttpGet("data-transfers/processes/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetRecentDtp(int n)
    {
        var recentDtps = await _dtpService.GetRecentDtps(n);
        return recentDtps != null
            ? Ok(ListSuccessResponse(recentDtps.Count, recentDtps))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH n MOST RECENT DTP entries (id, org_id, display_name)
    ****************************************************************/
    
    [HttpGet("data-transfers/entries/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetRecentDtpEntries(int n)
    {
        var recentDtpEntries = await _dtpService.GetRecentDtpEntries(n);
        return recentDtpEntries != null
            ? Ok(ListSuccessResponse(recentDtpEntries.Count, recentDtpEntries))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH data for a single study (including attribute data)
    ****************************************************************/
    
    [HttpGet("data-transfers/full/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> GetFullDtp(int id)
    {
        var fullDtp = await _dtpService.GetFullDtpById(id);
        return fullDtp != null
            ? Ok(SingleSuccessResponse(new List<FullDtp>() { fullDtp }))
            : Ok(NoEntityResponse(_attType, id.ToString()));
    }
    
    /****************************************************************
    * DELETE an entire study record (with attributes)
    ****************************************************************/

    [HttpDelete("data-transfers/full/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> DeleteFullDtp(int id)
    {
        if (await _dtpService.DtpExists(id)) {
            var count = await _dtpService.DeleteFullDtp(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, "", id.ToString()))
                : Ok(ErrorResponse("d", _attType, "", "", id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, id.ToString()));
    }

    
    /****************************************************************
    * Get DTP Total number
    ****************************************************************/

    [HttpGet("data-transfers/total")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]

    public async Task<IActionResult> GetDtpTotalNumber()
    {
        var stats = await _dtpService.GetTotalDtps();
        return stats.StatValue > 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", _attType, "", "", "total numbers"));
    }
    
    /****************************************************************
    * Get DTP Completed number
    ****************************************************************/
    
    [HttpGet("data-transfers/incomplete")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpCompletionNumbers()
    {
        var stats = await _dtpService.GetDtpsByCompletion();
        return stats.Count == 2
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "completion numbers"));
    }
    
    /****************************************************************
    * Get DTP numbers by status
    ****************************************************************/
    
    [HttpGet("data-transfers/by-status")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]

    public async Task<IActionResult> GetDtpsByStatus()
    {
        var stats = await _dtpService.GetDtpsByStatus();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by status"));
    }

    /****************************************************************
    * FETCH specified DTP
    ****************************************************************/ 
    
    [HttpGet("data-transfers/{dtpId:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtp(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            var dtp = await _dtpService.GetDtp(dtpId);
            return dtp != null
                ? Ok(SingleSuccessResponse(new List<Dtp>() { dtp }))
                : Ok(ErrorResponse("r", _attType, "", dtpId.ToString(), dtpId.ToString()));
        }
        return Ok(NoEntityResponse(_attType, dtpId.ToString()));
    }
    
    /****************************************************************
    * CREATE new DTP
    ****************************************************************/ 
    
    [HttpPost("data-transfers")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> CreateDtp([FromBody] Dtp dtpContent)
    {
        var newDtp = await _dtpService.CreateDtp(dtpContent);
        return newDtp != null
            ? Ok(SingleSuccessResponse(new List<Dtp>() { newDtp }))
            : Ok(ErrorResponse("c", _attType, "", "(not created)", "(not created)"));
    }
    
    /****************************************************************
    * UPDATE specified DTP
    ****************************************************************/ 

    [HttpPut("data-transfers/{dtpId:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> UpdateDtp(int dtpId, [FromBody] Dtp dtpContent)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            var updatedDtp = await _dtpService.UpdateDtp(dtpId, dtpContent);
            return (updatedDtp != null)
                ? Ok(SingleSuccessResponse(new List<Dtp>() { updatedDtp }))
                : Ok(ErrorResponse("u", _attType, "", dtpId.ToString(), dtpId.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, dtpId.ToString()));
    }
    
    /****************************************************************
    * DELETE specified DTP
    ****************************************************************/ 

    [HttpDelete("data-transfers/{dtpId:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> DeleteDtp(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            var count = await _dtpService.DeleteDtp(dtpId);
            return (count > 0)
                ? Ok(DeletionSuccessResponse(count, _attType, "", dtpId.ToString()))
                : Ok(ErrorResponse("d", _attType, "", dtpId.ToString(), dtpId.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, dtpId.ToString()));
    }
    
}