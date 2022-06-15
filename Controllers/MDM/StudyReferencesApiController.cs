using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyReferencesApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private OkObjectResult? _response;               // given a new value on each API call
    
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
        if (await _studyService.StudyExistsAsync(sd_sid)) 
        {
            var studyRefs = await _studyService.GetStudyReferencesAsync(sd_sid);
            _response = (studyRefs == null)
                ? Ok(NoAttributesFoundResponse<StudyReference>("No study references were found."))
                : Ok(CollectionSuccessResponse(studyRefs.Count, studyRefs));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyReference>());
        return _response;
    }
    
    /****************************************************************
     * FETCH A SINGLE study reference 
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> GetStudyReferences(string sd_sid, int id)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) 
        {
            var studyRef = await _studyService.GetStudyReferenceAsync(id);
            _response = (studyRef == null)
                ? Ok(NoAttributesResponse<StudyReference>("No study reference with that id found."))
                : Ok(SingleSuccessResponse(new List<StudyReference>() { studyRef }));
        } 
        else
            _response = Ok(StudyDoesNotExistResponse<StudyReference>());
        return _response;
    }

    /****************************************************************
     * CREATE a new reference for a specified study
     ****************************************************************/

    [HttpPost("studies/{sd_sid}/references")]
    [SwaggerOperation(Tags = new[] { "Study references endpoint" })]

    public async Task<IActionResult> CreateStudyReference(string sd_sid,
        [FromBody] StudyReference studyReferenceContent)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            studyReferenceContent.SdSid = sd_sid;
            var newStudyRef = await _studyService.CreateStudyReferenceAsync(studyReferenceContent);
            _response = (newStudyRef == null)
                ? Ok(ErrorInActionResponse<StudyReference>("Error during study reference creation."))
                : Ok(SingleSuccessResponse(new List<StudyReference>() { newStudyRef }));
        }
        else
            _response = Ok(StudyDoesNotExistResponse<StudyReference>());
        return _response;
    }

    /****************************************************************
     * UPDATE a single specified study reference 
     ****************************************************************/
    
    [HttpPut("studies/{sd_sid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> UpdateStudyReference(string sd_sid, int id, 
                 [FromBody] StudyReference studyReferenceContent)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyReference", id)) 
        {
            var updatedStudyReference = await _studyService.UpdateStudyReferenceAsync(id, studyReferenceContent);
            _response = (updatedStudyReference == null)
                ? Ok(ErrorInActionResponse<StudyReference>("Error during study reference update."))
                : Ok(SingleSuccessResponse(new List<StudyReference>() { updatedStudyReference }));
        } 
        else 
            _response = Ok(MissingAttributeResponse<StudyReference>("No feature with that id found for this study."));
        return _response;  
    }

    /****************************************************************
     * DELETE a single specified study reference 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/references/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study references endpoint"})]
    
    public async Task<IActionResult> DeleteStudyReference(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyReference", id)) 
        {
            var count = await _studyService.DeleteStudyReferenceAsync(id);
            _response = (count == 0)
                ? Ok(ErrorInActionResponse<StudyReference>("Deletion does not appear to have occured."))
                : Ok(DeletionSuccessResponse<StudyReference>(count, $"Study reference {id.ToString()} removed."));
        } 
        else
            _response = Ok(MissingAttributeResponse<StudyReference>("No reference with that id found for this study."));
        return _response;        
    }
}
