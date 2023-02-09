using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class StudyFeaturesApiController : BaseBrowsingApiController
{
    private readonly IStudyService _studyService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public StudyFeaturesApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _parType = "study"; _parIdType = "sd_sid"; _entityType = "StudyFeature";
        _attType = "study feature"; _attTypes = "study features";
    }

    /****************************************************************
    * FETCH ALL features for a specified study
    ****************************************************************/

    [HttpGet("studies/{sdSid}/features")]
    [SwaggerOperation(Tags = new[] { "Browsing - Study features endpoint" })]

    public async Task<IActionResult> GetStudyFeatures(string sdSid) 
    {
        if (await _studyService.StudyExists(sdSid)) {
            var studyFeatures = await _studyService.GetStudyFeatures(sdSid);
            return studyFeatures != null
                    ? Ok(ListSuccessResponse(studyFeatures.Count, studyFeatures))
                    : Ok(NoAttributesResponse(_attTypes));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }
    
    /****************************************************************
    * FETCH A SINGLE study feature 
    ****************************************************************/

    [HttpGet("studies/{sdSid}/features/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Browsing - Study features endpoint" })]
    
    public async Task<IActionResult> GetStudyFeature(string sdSid, int id) 
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var studyFeature = await _studyService.GetStudyFeature(id);
            return studyFeature != null
                    ? Ok(SingleSuccessResponse(new List<StudyFeature>() { studyFeature }))
                    : Ok(ErrorResponse("r", _attType, _parType, sdSid, sdSid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }
}