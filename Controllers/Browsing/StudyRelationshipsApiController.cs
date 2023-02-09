using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class StudyRelationshipsApiController : BaseBrowsingApiController
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
    
    [HttpGet("studies/{sdSid}/relationships")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> GetStudyRelationships(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var studyRels = await _studyService.GetStudyRelationships(sdSid);
            return studyRels != null
                    ? Ok(ListSuccessResponse(studyRels.Count, studyRels))
                    : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }
    
    /****************************************************************
     * FETCH A SINGLE study relationship 
     ****************************************************************/

    [HttpGet("studies/{sdSid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study relationships endpoint" })]

    public async Task<IActionResult> GetStudyRelationship(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var studyRel = await _studyService.GetStudyRelationship(id);
            return studyRel != null
                    ? Ok(SingleSuccessResponse(new List<StudyRelationship>() { studyRel }))
                    : Ok(ErrorResponse("r", _attType, _parType, sdSid, sdSid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }
    
    /****************************************************************
     * CREATE a new relationship for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sdSid}/relationships")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> CreateStudyRelationship(string sdSid, 
                 [FromBody] StudyRelationship studyRelationshipContent)
    {
        if (await _studyService.StudyExists(sdSid)) {
             studyRelationshipContent.SdSid = sdSid;    // ensure this is the case
             var newStudyRel = await _studyService.CreateStudyRelationship(studyRelationshipContent);
             
             // N.B. The converse relationship also needs to be created
             // if it does not already exist...Dealt with in the service layer and repository.
             
             return newStudyRel != null
                 ? Ok(SingleSuccessResponse(new List<StudyRelationship>() { newStudyRel }))
                 : Ok(ErrorResponse("c", _attType, _parType, sdSid, sdSid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }

     /****************************************************************
     * UPDATE a single specified study relationship 
     ****************************************************************/
     
    [HttpPut("studies/{sdSid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study relationships endpoint" })]
    
    public async Task<IActionResult> UpdateStudyRelationship(string sdSid, int id,
                 [FromBody] StudyRelationship studyRelContent)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            studyRelContent.SdSid = sdSid;  // ensure this is the case
            studyRelContent.Id = id;
            var updatedStudyRel = await _studyService.UpdateStudyRelationship(studyRelContent);
            return updatedStudyRel != null
                    ? Ok(SingleSuccessResponse(new List<StudyRelationship>() { updatedStudyRel }))
                    : Ok(ErrorResponse("u", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }

    /****************************************************************
     * DELETE a single specified study relationship 
     ****************************************************************/
    
    [HttpDelete("studies/{sdSid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> DeleteStudyRelationship(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var count = await _studyService.DeleteStudyRelationship(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sdSid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));       
    }
}