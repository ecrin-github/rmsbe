using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private readonly IUriService _uriService;
    private readonly string _attType, _fattType, _attTypes;

    public StudyApiController(IStudyService studyService, IUriService uriService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        _attType = "study"; _fattType = "full study"; _attTypes = "studies";
    }
      
    /****************************************************************
    * FETCH study records (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyData( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedStudyData = await _studyService.GetPaginatedStudyRecords(validFilter);
            if (pagedStudyData != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _studyService.GetTotalStudies()).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedStudyData,
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
            var allStudyData = await _studyService.GetAllStudyRecords();
            return allStudyData != null
                ? Ok(ListSuccessResponse(allStudyData.Count, allStudyData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH study entries (id, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("studies/entries")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyEntries( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedStudyEntries = await _studyService.GetPaginatedStudyEntries(validFilter);
            if (pagedStudyEntries != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _studyService.GetTotalStudies()).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedStudyEntries,
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
            var allStudyEntries = await _studyService.GetAllStudyEntries();
            return allStudyEntries != null
                ? Ok(ListSuccessResponse(allStudyEntries.Count, allStudyEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered study set
    ****************************************************************/
    
    [HttpGet("studies/data/title_contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyDataFiltered ( string titleFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredData = await _studyService.GetPaginatedFilteredStudyRecords(titleFilter, validFilter);
            if (pagedFilteredData != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _studyService.GetTotalFilteredStudies(titleFilter)).StatValue ?? 0;
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
            var filteredData = await _studyService.GetFilteredStudyRecords(titleFilter);
            return filteredData != null
                ? Ok(ListSuccessResponse(filteredData.Count, filteredData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered study entries (id, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("studies/entries/title_contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]  
    
    public async Task<IActionResult> GetStudyEntriesFiltered ( string titleFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredEntries = await _studyService.GetPaginatedFilteredStudyEntries(titleFilter, validFilter);
            if (pagedFilteredEntries != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _studyService.GetTotalFilteredStudies(titleFilter)).StatValue ?? 0;
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
            var filteredEntries = await _studyService.GetFilteredStudyEntries(titleFilter);
            return filteredEntries != null
                ? Ok(ListSuccessResponse(filteredEntries.Count, filteredEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH Study records linked to an organisation
    ****************************************************************/ 

    [HttpGet("studies/data/by_org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetDtpsByOrg(int orgId)
    {
        var studiesByOrg = await _studyService.GetStudyRecordsByOrg(orgId);
        return studiesByOrg != null
            ? Ok(ListSuccessResponse(studiesByOrg.Count, studiesByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH Study entries (id, sd_sid, name) linked to an organisation
    ****************************************************************/
    
    [HttpGet("studies/entries/by_org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetDtpEntriesByOrg(int orgId)
    {
        var studyEntriesByOrg = await _studyService.GetStudyEntriesByOrg(orgId);
        return studyEntriesByOrg != null
            ? Ok(ListSuccessResponse(studyEntriesByOrg.Count, studyEntriesByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }

    
    /****************************************************************
    * FETCH n MOST RECENT study data (without attributes)
    ****************************************************************/
    
    [HttpGet("studies/data/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetRecentStudyData(int n)
    {
        var recentStudyData = await _studyService.GetRecentStudyRecords(n);
        return recentStudyData != null
            ? Ok(ListSuccessResponse(recentStudyData.Count, recentStudyData))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH n MOST RECENT study entries (id, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("studies/entries/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetRecentStudyEntries(int n)
    {
        var recentStudyEntries = await _studyService.GetRecentStudyEntries(n);
        return recentStudyEntries != null
            ? Ok(ListSuccessResponse(recentStudyEntries.Count, recentStudyEntries))
            : Ok(NoAttributesResponse(_attTypes));
    }
      
    /****************************************************************
    * FETCH data for a single study (including attribute data)
    ****************************************************************/
    
    [HttpGet("studies/full/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> GetFullStudy(string sd_sid)
    {
        var fullStudy = await _studyService.GetFullStudyById(sd_sid);
        return fullStudy != null
            ? Ok(SingleSuccessResponse(new List<FullStudy>() { fullStudy }))
            : Ok(NoEntityResponse(_fattType, sd_sid));
    }
    
    /****************************************************************
    * DELETE an entire study record (with attributes)
    ****************************************************************/

    [HttpDelete("studies/full/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> DeleteFullStudy(string sd_sid)
    {
        if (await _studyService.StudyExists(sd_sid)) {
            var count = await _studyService.DeleteFullStudy(sd_sid);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _fattType, "", sd_sid))
                : Ok(ErrorResponse("d", _fattType, "", "", sd_sid));
        } 
        return Ok(NoEntityResponse(_fattType, sd_sid));
    }
    
    /****************************************************************
    * FETCH study statistics - total number of studies
    ****************************************************************/

    [HttpGet("studies/total")]
    [SwaggerOperation(Tags = new[] { "Study data endpoint" })]

    public async Task<IActionResult> GetStudyTotalNumber()
    {
        var stats = await _studyService.GetTotalStudies();
        return stats.StatValue > 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", _attType, "", "", "total numbers"));
    }
    
    /****************************************************************
    * FETCH study statistics - number of studies by study type
    ****************************************************************/
    
    [HttpGet("studies/by_type")]
    [SwaggerOperation(Tags = new[] { "Study data endpoint" })]

    public async Task<IActionResult> GetStudiesByType()
    {
        var stats = await _studyService.GetStudiesByType();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by type"));
    }
    
    /****************************************************************
    * FETCH single study record (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyData(string sd_sid)
    {
        if (await _studyService.StudyExists(sd_sid)) {
            var study = await _studyService.GetStudyRecordData(sd_sid);
            return study != null
                ? Ok(SingleSuccessResponse(new List<StudyData>() { study }))
                : Ok(ErrorResponse("r", _attType, "", sd_sid, sd_sid));
        }
        return Ok(NoEntityResponse(_attType, sd_sid));
    }
    
    /****************************************************************
    * CREATE a new study record (in studies table only)
    ****************************************************************/

    [HttpPost("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> CreateStudyData(string sd_sid, 
                 [FromBody] StudyData studyDataContent)
    {
        studyDataContent.SdSid = sd_sid;
        var newStudyData = await _studyService.CreateStudyRecordData(studyDataContent);
        return newStudyData != null
            ? Ok(SingleSuccessResponse(new List<StudyData>() { newStudyData }))
            : Ok(ErrorResponse("c", _attType, "", sd_sid, sd_sid));
    }
    
    /****************************************************************
    * UPDATE a specified study record (in studies table only)
    ****************************************************************/

    [HttpPut("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> UpdateStudyData(string sd_sid, 
                 [FromBody] StudyData studyDataContent)
    {
        if (await _studyService.StudyExists(sd_sid)) {
            var updatedStudyData = await _studyService.UpdateStudyRecordData(studyDataContent);
            return (updatedStudyData != null)
                ? Ok(SingleSuccessResponse(new List<StudyData>() { updatedStudyData }))
                : Ok(ErrorResponse("u", _attType, "", sd_sid, sd_sid));
        } 
        return Ok(NoEntityResponse(_attType, sd_sid));
    }
    
    /****************************************************************
    * DELETE a specified study record (from studies table only) 
    ****************************************************************/

    [HttpDelete("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new[] { "Study data endpoint" })]

    public async Task<IActionResult> DeleteStudyData(string sd_sid)
    {
        if (await _studyService.StudyExists(sd_sid)) {
            var count = await _studyService.DeleteStudyRecordData(sd_sid);
            return (count > 0)
                ? Ok(DeletionSuccessResponse(count, _attType, "", sd_sid))
                : Ok(ErrorResponse("d", _attType, "", sd_sid, sd_sid));
        } 
        return Ok(NoEntityResponse(_attType, sd_sid));
    }
    
}