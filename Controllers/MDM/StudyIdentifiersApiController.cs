using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyIdentifiersApiController : BaseApiController
{
    private readonly IStudyService _studyService;

    public StudyIdentifiersApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }

    /****************************************************************
    * FETCH ALL identifiers for a specified study
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> Getstudy_identifiers(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyIdentifier>());
        }
        var studyIdents = await _studyService.GetStudyIdentifiersAsync(sd_sid);
        if (studyIdents == null || studyIdents.Count == 0)
        {
            return Ok(NoAttributesResponse<StudyIdentifier>("No study identifiers were found."));
        }
        return Ok(new ApiResponse<StudyIdentifier>
        {
            Total = studyIdents.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = studyIdents
        });
    }
    
    /****************************************************************
    * FETCH A SINGLE study identifier 
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> GetStudyIdentifier(string sd_sid, int id)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyIdentifier>());
        }
        var studyIdent = await _studyService.GetStudyIdentifierAsync(id);
        if (studyIdent == null)
        {
            return Ok(NoAttributesResponse<StudyIdentifier>("No study identifier with that id found."));
        }
        return Ok(new ApiResponse<StudyIdentifier>
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyIdentifier> { studyIdent }
        });
    }

    /****************************************************************
     * CREATE a new identifier for a specified study
     ****************************************************************/

    [HttpPost("studies/{sd_sid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> CreateStudyIdentifier(string sd_sid, [FromBody] StudyIdentifier studyIdentifierContent)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<StudyIdentifier>());
        }
        studyIdentifierContent.SdSid = sd_sid;
        var studyIdent = await _studyService.CreateStudyIdentifierAsync(studyIdentifierContent);
        if (studyIdent == null)
        {
            return Ok(ErrorInActionResponse<StudyIdentifier>("Error during study identifier creation."));
        }       
        return Ok(new ApiResponse<StudyIdentifier>
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyIdentifier> { studyIdent }
        });
    }

    /****************************************************************
     * UPDATE a single specified study identifier 
     ****************************************************************/ 

    [HttpPut("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> UpdateStudyIdentifier(string sd_sid, int id, [FromBody] StudyIdentifier studyIdentifierContent)
    {
        if (await _studyService.StudyAttributeDoesNotExistAsync(sd_sid, "StudyIdentifier", id))
        {
            return Ok(NoAttributesResponse<StudyIdentifier>("No identifier with that id found for specified study."));
        }
        var updatedStudyIdent = await _studyService.UpdateStudyIdentifierAsync(id, studyIdentifierContent);
        if (updatedStudyIdent == null)
        {
            return Ok(ErrorInActionResponse<StudyIdentifier>("Error during study identifier update."));
        } 
        return Ok(new ApiResponse<StudyIdentifier>
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<StudyIdentifier>() { updatedStudyIdent }
        });
    }

    /****************************************************************
     * DELETE a single specified study identifier 
     ****************************************************************/

    [HttpDelete("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> DeleteStudyIdentifier(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeDoesNotExistAsync(sd_sid, "StudyIdentifier", id))
        {
            return Ok(NoAttributesResponse<StudyIdentifier>("No identifier with that id found for specified study."));
        }
        var count = await _studyService.DeleteStudyIdentifierAsync(id);
        return Ok(new ApiResponse<StudyIdentifier>
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string> { "Study identifier has been removed." }, Data = null
        });
    }
}