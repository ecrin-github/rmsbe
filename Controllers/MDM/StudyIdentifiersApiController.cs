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

    [HttpGet("studies/{sd_sid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> GetStudyIdentifiers(string sd_sid)
    {
        if (await _studyService.StudyExists(sd_sid)) {
            var studyIdents = await _studyService.GetStudyIdentifiers(sd_sid);
            return studyIdents != null
                ? Ok(ListSuccessResponse(studyIdents.Count, studyIdents))
                : Ok(NoAttributesResponse(_attTypes));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }
    
    /****************************************************************
    * FETCH A SINGLE study identifier 
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> GetStudyIdentifier(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExists(sd_sid, _entityType, id)) {
            var studyIdent = await _studyService.GetStudyIdentifier(id);
            return studyIdent != null
                    ? Ok(SingleSuccessResponse(new List<StudyIdentifier>(){ studyIdent }))
                    : Ok(ErrorResponse("r", _attType, _parType, sd_sid, sd_sid));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }

    /****************************************************************
     * CREATE a new identifier for a specified study
     ****************************************************************/

    [HttpPost("studies/{sd_sid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> CreateStudyIdentifier(string sd_sid, 
                 [FromBody] StudyIdentifier studyIdentifierContent)
    {
        if (await _studyService.StudyExists(sd_sid)) {
            studyIdentifierContent.SdSid = sd_sid;
            var newStudyIdent = await _studyService.CreateStudyIdentifier(studyIdentifierContent);
            return newStudyIdent != null
                    ? Ok(SingleSuccessResponse(new List<StudyIdentifier>() { newStudyIdent }))
                    : Ok(ErrorResponse("c", _attType, _parType, sd_sid, sd_sid));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));       
 }

    /****************************************************************
     * UPDATE a single specified study identifier 
     ****************************************************************/ 

    [HttpPut("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> UpdateStudyIdentifier(string sd_sid, int id, 
                 [FromBody] StudyIdentifier studyIdentContent)
    {
        if (await _studyService.StudyAttributeExists(sd_sid, _entityType, id)) {
            var updatedStudyIdentifier = await _studyService.UpdateStudyIdentifier(id, studyIdentContent);
            return updatedStudyIdentifier != null
                ? Ok(SingleSuccessResponse(new List<StudyIdentifier>() { updatedStudyIdentifier }))
                : Ok(ErrorResponse("u", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }

    /****************************************************************
     * DELETE a single specified study identifier 
     ****************************************************************/

    [HttpDelete("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    
    public async Task<IActionResult> DeleteStudyIdentifier(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExists(sd_sid, _entityType, id)) {
            var count = await _studyService.DeleteStudyIdentifier(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sd_sid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }
}