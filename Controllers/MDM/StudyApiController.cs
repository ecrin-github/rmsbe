using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private readonly string _attType, _fattType, _attTypes;

    public StudyApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _attType = "study"; _fattType = "full study"; _attTypes = "studies";
    }
      
    /****************************************************************
    * FETCH data for a single study (including attribute data)
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> GetFullStudy(string sd_sid)
    {
        var fullStudy = await _studyService.GetFullStudyByIdAsync(sd_sid);
        return fullStudy != null
            ? Ok(SingleSuccessResponse(new List<FullStudy>() { fullStudy }))
            : Ok(NoEntityResponse(_fattType, sd_sid));
    }
    
    /****************************************************************
    * DELETE an entire study record (with attributes)
    ****************************************************************/

    [HttpDelete("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> DeleteFullStudy(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            var count = await _studyService.DeleteFullStudyAsync(sd_sid);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _fattType, "", sd_sid))
                : Ok(ErrorResponse("d", _fattType, "", "", sd_sid));
        } 
        return Ok(NoEntityResponse(_fattType, sd_sid));
    }
    
    /****************************************************************
    * FETCH ALL study records (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyData()
    {
        var allStudyData = await _studyService.GetStudyRecordsDataAsync();
        return allStudyData != null
            ? Ok(ListSuccessResponse(allStudyData.Count, allStudyData))
            : Ok(NoAttributesResponse(_attTypes));
    }

    /****************************************************************
    * FETCH n MOST RECENT study data (without attributes)
    ****************************************************************/
    
    [HttpGet("studies/data/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetRecentStudyData(int n)
    {
        var recentStudyData = await _studyService.GetRecentStudyRecordsAsync(n);
        return recentStudyData != null
            ? Ok(ListSuccessResponse(recentStudyData.Count, recentStudyData))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH single study record (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyData(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            var study = await _studyService.GetStudyRecordDataAsync(sd_sid);
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
        var newStudyData = await _studyService.CreateStudyRecordDataAsync(studyDataContent);
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
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            var updatedStudyData = await _studyService.CreateStudyRecordDataAsync(studyDataContent);
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
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            var count = await _studyService.DeleteStudyRecordDataAsync(sd_sid);
            return (count > 0)
                ? Ok(DeletionSuccessResponse(count, _attType, "", sd_sid))
                : Ok(ErrorResponse("d", _attType, "", sd_sid, sd_sid));
        } 
        return Ok(NoEntityResponse(_attType, sd_sid));
    }
}