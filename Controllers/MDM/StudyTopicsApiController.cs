using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyTopicsApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private OkObjectResult? _response;               // given a new value on each API call

    public StudyTopicsApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
    
    /****************************************************************
     * FETCH ALL topics for a specified study
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/topics")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> GetStudyTopics(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) 
        {
            var studyTopics = await _studyService.GetStudyTopicsAsync(sd_sid);
            _response = (studyTopics == null)
                    ? Ok(NoAttributesFoundResponse<StudyTopic>("No study topics were found."))
                    : Ok(CollectionSuccessResponse(studyTopics.Count, studyTopics));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyTopic>());
        return _response;
    }
    
    /****************************************************************
     * FETCH A SINGLE study topic 
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> GetStudyTopic(string sd_sid, int id)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) 
        {
            var studyTopic = await _studyService.GetStudyTopicAsync(id);
            _response = (studyTopic == null)
                ? Ok(NoAttributesResponse<StudyTopic>("No study topic with that id found."))
                : Ok(SingleSuccessResponse(new List<StudyTopic>() { studyTopic }));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyTopic>());
        return _response;
    }
   
    /****************************************************************
     * CREATE a new topic for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sd_sid}/topics")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> CreateStudyTopic(string sd_sid, 
                 [FromBody] StudyTopic studyTopicContent)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            studyTopicContent.SdSid = sd_sid;
            var newStudyTopic = await _studyService.CreateStudyTopicAsync(studyTopicContent);
            _response = (newStudyTopic == null)
                ? Ok(ErrorInActionResponse<StudyTopic>("Error during study topic creation."))
                : Ok(SingleSuccessResponse(new List<StudyTopic>() { newStudyTopic }));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyTopic>());
        return _response;  
    }
 
    /****************************************************************
     * UPDATE a single specified study topic 
     ****************************************************************/
    
    [HttpPut("studies/{sd_sid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> UpdateStudyTopic(string sd_sid, int id, 
                 [FromBody] StudyTopic studyTopicContent)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyTopic", id)) 
        {
            var updatedStudyTopic = await _studyService.UpdateStudyTopicAsync(id, studyTopicContent);
            _response = (updatedStudyTopic == null)
                ? Ok(ErrorInActionResponse<StudyTopic>("Error during study topic update."))
                : Ok(SingleSuccessResponse(new List<StudyTopic>() { updatedStudyTopic }));
        } 
        else 
            _response = Ok(MissingAttributeResponse<StudyTopic>("No topic with that id found for this study."));
        return _response;  
    }
    
    /****************************************************************
     * DELETE a single specified study topic 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/topics/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
    
    public async Task<IActionResult> DeleteStudyTopic(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyTopic", id)) 
        {
            var count = await _studyService.DeleteStudyTopicAsync(id);
            _response = (count == 0)
                ? Ok(ErrorInActionResponse<StudyTopic>("Deletion does not appear to have occured."))
                : Ok(DeletionSuccessResponse<StudyTopic>(count, $"Study topic {id.ToString()} removed."));
        } 
        else
            _response = Ok(MissingAttributeResponse<StudyTopic>("No topic with that id found for this study."));
        return _response;        
    }
}
