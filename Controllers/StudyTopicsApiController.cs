using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class StudyTopicsApiController : BaseApiController
{
    private readonly IStudyDataService _studyService;

    public StudyTopicsApiController(IStudyDataService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
    
    /****************************************************************
     * FETCH ALL topics for a specified study
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/topics")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> GetStudyTopics(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyTopic>);
        }
        var studyTopics = await _studyService.GetStudyTopicsAsync(sd_sid);
        if (studyTopics == null || studyTopics.Count == 0)
        {
            return Ok(NoAttributesResponse<StudyTopic>("No study topics were found."));
        } 
        return Ok(new ApiResponse<StudyTopic>()
        {
            Total = studyTopics.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = studyTopics
        });
    }
    
    /****************************************************************
     * FETCH A SINGLE study topic 
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> GetStudyTopic(string sd_sid, int id)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyTopic>);
        }
        var studyTopic = await _studyService.GetStudyTopicAsync(id);
        if (studyTopic == null) 
        {
            return Ok(NoAttributesResponse<StudyTopic>("No study topic with that id found."));
        }
        return Ok(new ApiResponse<StudyTopic>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyTopic>() { studyTopic }
        });
    }

    /****************************************************************
     * CREATE a new topic for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sd_sid}/topics")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> CreateStudyTopic(string sd_sid, [FromBody] StudyTopic studyTopicContent)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyTopic>);
        }
        studyTopicContent.SdSid = sd_sid;
        var studyTopic = await _studyService.CreateStudyTopicAsync(studyTopicContent);
        if (studyTopic == null) 
        {
            return Ok(ErrorInActionResponse<StudyTopic>("Error during study topic creation."));
        } 
        return Ok(new ApiResponse<StudyTopic>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyTopic>() { studyTopic }
        });
    }

    /****************************************************************
     * UPDATE a single specified study topic 
     ****************************************************************/
    
    [HttpPut("studies/{sd_sid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> UpdateStudyTopic(string sd_sid, int id, [FromBody] StudyTopic studyTopicContent)
    {
        if (await _studyService.StudyAttributeDoesNotExistAsync(sd_sid, "StudyTopic", id))
        {
            return Ok(NoAttributesResponse<StudyTopic>("No topic with that id found for specified study."));
        }
        var updatedStudyTopic = await _studyService.UpdateStudyTopicAsync(id, studyTopicContent);
        if (updatedStudyTopic == null)
        {
            return Ok(ErrorInActionResponse<StudyTopic>("Error during study topic update."));
        }
        return Ok(new ApiResponse<StudyTopic>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyTopic>() { updatedStudyTopic }
        });
    }

    /****************************************************************
     * DELETE a single specified study topic 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> DeleteStudyTopic(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeDoesNotExistAsync(sd_sid, "StudyTopic", id))
        {
            return Ok(NoAttributesResponse<StudyTopic>("No topic with that id found for specified study."));
        }
        var count = await _studyService.DeleteStudyTopicAsync(id);
        return Ok(new ApiResponse<StudyTopic>()
        {
            Total = count, StatusCode = Ok().StatusCode, 
            Messages = new List<string>() { "Study topic has been removed." }, Data = null
        });
    }
}
