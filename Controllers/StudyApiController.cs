using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class StudyApiController : BaseApiController
{
    private readonly IStudyDataService _studyService;

    public StudyApiController(IStudyDataService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
        
    /****************************************************************
    * FETCH ALL study records (including all attribute data)
    ****************************************************************/
    /*
    [HttpGet("studies")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    

    public async Task<IActionResult> GetAllStudies()
    {
        var fullStudies = await _studyService.GetFullStudyDataAsync();
        if (fullStudies == null || fullStudies.Count == 0)
        {
            return Ok(NoAttributesResponse<Study>("No study records were found."));
        }
        return Ok(new ApiResponse<Study>()
        {
            Total = fullStudies.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = fullStudies
        });
    }
    */    
    /****************************************************************
    * FETCH data for a single study (including attribute data)
    ****************************************************************/
    /*
    [HttpGet("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> GetStudyById(string sd_sid)
    {
        var fullStudy = await _studyService.GetFullStudyByIdAsync(sd_sid);
        if (fullStudy == null) 
        {
            return Ok(NoAttributesResponse<Study>("No study date found with that id."));
        }
        return Ok(new ApiResponse<Study>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Study>() { fullStudy }
        });
    }
    */
    /****************************************************************
    * CREATE a new study record (with attributes)
    ****************************************************************/
    /*
    [HttpPost("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> CreateStudy(string sd_sid, [FromBody] Study studyContent)
    {
        studyContent.SdSid = sd_sid;
        var fullStudy = await _studyService.CreateFullStudyAsync(studyContent);
        if (fullStudy == null)
        {
            return Ok(ErrorInActionResponse<Study>("Error during study creation."));
        } 
        return Ok(new ApiResponse<Study>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Study>() { fullStudy }
        });
    }
    */
    /****************************************************************
    * UPDATE an entire study record (with attribute data)
    ****************************************************************/
    /*
    [HttpPut("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> UpdateStudy(string sd_sid, [FromBody] Study studyContent)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<Study>);
        }
        studyContent.SdSid = sd_sid;
        var updatedFullStudy = await _studyService.UpdateFullStudyAsync(studyContent);
        if (updatedFullStudy == null)
        {
            return Ok(ErrorInActionResponse<Study>("Error during study update."));
        } 
        return Ok(new ApiResponse<Study>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Study>() { updatedFullStudy }
        });
    }
    */

    /****************************************************************
    * DELETE an entire study record (with attributes)
    ****************************************************************/

    [HttpDelete("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> DeleteStudy(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<Study>);
        }
        var count = await _studyService.DeleteFullStudyAsync(sd_sid);
        return Ok(new ApiResponse<Study>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Full study data has been removed." }, Data = null
        });
    }
}