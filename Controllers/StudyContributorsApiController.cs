using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class StudyContributorsApiController : BaseApiController
{
    private readonly IStudyDataService _studyService;

    public StudyContributorsApiController(IStudyDataService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }

    /****************************************************************
    * FETCH ALL contributors for a specified study
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/contributors")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> GetStudyContributors(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExist(sd_sid))
        {
            return Ok(NoStudyResponse<StudyContributor>);
        }
        var studyContribs = await _studyService.GetStudyContributorsAsync(sd_sid);
        if (studyContribs == null || studyContribs.Count == 0)
        {
            return Ok(NoAttributesResponse<StudyContributor>("No study contributors were found."));
        }    
        return Ok(new ApiResponse<StudyContributor>()
        {
            Total = studyContribs.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = studyContribs
        });
    }

    /****************************************************************
    * FETCH A SINGLE study contributor 
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> GetStudyContributor(string sd_sid, int id)
    {
        if (await _studyService.StudyDoesNotExist(sd_sid))
        {
            return Ok(NoStudyResponse<StudyContributor>);
        }
        var studyContributor = await _studyService.GetStudyContributorAsync(id);
        if (studyContributor == null) 
        {
            return Ok(NoAttributesResponse<StudyContributor>("No study contributor with that id found."));
        }
        return Ok(new ApiResponse<StudyContributor>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyContributor>() { studyContributor }
        });
    }

    /****************************************************************
     * CREATE a new contributor for a specified study
     ****************************************************************/

    [HttpPost("studies/{sd_sid}/contributors")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> CreateStudyContributor(string sd_sid, [FromBody] StudyContributor studyContributorContent)
    {
        if (await _studyService.StudyDoesNotExist(sd_sid))
        {
            return Ok(NoStudyResponse<StudyContributor>);
        }
        studyContributorContent.SdSid = sd_sid;
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var studyContrib = await _studyService.CreateStudyContributorAsync(studyContributorContent, accessToken);
        if (studyContrib == null)
        {
            return Ok(ErrorInActionResponse<StudyContributor>("Error during study contributor creation."));
        }  
        return Ok(new ApiResponse<StudyContributor>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyContributor>() { studyContrib }
        });
    }

    /****************************************************************
     * UPDATE a single specified study contributor 
     ****************************************************************/

    [HttpPut("studies/{sd_sid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> UpdateStudyContributor(string sd_sid, int id, [FromBody] StudyContributor studyContributorContent)
    {
        if (await _studyService.StudyAttributeDoesNotExist(sd_sid, "StudyContributor", id))
        {
            return Ok(NoAttributesResponse<StudyContributor>("No contributor with that id found for specified study."));
        }
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var updatedStudyContrib = await _studyService.UpdateStudyContributorAsync(id, studyContributorContent, accessToken);
        if (updatedStudyContrib == null)
        {
            return Ok(ErrorInActionResponse<StudyContributor>("Error during study contributor update."));
        }
        return Ok(new ApiResponse<StudyContributor>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyContributor>() { updatedStudyContrib }
        });
    }

    /****************************************************************
     * DELETE a single specified study contributor 
     ****************************************************************/

    [HttpDelete("studies/{sd_sid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> DeleteStudyContributor(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeDoesNotExist(sd_sid, "StudyContributor", id))
        {
            return Ok(NoAttributesResponse<StudyContributor>("No contributor with that id found for specified study."));
        }
        var count = await _studyService.DeleteStudyContributorAsync(id);
        return Ok(new ApiResponse<StudyContributor>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Study contributor has been removed." }, Data = null
        });
    }
}
