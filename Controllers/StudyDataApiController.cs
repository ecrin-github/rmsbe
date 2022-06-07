using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class StudyDataApiController : BaseApiController
{
    private readonly IStudyDataService _studyService;

    public StudyDataApiController(IStudyDataService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
    
    /****************************************************************
    * FETCH ALL study records (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyData()
    {
        var allStudyRecordData = await _studyService.GetStudyRecordsDataAsync();
        if (allStudyRecordData == null || allStudyRecordData.Count == 0)
        {
            return Ok(NoAttributesResponse<StudyData>("No study records were found."));
        }
        return Ok(new ApiResponse<StudyData>()
        {
            Total = allStudyRecordData.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = allStudyRecordData
        });
    }

    /****************************************************************
    * FETCH single study record (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyData(string sd_sid)
    {
        var study = await _studyService.GetStudyRecordDataAsync(sd_sid);
        if (study == null) 
        {
            return Ok(NoAttributesResponse<StudyData>("No study date found with that id."));
        }
        return Ok(new ApiResponse<StudyData>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyData>() { study }
        });
    }
    
    /****************************************************************
    * FETCH n MOST RECENT study data (without attributes)
    ****************************************************************/
    
    [HttpGet("studies/data/recent/{number:int}")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetRecentStudyData(int n)
    {
        var recentStudyData = await _studyService.GetRecentStudyRecordsAsync(n);
        if (recentStudyData == null || recentStudyData.Count == 0)
        {
            return Ok(NoAttributesResponse<StudyData>("No study records were found."));
        }
        return Ok(new ApiResponse<StudyData>()
        {
            Total = recentStudyData.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = recentStudyData
        });
    }
    
    /****************************************************************
    * CREATE a new study record (in studies table only)
    ****************************************************************/

    [HttpPost("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> CreateStudyData(string sd_sid, [FromBody] StudyData studyDataContent)
    {
        studyDataContent.SdSid = sd_sid;
        var studyData = await _studyService.CreateStudyRecordDataAsync(studyDataContent);
        if (studyData == null)
        {
            return Ok(ErrorInActionResponse<StudyData>("Error during study record creation."));
        }     
        return Ok(new ApiResponse<StudyData>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyData>() { studyData }
        });
    }
    
    /****************************************************************
    * UPDATE a specified study record (in studies table only)
    ****************************************************************/

    [HttpPut("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> UpdateStudyData(string sd_sid, [FromBody] StudyData studyDataContent)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyData>);
        }
        studyDataContent.SdSid = sd_sid;
        var updatedStudy = await _studyService.UpdateStudyRecordDataAsync(studyDataContent);
        if (updatedStudy == null)
        {
            return Ok(ErrorInActionResponse<StudyData>("Error during study record update."));
        }    
        return Ok(new ApiResponse<StudyData>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyData>() { updatedStudy }
        });
    }
    
    /****************************************************************
    * DELETE a specified study record (from studies table only) 
    ****************************************************************/

    [HttpDelete("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new[] { "Study data endpoint" })]

    public async Task<IActionResult> DeleteStudyData(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyData>);
        }
        var count = await _studyService.DeleteStudyRecordDataAsync(sd_sid);
        return Ok(new ApiResponse<StudyTitle>()
        {
            Total = count, StatusCode = Ok().StatusCode, 
            Messages = new List<string>() { "Study record data has been removed." }, Data = null
        });
    }
    
}
