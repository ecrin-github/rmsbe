using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyReferencesApiController : BaseApiController
{
    private readonly IStudyService _studyService;

    public StudyReferencesApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
    
    /****************************************************************
     * FETCH ALL references for a specified study
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/references")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> GetStudyReferences(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyReference>);
        }
        var studyRefs = await _studyService.GetStudyReferencesAsync(sd_sid);
        if (studyRefs == null || studyRefs.Count == 0)
        {
            return Ok(NoAttributesResponse<StudyReference>("No study references were found."));
        } 
        return Ok(new ApiResponse<StudyReference>()
        {
            Total = studyRefs.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = studyRefs
        });
    }
    
    /****************************************************************
     * FETCH A SINGLE study reference 
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> GetStudyReferences(string sd_sid, int id)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyReference>);
        }
        var studyRef = await _studyService.GetStudyReferenceAsync(id);
        if (studyRef == null) 
        {
            return Ok(NoAttributesResponse<StudyReference>("No study reference with that id found."));
        }   
        return Ok(new ApiResponse<StudyReference>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyReference>() { studyRef }
        });
    }

    /****************************************************************
     * CREATE a new reference for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sd_sid}/references")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> CreateStudyReference(string sd_sid,
        [FromBody] StudyReference studyReferenceContent)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyReference>);
        }
        studyReferenceContent.SdSid = sd_sid;
        var studyRef = await _studyService.CreateStudyReferenceAsync(studyReferenceContent);
        if (studyRef == null)
        {
            return Ok(ErrorInActionResponse<StudyReference>("Error during study reference creation."));
        }  
        return Ok(new ApiResponse<StudyReference>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyReference>() { studyRef }
        });
    }

    /****************************************************************
     * UPDATE a single specified study reference 
     ****************************************************************/
    
    [HttpPut("studies/{sd_sid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> UpdateStudyReference(string sd_sid, int id, [FromBody] StudyReference studyReferenceContent)
    {
        if (await _studyService.StudyAttributeDoesNotExistAsync(sd_sid, "StudyReference", id))
        {
            return Ok(NoAttributesResponse<StudyReference>(
                "No reference with that id found for specified study."));
        }
        var updatedStudyRef = await _studyService.UpdateStudyReferenceAsync(id, studyReferenceContent);
        if (updatedStudyRef == null)
        {
            return Ok(ErrorInActionResponse<StudyReference>("Error during study reference update."));
        }  
        return Ok(new ApiResponse<StudyReference>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyReference>() { updatedStudyRef }
        });
    }

    /****************************************************************
     * DELETE a single specified study reference 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> DeleteStudyReference(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeDoesNotExistAsync(sd_sid, "StudyReference", id))
        {
            return Ok(NoAttributesResponse<StudyReference>(
                "No reference with that id found for specified study."));
        }
        var count = await _studyService.DeleteStudyReferenceAsync(id);
        return Ok(new ApiResponse<StudyReference>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Study reference has been removed." }, Data = null
        });
    }
}
