using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyTopicsApiController : BaseApiController
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
    
    [HttpGet("studies/{sd_sid}/topics")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> GetStudyTopics(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            var studyTopics = await _studyService.GetStudyTopicsAsync(sd_sid);
            return studyTopics != null
                    ? Ok(ListSuccessResponse(studyTopics.Count, studyTopics))
                    : Ok(NoAttributesResponse(_attTypes));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }
    
    /****************************************************************
     * FETCH A SINGLE study topic 
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> GetStudyTopic(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, _entityType, id)) {
            var studyTopic = await _studyService.GetStudyTopicAsync(id);
            return studyTopic != null
                    ? Ok(SingleSuccessResponse(new List<StudyTopic>() { studyTopic }))
                    : Ok(ErrorResponse("r", _attType, _parType, sd_sid, sd_sid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }
   
    /****************************************************************
     * CREATE a new topic for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sd_sid}/topics")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> CreateStudyTopic(string sd_sid, 
                 [FromBody] StudyTopic studyTopicContent)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            studyTopicContent.SdSid = sd_sid;
            var newStudyTopic = await _studyService.CreateStudyTopicAsync(studyTopicContent);
            return newStudyTopic != null
                    ? Ok(SingleSuccessResponse(new List<StudyTopic>(){ newStudyTopic }))
                    : Ok(ErrorResponse("c", _attType, _parType, sd_sid, sd_sid));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }
 
    /****************************************************************
     * UPDATE a single specified study topic 
     ****************************************************************/
    
    [HttpPut("studies/{sd_sid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> UpdateStudyTopic(string sd_sid, int id, 
                 [FromBody] StudyTopic studyTopicContent)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, _entityType, id)) {
            var updatedStudyTopic = await _studyService.UpdateStudyTopicAsync(id, studyTopicContent);
            return updatedStudyTopic != null
                    ? Ok(SingleSuccessResponse(new List<StudyTopic>() { updatedStudyTopic }))
                    : Ok(ErrorResponse("u", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }
    
    /****************************************************************
     * DELETE a single specified study topic 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> DeleteStudyTopic(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, _entityType, id)) {
            var count = await _studyService.DeleteStudyTopicAsync(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sd_sid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));     
    }
}
