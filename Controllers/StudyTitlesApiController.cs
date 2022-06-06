using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class StudyTitlesApiController : BaseApiController
{
    private readonly IStudyDataService _studyService;

    public StudyTitlesApiController(IStudyDataService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
    
    /****************************************************************
    * FETCH ALL titles for a specified study
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/titles")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> GetStudyTitles(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExist(sd_sid))
        {
            return Ok(NoStudyResponse<StudyTitle>);
        }
        var studyTitles = await _studyService.GetStudyTitlesAsync(sd_sid);
        if (studyTitles == null || studyTitles.Count == 0)
        {
            return Ok(NoAttributesResponse<StudyTitle>("No study titles were found."));
        }
        return Ok(new ApiResponse<StudyTitle>()
        {
            Total = studyTitles.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = studyTitles
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE study title 
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> GetStudyTitle(string sd_sid, int id)
    {
        if (await _studyService.StudyDoesNotExist(sd_sid))
        {
            return Ok(NoStudyResponse<StudyTitle>);
        }
        var studyTitle = await _studyService.GetStudyTitleAsync(id);
        if (studyTitle == null) 
        {
            return Ok(NoAttributesResponse<StudyTitle>("No study date with that id found."));
        }
        return Ok(new ApiResponse<StudyTitle>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyTitle>() { studyTitle }
        });
    }

    /****************************************************************
     * CREATE a new title for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sd_sid}/titles")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> CreateStudyTitle(string sd_sid, [FromBody] StudyTitle studyTitleContent)
    {
        if (await _studyService.StudyDoesNotExist(sd_sid))
        {
            return Ok(NoStudyResponse<StudyTitle>);
        }
        studyTitleContent.SdSid = sd_sid;
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var studyTitle = await _studyService.CreateStudyTitleAsync(studyTitleContent, accessToken);
        if (studyTitle == null) 
        {
            return Ok(ErrorInActionResponse<StudyTitle>("Error during study title creation."));
        }   
        return Ok(new ApiResponse<StudyTitle>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyTitle>() { studyTitle }
        });
    }

    /****************************************************************
     * UPDATE a single specified study title 
     ****************************************************************/
    
    [HttpPut("studies/{sd_sid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> UpdateStudyTitle(string sd_sid, int id, [FromBody] StudyTitle studyTitleContent)
    {
        if (await _studyService.StudyAttributeDoesNotExist(sd_sid, "StudyTitle", id))
        {
            return Ok(NoAttributesResponse<StudyTitle>("No title with that id found for specified study."));
        }
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var updatedStudyTitle = await _studyService.UpdateStudyTitleAsync(id, studyTitleContent, accessToken);
        if (updatedStudyTitle == null)
        {
            return Ok(ErrorInActionResponse<StudyTitle>("Error during study title update."));
        }
        return Ok(new ApiResponse<StudyTitle>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyTitle>() { updatedStudyTitle }
        });
    }

    /****************************************************************
     * DELETE a single specified study title 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> DeleteStudyTitle(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeDoesNotExist(sd_sid, "StudyTitle", id))
        {
            return Ok(NoAttributesResponse<StudyTitle>("No title with that id found for specified study."));
        }
        var count = await _studyService.DeleteStudyTitleAsync(id);
        return Ok(new ApiResponse<StudyTitle>()
        {
            Total = count, StatusCode = Ok().StatusCode, 
            Messages = new List<string>() { "Study title has been removed." }, Data = null
        });
    }
}
