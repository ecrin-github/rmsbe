using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyTitlesApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private OkObjectResult? _response;               // given a new value on each API call
    
    public StudyTitlesApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
    
    /****************************************************************
    * FETCH ALL titles for a specified study
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/titles")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> GetStudyTitles(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) 
        {
            var studyTitles = await _studyService.GetStudyTitlesAsync(sd_sid);
            _response = (studyTitles == null)
                ? Ok(NoAttributesFoundResponse<StudyTitle>("No study titles were found."))
                : Ok(CollectionSuccessResponse(studyTitles.Count, studyTitles));
        }
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyTitle>());
        return _response; 
    }
    
    /****************************************************************
    * FETCH A SINGLE study title 
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study titles endpoint" })]

    public async Task<IActionResult> GetStudyTitle(string sd_sid, int id)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            var studyTitle = await _studyService.GetStudyTitleAsync(id);
            _response = (studyTitle == null)
                ? Ok(NoAttributesResponse<StudyTitle>("No study title with that id found."))
                : Ok(SingleSuccessResponse(new List<StudyTitle>() { studyTitle }));
        }
        else
            _response = Ok(StudyDoesNotExistResponse<StudyTitle>());
        return _response;
    }

    /****************************************************************
     * CREATE a new title for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sd_sid}/titles")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> CreateStudyTitle(string sd_sid, 
                 [FromBody] StudyTitle studyTitleContent)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            studyTitleContent.SdSid = sd_sid;
            var newStudyTitle = await _studyService.CreateStudyTitleAsync(studyTitleContent);
            _response = (newStudyTitle == null)
                ? Ok(ErrorInActionResponse<StudyTitle>("Error during study title creation."))
                : Ok(SingleSuccessResponse(new List<StudyTitle>() { newStudyTitle }));
        }
        else
            _response = Ok(StudyDoesNotExistResponse<StudyTitle>());
        return _response;
    }
    
    /****************************************************************
     * UPDATE a single specified study title 
     ****************************************************************/
    
    [HttpPut("studies/{sd_sid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> UpdateStudyTitle(string sd_sid, int id, 
                 [FromBody] StudyTitle studyTitleContent)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyTitle", id)) 
        {
            var updatedStudyTitle = await _studyService.UpdateStudyTitleAsync(id, studyTitleContent);
            _response = (updatedStudyTitle == null)
                ? Ok(ErrorInActionResponse<StudyTitle>("Error during study title update."))
                : Ok(SingleSuccessResponse(new List<StudyTitle>() { updatedStudyTitle }));
        } 
        else 
            _response = Ok(MissingAttributeResponse<StudyTitle>("No title with that id found for this study."));
        return _response;  
    }
 
    /****************************************************************
     * DELETE a single specified study title 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/titles/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
    
    public async Task<IActionResult> DeleteStudyTitle(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyTitle", id)) 
        {
            var count = await _studyService.DeleteStudyTitleAsync(id);
            _response = (count == 0)
                ? Ok(ErrorInActionResponse<StudyTitle>("Deletion does not appear to have occured."))
                : Ok(DeletionSuccessResponse<StudyTitle>(count, $"Study title {id.ToString()} removed."));
        } 
        else
            _response = Ok(MissingAttributeResponse<StudyTitle>("No title with that id found for this study."));
        return _response;        
    }
}
