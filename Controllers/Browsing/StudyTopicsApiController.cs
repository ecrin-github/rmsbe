using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class StudyTopicsApiController : BaseBrowsingApiController
{
    private readonly IStudyService _studyService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public StudyTopicsApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _parType = "study"; _parIdType = "sd_sid"; _entityType = "StudyTopic";
        _attType = "study topic"; _attTypes = "study topics";
    }
    
    /****************************************************************
     * FETCH ALL topics for a specified study
     ****************************************************************/
    
    [HttpGet("studies/{sdSid}/topics")]
    [SwaggerOperation(Tags = new []{"Browsing - Study topics endpoint"})]
    
    public async Task<IActionResult> GetStudyTopics(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var studyTopics = await _studyService.GetStudyTopics(sdSid);
            return studyTopics != null
                    ? Ok(ListSuccessResponse(studyTopics.Count, studyTopics))
                    : Ok(NoAttributesResponse(_attTypes));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }
    
    /****************************************************************
     * FETCH A SINGLE study topic 
     ****************************************************************/
    
    [HttpGet("studies/{sdSid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Browsing - Study topics endpoint"})]
    
    public async Task<IActionResult> GetStudyTopic(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var studyTopic = await _studyService.GetStudyTopic(id);
            return studyTopic != null
                    ? Ok(SingleSuccessResponse(new List<StudyTopic>() { studyTopic }))
                    : Ok(ErrorResponse("r", _attType, _parType, sdSid, sdSid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }
}
