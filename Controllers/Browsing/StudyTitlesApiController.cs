using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class StudyTitlesApiController : BaseBrowsingApiController
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
    [SwaggerOperation(Tags = new []{"Browsing - Study titles endpoint"})]
    
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
    [SwaggerOperation(Tags = new[] { "Browsing - Study titles endpoint" })]

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
}
