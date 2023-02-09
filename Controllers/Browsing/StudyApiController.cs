using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class StudyApiController : BaseBrowsingApiController
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
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
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
    
    [HttpGet("studies/list")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
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
    
    [HttpGet("studies/data/title-contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
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
    
    [HttpGet("studies/list/title-contains/{titleFilter}")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]  
    
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

    [HttpGet("studies/data/by-org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
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
    
    [HttpGet("studies/list/by-org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
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
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
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
    
    [HttpGet("studies/list/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
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
    
    [HttpGet("studies/full/{sdSid}")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
    public async Task<IActionResult> GetFullStudy(string sdSid)
    {
        var fullStudy = await _studyService.GetFullStudyById(sdSid);
        return fullStudy != null
            ? Ok(SingleSuccessResponse(new List<FullStudy>() { fullStudy }))
            : Ok(NoEntityResponse(_fattType, sdSid));
    }
    
    
    /****************************************************************
    * DELETE an entire study record (with attributes)
    ****************************************************************/

    [HttpDelete("studies/full/{sdSid}")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
    public async Task<IActionResult> DeleteFullStudy(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var count = await _studyService.DeleteFullStudy(sdSid);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _fattType, "", sdSid))
                : Ok(ErrorResponse("d", _fattType, "", "", sdSid));
        } 
        return Ok(NoEntityResponse(_fattType, sdSid));
    }
    
    
    /****************************************************************
    * FETCH object list for a single study 
    ****************************************************************/
    
    [HttpGet("studies/{sdSid}/objects")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
    public async Task<IActionResult> GetStudyObjectList(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var objectsList = await _studyService.GetStudyObjectList(sdSid);
            return objectsList != null
                ? Ok(ListSuccessResponse(objectsList.Count, objectsList))
                : Ok(NoAttributesResponse("data objects"));
        } 
        return Ok(NoEntityResponse(_fattType, sdSid));
    }
    
    /****************************************************************
    * FETCH object list for a group of studies, sdSids as an array
    ****************************************************************/
    
    [HttpGet("multi-studies/objects")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
    public async Task<IActionResult> GetMultipleStudyObjectList(
        [FromQuery] string sdSids)
    {
        string[] sdSidArray = sdSids.Split(",");
        if (sdSidArray.Length > 0) {
            var objectsList = await _studyService.GetMultiStudyObjectList(sdSidArray);
            return objectsList != null
                ? Ok(ListSuccessResponse(objectsList.Count, objectsList))
                : Ok(NoAttributesResponse("data objects"));
        }
        return Ok(NoEntityResponse("Study Ids", ""));
    }

    /****************************************************************
    * FETCH study statistics - total number of studies
    ****************************************************************/

    [HttpGet("studies/total")]
    [SwaggerOperation(Tags = new[] { "Studies endpoint" })]

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
    
    [HttpGet("studies/by-type")]
    [SwaggerOperation(Tags = new[] { "Studies endpoint" })]

    public async Task<IActionResult> GetStudiesByType()
    {
        var stats = await _studyService.GetStudiesByType();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by type"));
    }
    
    /****************************************************************
    * FETCH involvement statistics - number of DTPs, DUPs
    * a study is / has been included within
    ****************************************************************/

    [HttpGet("studies/{sdSid}/involvement")]
    [SwaggerOperation(Tags = new[] { "Studies endpoint" })]
    
    public async Task<IActionResult> GetStudyInvolvement(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var stats = await _studyService.GetStudyInvolvement(sdSid);
            return Ok(ListSuccessResponse(stats.Count, stats));
        }
        return Ok(NoEntityResponse(_attType, sdSid));
    }
    
    /****************************************************************
    * FETCH the numbers of DTP or DUP objects where object is linked to study
    ****************************************************************/

    [HttpGet("studies/{sdSid}/object-involvement")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
    public async Task<IActionResult> GetStudyObjectInvolvement(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var stats = await _studyService.GetStudyObjectInvolvement(sdSid);
            return Ok(ListSuccessResponse(stats.Count, stats));
        }
        return Ok(NoEntityResponse(_attType, sdSid));
    }
    
    /****************************************************************
    * FETCH single study record (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/{sdSid}")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
    public async Task<IActionResult> GetStudyData(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var study = await _studyService.GetStudyRecordData(sdSid);
            return study != null
                ? Ok(SingleSuccessResponse(new List<StudyData>() { study }))
                : Ok(ErrorResponse("r", _attType, "", sdSid, sdSid));
        }
        return Ok(NoEntityResponse(_attType, sdSid));
    }
    
    /****************************************************************
    * CREATE a new study record (in studies table only)
    ****************************************************************/
    
    [HttpPost("studies/{sdSid}")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
    public async Task<IActionResult> CreateStudyData(string sdSid, 
                 [FromBody] StudyData studyDataContent)
    {
        // Check first that this SdSid has not already been used.
        
        if (!await _studyService.StudyExists(sdSid))
        {
            studyDataContent.SdSid = sdSid;
            var newStudyData = await _studyService.CreateStudyRecordData(studyDataContent);
            
            // Also add a new study title record (public title).
            
            bool titleAdditionOk = false;
            if (newStudyData != null)
            {
                var studyTitle = new StudyTitle()
                {
                    SdSid = sdSid, TitleTypeId = 15,
                    TitleText = newStudyData.DisplayTitle,
                    LangCode = "en", IsDefault = true

                };
                StudyTitle? newStudyTitle = await _studyService.CreateStudyTitle(studyTitle);
                titleAdditionOk = (newStudyTitle != null);
            }
            
            return (newStudyData != null && titleAdditionOk)
                ? Ok(SingleSuccessResponse(new List<StudyData>() { newStudyData }))
                : Ok(ErrorResponse("c", _attType, "", sdSid, sdSid));
        }
        return Ok(ExistingEntityResponse(_attType, sdSid));
    }
    
    /****************************************************************
    * UPDATE a specified study record (in studies table only)
    ****************************************************************/

    [HttpPut("studies/{sdSid}")]
    [SwaggerOperation(Tags = new []{"Studies endpoint"})]
    
    public async Task<IActionResult> UpdateStudyData(string sdSid, 
                 [FromBody] StudyData studyDataContent)
    {
        if (await _studyService.StudyExists(sdSid)) {
            studyDataContent.SdSid = sdSid;            // ensure this is the case
            var updatedStudyData = await _studyService.UpdateStudyRecordData(studyDataContent);
            return (updatedStudyData != null)
                ? Ok(SingleSuccessResponse(new List<StudyData>() { updatedStudyData }))
                : Ok(ErrorResponse("u", _attType, "", sdSid, sdSid));
        } 
        return Ok(NoEntityResponse(_attType, sdSid));
    }
    
    /****************************************************************
    * DELETE a specified study record (from studies table only) 
    ****************************************************************/

    [HttpDelete("studies/{sdSid}")]
    [SwaggerOperation(Tags = new[] { "Studies endpoint" })]

    public async Task<IActionResult> DeleteStudyData(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var count = await _studyService.DeleteStudyRecordData(sdSid);
            return (count > 0)
                ? Ok(DeletionSuccessResponse(count, _attType, "", sdSid))
                : Ok(ErrorResponse("d", _attType, "", sdSid, sdSid));
        } 
        return Ok(NoEntityResponse(_attType, sdSid));
    }
    
}