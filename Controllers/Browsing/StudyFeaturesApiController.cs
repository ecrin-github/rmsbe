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
    [SwaggerOperation(Tags = new[] { "Study features endpoint" })]

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
    [SwaggerOperation(Tags = new[] { "Study features endpoint" })]
    
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
    
    /****************************************************************
     * CREATE a new feature for a specified study
     ****************************************************************/

    [HttpPost("studies/{sdSid}/features")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> CreateStudyFeature(string sdSid, 
                 [FromBody] StudyFeature studyFeatureContent) 
    {
        if (await _studyService.StudyExists(sdSid)) {
            studyFeatureContent.SdSid = sdSid;   // ensure this is the case
            var newStudyFeature = await _studyService.CreateStudyFeature(studyFeatureContent);
            return newStudyFeature != null
                    ? Ok(SingleSuccessResponse(new List<StudyFeature>() { newStudyFeature }))
                    : Ok(ErrorResponse("c", _attType, _parType, sdSid, sdSid));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }

    /****************************************************************
    * UPDATE a single specified study feature 
    ****************************************************************/

    [HttpPut("studies/{sdSid}/features/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> UpdateStudyFeature(string sdSid, int id,  
                 [FromBody] StudyFeature studyFeatureContent)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            studyFeatureContent.SdSid = sdSid;  // ensure this is the case
            studyFeatureContent.Id = id;
            var updatedStudyFeature = await _studyService.UpdateStudyFeature(studyFeatureContent);
            return updatedStudyFeature != null
                    ? Ok(SingleSuccessResponse(new List<StudyFeature>() { updatedStudyFeature }))
                    : Ok(ErrorResponse("u", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }

    /****************************************************************
     * DELETE a single specified study feature 
     ****************************************************************/

    [HttpDelete("studies/{sdSid}/features/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> DeleteStudyFeature(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var count = await _studyService.DeleteStudyFeature(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sdSid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));      
    }
}