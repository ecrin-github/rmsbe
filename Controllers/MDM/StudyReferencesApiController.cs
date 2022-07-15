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
    
    [HttpGet("studies/{sdSid}/references")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> GetStudyReferences(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var studyRefs = await _studyService.GetStudyReferences(sdSid);
            return studyRefs != null
                    ? Ok(ListSuccessResponse(studyRefs.Count, studyRefs))
                    : Ok(NoAttributesResponse(_attTypes));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }
    
    /****************************************************************
     * FETCH A SINGLE study reference 
     ****************************************************************/
    
    [HttpGet("studies/{sdSid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> GetStudyReferences(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var studyRef = await _studyService.GetStudyReference(id);
            return studyRef != null
                    ? Ok(SingleSuccessResponse(new List<StudyReference>() { studyRef }))
                    : Ok(ErrorResponse("r", _attType, _parType, sdSid, sdSid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }

    /****************************************************************
     * CREATE a new reference for a specified study
     ****************************************************************/

    [HttpPost("studies/{sdSid}/references")]
    [SwaggerOperation(Tags = new[] { "Study references endpoint" })]

    public async Task<IActionResult> CreateStudyReference(string sdSid,
        [FromBody] StudyReference studyReferenceContent)
    {
        if (await _studyService.StudyExists(sdSid)) {
            studyReferenceContent.SdSid = sdSid;
            var newStudyRef = await _studyService.CreateStudyReference(studyReferenceContent);
            return newStudyRef != null
                    ? Ok(SingleSuccessResponse(new List<StudyReference>() { newStudyRef }))
                    : Ok(ErrorResponse("c", _attType, _parType, sdSid, sdSid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }

    /****************************************************************
     * UPDATE a single specified study reference 
     ****************************************************************/
    
    [HttpPut("studies/{sdSid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> UpdateStudyReference(string sdSid, int id, 
                 [FromBody] StudyReference studyReferenceContent)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var updatedStudyReference = await _studyService.UpdateStudyReference(id, studyReferenceContent);
            return updatedStudyReference != null
                    ? Ok(SingleSuccessResponse(new List<StudyReference>() { updatedStudyReference }))
                    : Ok(ErrorResponse("u", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }

    /****************************************************************
     * DELETE a single specified study reference 
     ****************************************************************/
    
    [HttpDelete("studies/{sdSid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> DeleteStudyReference(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var count = await _studyService.DeleteStudyReference(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sdSid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));    
    }
}