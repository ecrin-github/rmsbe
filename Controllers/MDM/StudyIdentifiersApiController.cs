using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyIdentifiersApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private OkObjectResult? _response;               // given a new value on each API call

    public StudyIdentifiersApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }

    /****************************************************************
    * FETCH ALL identifiers for a specified study
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> GetStudyIdentifiers(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) 
        {
            var studyIdents = await _studyService.GetStudyIdentifiersAsync(sd_sid);
            _response = (studyIdents == null)
                ? Ok(NoAttributesFoundResponse<StudyIdentifier>("No study identifiers were found."))
                : Ok(CollectionSuccessResponse(studyIdents.Count, studyIdents));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyIdentifier>());
        return _response;
    }
    
    /****************************************************************
    * FETCH A SINGLE study identifier 
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> GetStudyIdentifier(string sd_sid, int id)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            var studyIdent = await _studyService.GetStudyIdentifierAsync(id);
            _response = (studyIdent == null)
                ? Ok(NoAttributesResponse<StudyIdentifier>("No study identifier with that id found."))
                : Ok(SingleSuccessResponse(new List<StudyIdentifier>() { studyIdent }));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyIdentifier>());
        return _response;
    }

    /****************************************************************
     * CREATE a new identifier for a specified study
     ****************************************************************/

    [HttpPost("studies/{sd_sid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> CreateStudyIdentifier(string sd_sid, 
                 [FromBody] StudyIdentifier studyIdentifierContent)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            studyIdentifierContent.SdSid = sd_sid;
            var newStudyIdent = await _studyService.CreateStudyIdentifierAsync(studyIdentifierContent);
            _response = (newStudyIdent == null)
                ? Ok(ErrorInActionResponse<StudyIdentifier>("Error during study feature creation."))
                : Ok(SingleSuccessResponse(new List<StudyIdentifier>() { newStudyIdent }));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyIdentifier>());
        return _response;         
 }

    /****************************************************************
     * UPDATE a single specified study identifier 
     ****************************************************************/ 

    [HttpPut("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> UpdateStudyIdentifier(string sd_sid, int id, 
                 [FromBody] StudyIdentifier studyIdentContent)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyIdentifier", id)) 
        {
            var updatedStudyIdentifier = await _studyService.UpdateStudyIdentifierAsync(id, studyIdentContent);
            _response = (updatedStudyIdentifier == null)
                ? Ok(ErrorInActionResponse<StudyIdentifier>("Error during study identifier update."))
                : Ok(SingleSuccessResponse(new List<StudyIdentifier>() { updatedStudyIdentifier }));
        } 
        else 
            _response = Ok(MissingAttributeResponse<StudyFeature>("No identifier with that id found for this study."));
        return _response;  
    }

    /****************************************************************
     * DELETE a single specified study identifier 
     ****************************************************************/

    [HttpDelete("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> DeleteStudyIdentifier(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyIdentifier", id)) 
        {
            var count = await _studyService.DeleteStudyIdentifierAsync(id);
            _response = (count == 0)
                ? Ok(ErrorInActionResponse<StudyIdentifier>("Deletion does not appear to have occured."))
                : Ok(DeletionSuccessResponse<StudyIdentifier>(count, $"Study identifier {id.ToString()} removed."));
        } 
        else
            _response = Ok(MissingAttributeResponse<StudyIdentifier>("No identifier with that id found for this study."));
        return _response;    
    }
}