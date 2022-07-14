using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyFeaturesApiController : BaseApiController
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

    [HttpGet("studies/{sd_sid}/features")]
    [SwaggerOperation(Tags = new[] { "Study features endpoint" })]

    public async Task<IActionResult> GetStudyFeatures(string sd_sid) 
    {
        if (await _studyService.StudyExists(sd_sid)) {
            var studyFeatures = await _studyService.GetStudyFeatures(sd_sid);
            return studyFeatures != null
                    ? Ok(ListSuccessResponse(studyFeatures.Count, studyFeatures))
                    : Ok(NoAttributesResponse(_attTypes));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }
    
    /****************************************************************
    * FETCH A SINGLE study feature 
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/features/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study features endpoint" })]
    
    public async Task<IActionResult> GetStudyFeature(string sd_sid, int id) 
    {
        if (await _studyService.StudyAttributeExists(sd_sid, _entityType, id)) {
            var studyFeature = await _studyService.GetStudyFeature(id);
            return studyFeature != null
                    ? Ok(SingleSuccessResponse(new List<StudyFeature>() { studyFeature }))
                    : Ok(ErrorResponse("r", _attType, _parType, sd_sid, sd_sid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }
    
    /****************************************************************
     * CREATE a new feature for a specified study
     ****************************************************************/

    [HttpPost("studies/{sd_sid}/features")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> CreateStudyFeature(string sd_sid, 
                 [FromBody] StudyFeature studyFeatureContent) 
    {
        if (await _studyService.StudyExists(sd_sid)) {
            studyFeatureContent.SdSid = sd_sid;
            var newStudyFeature = await _studyService.CreateStudyFeature(studyFeatureContent);
            return newStudyFeature != null
                    ? Ok(SingleSuccessResponse(new List<StudyFeature>() { newStudyFeature }))
                    : Ok(ErrorResponse("c", _attType, _parType, sd_sid, sd_sid));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }

    /****************************************************************
    * UPDATE a single specified study feature 
    ****************************************************************/

    [HttpPut("studies/{sd_sid}/features/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> UpdateStudyFeature(string sd_sid, int id,  
                 [FromBody] StudyFeature studyFeatureContent)
    {
        if (await _studyService.StudyAttributeExists(sd_sid, _entityType, id)) {
            var updatedStudyFeature = await _studyService.UpdateStudyFeature(id, studyFeatureContent);
            return updatedStudyFeature != null
                    ? Ok(SingleSuccessResponse(new List<StudyFeature>() { updatedStudyFeature }))
                    : Ok(ErrorResponse("u", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }

    /****************************************************************
     * DELETE a single specified study feature 
     ****************************************************************/

    [HttpDelete("studies/{sd_sid}/features/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> DeleteStudyFeature(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExists(sd_sid, _entityType, id)) {
            var count = await _studyService.DeleteStudyFeature(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sd_sid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));      
    }
}