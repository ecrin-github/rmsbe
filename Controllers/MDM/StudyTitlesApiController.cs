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
    
    [HttpGet("studies/{sd_sid}/titles")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> GetStudyTitles(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            var studyTitles = await _studyService.GetStudyTitlesAsync(sd_sid);
            return studyTitles != null
                    ? Ok(ListSuccessResponse(studyTitles.Count, studyTitles))
                    : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }
    
    /****************************************************************
    * FETCH A SINGLE study title 
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study titles endpoint" })]

    public async Task<IActionResult> GetStudyTitle(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, _entityType, id)) {
            var studyTitle = await _studyService.GetStudyTitleAsync(id);
            return studyTitle != null
                    ? Ok(SingleSuccessResponse(new List<StudyTitle>() { studyTitle }))
                    : Ok(ErrorResponse("r", _attType, _parType, sd_sid, sd_sid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }

    /****************************************************************
     * CREATE a new title for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sd_sid}/titles")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> CreateStudyTitle(string sd_sid, 
                 [FromBody] StudyTitle studyTitleContent)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            studyTitleContent.SdSid = sd_sid;
            var newStudyTitle = await _studyService.CreateStudyTitleAsync(studyTitleContent);
            return newStudyTitle != null
                    ? Ok(SingleSuccessResponse(new List<StudyTitle>() { newStudyTitle }))
                    : Ok(ErrorResponse("c", _attType, _parType, sd_sid, sd_sid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_sid));
    }
    
    /****************************************************************
     * UPDATE a single specified study title 
     ****************************************************************/
    
    [HttpPut("studies/{sd_sid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> UpdateStudyTitle(string sd_sid, int id, 
                 [FromBody] StudyTitle studyTitleContent)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, _entityType, id)) {
            var updatedStudyTitle = await _studyService.UpdateStudyTitleAsync(id, studyTitleContent);
            return updatedStudyTitle != null
                    ? Ok(SingleSuccessResponse(new List<StudyTitle>() { updatedStudyTitle }))
                    : Ok(ErrorResponse("u", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));
    }
 
    /****************************************************************
     * DELETE a single specified study title 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> DeleteStudyTitle(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, _entityType, id)) {
            var count = await _studyService.DeleteStudyTitleAsync(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, sd_sid, id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, sd_sid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sd_sid, id.ToString()));   
    }
}
