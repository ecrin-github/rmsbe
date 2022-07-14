using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyRelationshipsApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public StudyRelationshipsApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _parType = "study"; _parIdType = "sd_sid"; _entityType = "StudyRelationship";
        _attType = "study relationship"; _attTypes = "study relationships";
    }
    
    /****************************************************************
     * FETCH ALL relationships for a specified study
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/relationships")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> GetStudyRelationships(string sd_sid)
    {
        if (await _studyService.StudyExists(sd_sid)) {
            var studyRels = await _studyService.GetStudyRelationships(sd_sid);
            return studyRels != null
                    ? Ok(ListSuccessResponse(studyRels.Count, studyRels))
                    : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }
    
    /****************************************************************
     * FETCH A SINGLE study relationship 
     ****************************************************************/

    [HttpGet("studies/{sd_sid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study relationships endpoint" })]

    public async Task<IActionResult> GetStudyRelationship(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExists(sd_sid, _entityType, id)) {
            var studyRel = await _studyService.GetStudyRelationship(id);
            return studyRel != null
                    ? Ok(SingleSuccessResponse(new List<StudyRelationship>() { studyRel }))
                    : Ok(ErrorResponse("r", _attType, _parType, sd_sid, sd_sid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }
    
    /****************************************************************
     * CREATE a new relationship for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sd_sid}/relationships")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> CreateStudyRelationship(string sd_sid, 
                 [FromBody] StudyRelationship studyRelationshipContent)
    {
        if (await _studyService.StudyExists(sd_sid)) {
             studyRelationshipContent.SdSid = sd_sid;
             var newStudyRel = await _studyService.CreateStudyRelationship(studyRelationshipContent);
             return newStudyRel != null
                 ? Ok(SingleSuccessResponse(new List<StudyRelationship>() { newStudyRel }))
                 : Ok(ErrorResponse("c", _attType, _parType, sd_sid, sd_sid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }

     /****************************************************************
     * UPDATE a single specified study relationship 
     ****************************************************************/
     
    [HttpPut("studies/{sd_sid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study relationships endpoint" })]
    
    public async Task<IActionResult> UpdateStudyRelationship(string sd_sid, int id,
                 [FromBody] StudyRelationship studyRelContent)
    {
        if (await _studyService.StudyAttributeExists(sd_sid, _entityType, id)) {
            var updatedStudyRel = await _studyService.UpdateStudyRelationship(id, studyRelContent);
            return updatedStudyRel != null
                    ? Ok(SingleSuccessResponse(new List<StudyRelationship>() { updatedStudyRel }))
                    : Ok(ErrorResponse("u", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }

    /****************************************************************
     * DELETE a single specified study relationship 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> DeleteStudyRelationship(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExists(sd_sid, _entityType, id)) {
            var count = await _studyService.DeleteStudyRelationship(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sd_sid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));       
    }
}