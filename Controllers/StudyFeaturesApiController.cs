using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class StudyFeaturesApiController : BaseApiController
{
    private readonly IStudyService _studyService;

    public StudyFeaturesApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }

    /****************************************************************
    * FETCH ALL features for a specified study
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/features")]
    [SwaggerOperation(Tags = new[] { "Study features endpoint" })]
    
    public async Task<IActionResult> GetStudyFeatures(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyFeature>);
        }
        var studyFeatures = await _studyService.GetStudyFeaturesAsync(sd_sid);
        if (studyFeatures == null || studyFeatures.Count == 0)
        {
            return Ok(NoAttributesResponse<StudyFeature>("No study features were found."));
        } 
        return Ok(new ApiResponse<StudyFeature>()
        {
            Total = studyFeatures.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = studyFeatures
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE study feature 
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/features/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study features endpoint" })]
    
    public async Task<IActionResult> GetStudyFeature(string sd_sid, int id)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyFeature>);
        }
        var studyFeature = await _studyService.GetStudyFeatureAsync(id);
        if (studyFeature == null) 
        {
            return Ok(NoAttributesResponse<StudyFeature>("No study feature with that id found."));
        }    
        return Ok(new ApiResponse<StudyFeature>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyFeature>() { studyFeature }
        });
    }
    
    /****************************************************************
     * CREATE a new feature for a specified study
     ****************************************************************/

    [HttpPost("studies/{sd_sid}/features")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> CreateStudyFeature(string sd_sid, [FromBody] StudyFeature studyFeatureContent)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyFeature>);
        }
        studyFeatureContent.SdSid = sd_sid;
        var studyFeature = await _studyService.CreateStudyFeatureAsync(studyFeatureContent);
        if (studyFeature == null)
        {
            return Ok(ErrorInActionResponse<StudyFeature>("Error during study feature creation."));
        }      
        var studyFeatureList = new List<StudyFeature>() { studyFeature };
        return Ok(new ApiResponse<StudyFeature>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyFeature>() { studyFeature }
        });
    }

    /****************************************************************
     * UPDATE a single specified study feature 
     ****************************************************************/

    [HttpPut("studies/{sd_sid}/features/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> UpdateStudyFeature(string sd_sid, int id, [FromBody] StudyFeature studyFeatureContent)
    {
        if (await _studyService.StudyAttributeDoesNotExistAsync(sd_sid, "StudyFeature", id))
        {
            return Ok(NoAttributesResponse<StudyFeature>("No feature with that id found for specified study."));
        }
        var updatedStudyFeature = await _studyService.UpdateStudyFeatureAsync(id, studyFeatureContent);
        if (updatedStudyFeature == null)
        {
            return Ok(ErrorInActionResponse<StudyTitle>("Error during study feature update."));
        }
        var studyFeatureList = new List<StudyFeature>() { updatedStudyFeature };
        return Ok(new ApiResponse<StudyFeature>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyFeature>() { updatedStudyFeature }
        });
    }

    /****************************************************************
     * DELETE a single specified study feature 
     ****************************************************************/

    [HttpDelete("studies/{sd_sid}/features/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> DeleteStudyFeature(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeDoesNotExistAsync(sd_sid, "StudyFeature", id))
        {
            return Ok(NoAttributesResponse<StudyFeature>("No feature with that id found for specified study."));
        }
        var count = await _studyService.DeleteStudyFeatureAsync(id);
        return Ok(new ApiResponse<StudyFeature>()
        {
            Total = count,  StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Study feature has been removed." }, Data = null
        });
    }
}
