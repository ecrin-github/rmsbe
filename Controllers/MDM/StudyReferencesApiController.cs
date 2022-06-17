using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyReferencesApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public StudyReferencesApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _parType = "study"; _parIdType = "sd_sid"; _entityType = "StudyReference";
        _attType = "study reference"; _attTypes = "study references";
    }
    
    /****************************************************************
     * FETCH ALL references for a specified study
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/references")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> GetStudyReferences(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            var studyRefs = await _studyService.GetStudyReferencesAsync(sd_sid);
            return studyRefs != null
                    ? Ok(ListSuccessResponse(studyRefs.Count, studyRefs))
                    : Ok(NoAttributesResponse(_attTypes));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }
    
    /****************************************************************
     * FETCH A SINGLE study reference 
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> GetStudyReferences(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, _entityType, id)) {
            var studyRef = await _studyService.GetStudyReferenceAsync(id);
            return studyRef != null
                    ? Ok(SingleSuccessResponse(new List<StudyReference>() { studyRef }))
                    : Ok(ErrorResponse("r", _attType, _parType, sd_sid, sd_sid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }

    /****************************************************************
     * CREATE a new reference for a specified study
     ****************************************************************/

    [HttpPost("studies/{sd_sid}/references")]
    [SwaggerOperation(Tags = new[] { "Study references endpoint" })]

    public async Task<IActionResult> CreateStudyReference(string sd_sid,
        [FromBody] StudyReference studyReferenceContent)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            studyReferenceContent.SdSid = sd_sid;
            var newStudyRef = await _studyService.CreateStudyReferenceAsync(studyReferenceContent);
            return newStudyRef != null
                    ? Ok(SingleSuccessResponse(new List<StudyReference>() { newStudyRef }))
                    : Ok(ErrorResponse("c", _attType, _parType, sd_sid, sd_sid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }

    /****************************************************************
     * UPDATE a single specified study reference 
     ****************************************************************/
    
    [HttpPut("studies/{sd_sid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> UpdateStudyReference(string sd_sid, int id, 
                 [FromBody] StudyReference studyReferenceContent)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, _entityType, id)) {
            var updatedStudyReference = await _studyService.UpdateStudyReferenceAsync(id, studyReferenceContent);
            return updatedStudyReference != null
                    ? Ok(SingleSuccessResponse(new List<StudyReference>() { updatedStudyReference }))
                    : Ok(ErrorResponse("u", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }

    /****************************************************************
     * DELETE a single specified study reference 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> DeleteStudyReference(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, _entityType, id)) {
            var count = await _studyService.DeleteStudyReferenceAsync(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sd_sid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));    
    }
}