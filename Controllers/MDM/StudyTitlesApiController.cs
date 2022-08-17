using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyTitlesApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public StudyTitlesApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _parType = "study"; _parIdType = "sd_sid"; _entityType = "StudyTitle";
        _attType = "study title"; _attTypes = "study titles";
    }
    
    /****************************************************************
    * FETCH ALL titles for a specified study
    ****************************************************************/
    
    [HttpGet("studies/{sdSid}/titles")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> GetStudyTitles(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var studyTitles = await _studyService.GetStudyTitles(sdSid);
            return studyTitles != null
                    ? Ok(ListSuccessResponse(studyTitles.Count, studyTitles))
                    : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }
    
    /****************************************************************
    * FETCH A SINGLE study title 
    ****************************************************************/

    [HttpGet("studies/{sdSid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study titles endpoint" })]

    public async Task<IActionResult> GetStudyTitle(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var studyTitle = await _studyService.GetStudyTitle(id);
            return studyTitle != null
                    ? Ok(SingleSuccessResponse(new List<StudyTitle>() { studyTitle }))
                    : Ok(ErrorResponse("r", _attType, _parType, sdSid, sdSid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }

    /****************************************************************
     * CREATE a new title for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sdSid}/titles")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> CreateStudyTitle(string sdSid, 
                 [FromBody] StudyTitle studyTitleContent)
    {
        if (await _studyService.StudyExists(sdSid)) {
            studyTitleContent.SdSid = sdSid;   // ensure this is the case
            var newStudyTitle = await _studyService.CreateStudyTitle(studyTitleContent);
            return newStudyTitle != null
                    ? Ok(SingleSuccessResponse(new List<StudyTitle>() { newStudyTitle }))
                    : Ok(ErrorResponse("c", _attType, _parType, sdSid, sdSid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }
    
    /****************************************************************
     * UPDATE a single specified study title 
     ****************************************************************/
    
    [HttpPut("studies/{sdSid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> UpdateStudyTitle(string sdSid, int id, 
                 [FromBody] StudyTitle studyTitleContent)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            studyTitleContent.SdSid = sdSid;  // ensure this is the case
            studyTitleContent.Id = id;
            var updatedStudyTitle = await _studyService.UpdateStudyTitle(studyTitleContent);
            return updatedStudyTitle != null
                    ? Ok(SingleSuccessResponse(new List<StudyTitle>() { updatedStudyTitle }))
                    : Ok(ErrorResponse("u", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }
 
    /****************************************************************
     * DELETE a single specified study title 
     ****************************************************************/
    
    [HttpDelete("studies/{sdSid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> DeleteStudyTitle(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var count = await _studyService.DeleteStudyTitle(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sdSid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));   
    }
}
