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
    
    [HttpGet("data-transfers/data")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpData( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedDtpData = await _dtpService.GetPaginatedDtpDataAsync(validFilter);
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
            var allDtpData = await _dtpService.GetAllDtpsAsync();
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
            var pagedDtpEntries = await _dtpService.GetPaginatedDtpEntriesAsync(validFilter);
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
            var allDtpEntries = await _dtpService.GetDtpEntriesAsync();
            return allDtpEntries != null
                ? Ok(ListSuccessResponse(allDtpEntries.Count, allDtpEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered DTP set
    ****************************************************************/
    
    [HttpGet("data-transfers/data/title_contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpDataFiltered ( string titleFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredData = await _dtpService.GetPaginatedFilteredDtpRecordsAsync(titleFilter, validFilter);
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
            var filteredData = await _dtpService.GetFilteredDtpRecordsAsync(titleFilter);
            return filteredData != null
                ? Ok(ListSuccessResponse(filteredData.Count, filteredData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered DTP entries (id, org_id, display_name)
    ****************************************************************/
    
    [HttpGet("data-transfers/entries/title_contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]  
    
    public async Task<IActionResult> GetDtpEntriesFiltered ( string titleFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredEntries = await _dtpService.GetPaginatedFilteredDtpEntriesAsync(titleFilter, validFilter);
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
            var filteredEntries = await _dtpService.GetFilteredDtpEntriesAsync(titleFilter);
            return filteredEntries != null
                ? Ok(ListSuccessResponse(filteredEntries.Count, filteredEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH most recent DTP records
    ****************************************************************/ 

    [HttpGet("data-transfers/processes/recent/{number:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetRecentDtp(int n)
    {
        var recentDtps = await _dtpService.GetRecentDtpsAsync(n);
        return recentDtps != null
            ? Ok(ListSuccessResponse(recentDtps.Count, recentDtps))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH n MOST RECENT DTP entries (id, org_id, display_name)
    ****************************************************************/
    
    [HttpGet("data-transfers/entries/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"SData transfer process endpoint"})]
    
    public async Task<IActionResult> GetRecentDTPEntries(int n)
    {
        var recentDtpEntries = await _dtpService.GetRecentDtpEntriesAsync(n);
        return recentDtpEntries != null
            ? Ok(ListSuccessResponse(recentDtpEntries.Count, recentDtpEntries))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH specified DTP
    ****************************************************************/ 
    
    [HttpGet("data-transfers/processes/{dtp_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtp(int dtp_id)
    {
        if (await _dtpService.DtpExistsAsync(dtp_id)) {
            var dtp = await _dtpService.GetDtpAsync(dtp_id);
            return dtp != null
                ? Ok(SingleSuccessResponse(new List<Dtp>() { dtp }))
                : Ok(ErrorResponse("r", _attType, "", dtp_id.ToString(), dtp_id.ToString()));
        }
        return Ok(NoEntityResponse(_attType, dtp_id.ToString()));
    }
    
    /****************************************************************
    * CREATE new DTP
    ****************************************************************/ 
    
    [HttpPost("data-transfers/processes")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> CreateDtp([FromBody] Dtp dtpContent)
    {
        var newDtp = await _dtpService.CreateDtpAsync(dtpContent);
        return newDtp != null
            ? Ok(SingleSuccessResponse(new List<Dtp>() { newDtp }))
            : Ok(ErrorResponse("c", _attType, "", "(not created)", "(not created)"));
    }
    
    /****************************************************************
    * UPDATE specified DTP
    ****************************************************************/ 

    [HttpPut("data-transfers/processes/{dtp_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> UpdateDtp(int dtp_id, [FromBody] Dtp dtpContent)
    {
        if (await _dtpService.DtpExistsAsync(dtp_id)) {
            var updatedDtp = await _dtpService.UpdateDtpAsync(dtp_id, dtpContent);
            return (updatedDtp != null)
                ? Ok(SingleSuccessResponse(new List<Dtp>() { updatedDtp }))
                : Ok(ErrorResponse("u", _attType, "", dtp_id.ToString(), dtp_id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, dtp_id.ToString()));
    }
    
    /****************************************************************
    * DELETE specified DTP
    ****************************************************************/ 

    [HttpDelete("data-transfers/processes/{dtp_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> DeleteDtp(int dtp_id)
    {
        if (await _dtpService.DtpExistsAsync(dtp_id)) {
            var count = await _dtpService.DeleteDtpAsync(dtp_id);
            return (count > 0)
                ? Ok(DeletionSuccessResponse(count, _attType, "", dtp_id.ToString()))
                : Ok(ErrorResponse("d", _attType, "", dtp_id.ToString(), dtp_id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, dtp_id.ToString()));
    }
    
    
    /****************************************************************
    * Get DTP statistics 
    ****************************************************************/

    [HttpGet("data-transfers/processes/total")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]

    public async Task<IActionResult> GetDtpTotalNumber()
    {
        var stats = await _dtpService.GetTotalDtps();
        return stats.StatValue > 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", _attType, "", "", "total numbers"));
    }
    
    [HttpGet("data-transfers/processes/by_completion")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
    
    public async Task<IActionResult> GetDtpCompletionNumbers()
    {
        var stats = await _dtpService.GetDtpsByCompletion();
        return stats.Count == 2
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "completion numbers"));
    }
    
    
    [HttpGet("data-transfers/processes/by_status")]
    [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]

    public async Task<IActionResult> GetDtpsByStatus()
    {
        var stats = await _dtpService.GetDtpsByStatus();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by status"));
    }
    
}