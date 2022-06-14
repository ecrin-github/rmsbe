using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyApiController : BaseApiController
{
    private readonly IStudyService _studyService;

    public StudyApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
      
    /****************************************************************
    * FETCH data for a single study (including attribute data)
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> GetStudyById(string sd_sid)
    {
        var fullStudy = await _studyService.GetFullStudyByIdAsync(sd_sid);
        if (fullStudy == null) 
        {
            return Ok(NoAttributesResponse<FullStudy>("No study date found with that id."));
        }
        return Ok(new ApiResponse<FullStudy>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<FullStudy>() { fullStudy }
        });
    }
    
    /****************************************************************
    * DELETE an entire study record (with attributes)
    ****************************************************************/

    [HttpDelete("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> DeleteStudy(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<FullStudy>);
        }
        var count = await _studyService.DeleteFullStudyAsync(sd_sid);
        return Ok(new ApiResponse<FullStudy>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Full study data has been removed." }, Data = null
        });
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
    * FETCH n MOST RECENT study data (without attributes)
    ****************************************************************/
    
    [HttpGet("studies/data/recent/{n:int}")]
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
    * FETCH single study record (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyData(string sd_sid)
    {
        var study = await _studyService.GetStudyRecordDataAsync(sd_sid);
        if (study == null) 
        {
            return Ok(NoAttributesResponse<StudyData>("No study found with that id."));
        }
        return Ok(new ApiResponse<StudyData>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyData>() { study }
        });
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
    
    public async Task<IActionResult> UpdateStudyData(string sd_sid, 
        [FromBody] StudyData studyDataContent)
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
            return Ok(NoStudyResponse<StudyData>());
        }
        var count = await _studyService.DeleteStudyRecordDataAsync(sd_sid);
        return Ok(new ApiResponse<StudyTitle>()
        {
            Total = count, StatusCode = Ok().StatusCode, 
            Messages = new List<string>() { "Study record data has been removed." }, Data = null
        });
    }
}