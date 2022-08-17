using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyIdentifiersApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public StudyIdentifiersApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _parType = "study"; _parIdType = "sd_sid"; _entityType = "StudyIdentifier";
        _attType = "study identifier"; _attTypes = "study identifiers";
    }

    /****************************************************************
    * FETCH ALL identifiers for a specified study
    ****************************************************************/

    [HttpGet("studies/{sdSid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> GetStudyIdentifiers(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var studyIdents = await _studyService.GetStudyIdentifiers(sdSid);
            return studyIdents != null
                ? Ok(ListSuccessResponse(studyIdents.Count, studyIdents))
                : Ok(NoAttributesResponse(_attTypes));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }
    
    /****************************************************************
    * FETCH A SINGLE study identifier 
    ****************************************************************/

    [HttpGet("studies/{sdSid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> GetStudyIdentifier(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var studyIdent = await _studyService.GetStudyIdentifier(id);
            return studyIdent != null
                    ? Ok(SingleSuccessResponse(new List<StudyIdentifier>(){ studyIdent }))
                    : Ok(ErrorResponse("r", _attType, _parType, sdSid, sdSid));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }

    /****************************************************************
     * CREATE a new identifier for a specified study
     ****************************************************************/

    [HttpPost("studies/{sdSid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> CreateStudyIdentifier(string sdSid, 
                 [FromBody] StudyIdentifier studyIdentifierContent)
    {
        if (await _studyService.StudyExists(sdSid)) {
            studyIdentifierContent.SdSid = sdSid;  // ensure this is the case
            var newStudyIdent = await _studyService.CreateStudyIdentifier(studyIdentifierContent);
            return newStudyIdent != null
                    ? Ok(SingleSuccessResponse(new List<StudyIdentifier>() { newStudyIdent }))
                    : Ok(ErrorResponse("c", _attType, _parType, sdSid, sdSid));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));       
 }

    /****************************************************************
     * UPDATE a single specified study identifier 
     ****************************************************************/ 

    [HttpPut("studies/{sdSid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> UpdateStudyIdentifier(string sdSid, int id, 
                 [FromBody] StudyIdentifier studyIdentifierContent)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            studyIdentifierContent.SdSid = sdSid;  // ensure this is the case
            studyIdentifierContent.Id = id;
            var updatedStudyIdentifier = await _studyService.UpdateStudyIdentifier(studyIdentifierContent);
            return updatedStudyIdentifier != null
                ? Ok(SingleSuccessResponse(new List<StudyIdentifier>() { updatedStudyIdentifier }))
                : Ok(ErrorResponse("u", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }

    /****************************************************************
     * DELETE a single specified study identifier 
     ****************************************************************/

    [HttpDelete("studies/{sdSid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> DeleteStudyIdentifier(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var count = await _studyService.DeleteStudyIdentifier(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sdSid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }
}