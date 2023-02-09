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
    [SwaggerOperation(Tags = new []{"Browsing - Study relationships endpoint"})]
    
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
    [SwaggerOperation(Tags = new[] { "Browsing - Study relationships endpoint" })]

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
}