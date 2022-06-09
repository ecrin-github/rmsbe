using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyRelationshipsApiController : BaseApiController
{
    private readonly IStudyService _studyService;

    public StudyRelationshipsApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
    
    /****************************************************************
     * FETCH ALL relationships for a specified study
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/relationships")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> GetStudyRelationships(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyRelationship>);
        }
        var studyRelationships = await _studyService.GetStudyRelationshipsAsync(sd_sid);
        if (studyRelationships == null || studyRelationships.Count == 0)
        {
            return Ok(NoAttributesResponse<StudyRelationship>("No study relationships were found."));
        } 
        return Ok(new ApiResponse<StudyRelationship>()
        {
            Total = studyRelationships.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = studyRelationships
        });
    }
    
    /****************************************************************
     * FETCH A SINGLE study relationship 
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> GetStudyRelationship(string sd_sid, int id)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyRelationship>);
        }
        var studyRel = await _studyService.GetStudyRelationshipAsync(id);
        if (studyRel == null) 
        {
            return Ok(NoAttributesResponse<StudyRelationship>("No study relationship with that id found."));
        }    
        return Ok(new ApiResponse<StudyRelationship>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyRelationship>() { studyRel }
        });
    }

    /****************************************************************
     * CREATE a new relationship for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sd_sid}/relationships")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> CreateStudyRelationship(string sd_sid, [FromBody] StudyRelationship studyRelationshipContent)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyRelationship>);
        }
        studyRelationshipContent.SdSid = sd_sid;
        var studyRel = await _studyService.CreateStudyRelationshipAsync(studyRelationshipContent);
        if (studyRel == null)
        {
            return Ok(ErrorInActionResponse<StudyRelationship>("Error during study relationship creation."));
        }  
        var studyRelList = new List<StudyRelationship>() { studyRel };
        return Ok(new ApiResponse<StudyRelationship>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyRelationship>() { studyRel }
        });
    }

     /****************************************************************
     * UPDATE a single specified study relationship 
     ****************************************************************/
     
    [HttpPut("studies/{sd_sid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study relationships endpoint" })]
    
    public async Task<IActionResult> UpdateStudyRelationship(string sd_sid, int id,
        [FromBody] StudyRelationship studyRelationshipContent)
    {
        if (await _studyService.StudyAttributeDoesNotExistAsync(sd_sid, "StudyRelationship", id))
        {
            return Ok(NoAttributesResponse<StudyRelationship>(
                "No relationship with that id found for specified study."));
        }
        var updatedStudyRel = await _studyService.UpdateStudyRelationshipAsync(id, studyRelationshipContent);
        if (updatedStudyRel == null)
        {
            return Ok(ErrorInActionResponse<StudyRelationship>("Error during study relationship update."));
        }
        return Ok(new ApiResponse<StudyRelationship>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyRelationship>() { updatedStudyRel }
        });
    }

    /****************************************************************
     * DELETE a single specified study relationship 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> DeleteStudyRelationship(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeDoesNotExistAsync(sd_sid, "StudyRelationship", id))
        {
            return Ok(NoAttributesResponse<StudyRelationship>("No relationship with that id found for specified study."));
        }
        var count = await _studyService.DeleteStudyRelationshipAsync(id);
        return Ok(new ApiResponse<StudyRelationship>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Study relationship has been removed." }, Data = null
        });
    }
}
