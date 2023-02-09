using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class StudyContributorsApiController : BaseBrowsingApiController
{
    private readonly IStudyService _studyService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public StudyContributorsApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _parType = "study"; _parIdType = "sd_sid"; _entityType = "StudyContributor";
        _attType = "study contributor"; _attTypes = "study contributors";
    }

    /****************************************************************
    * FETCH ALL contributors for a specified study
    ****************************************************************/

    [HttpGet("studies/{sdSid}/contributors")]
    [SwaggerOperation(Tags = new []{"Browsing - Study contributors endpoint"})]
    
    public async Task<IActionResult> GetStudyContributors(string sdSid)
    {
        if (await _studyService.StudyExists(sdSid)) {
            var studyContribs = await _studyService.GetStudyContributors(sdSid);
            return studyContribs != null
                ? Ok(ListSuccessResponse(studyContribs.Count, studyContribs))
                : Ok(NoAttributesResponse(_attTypes));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));
    }

    /****************************************************************
    * FETCH A SINGLE study contributor 
    ****************************************************************/

    [HttpGet("studies/{sdSid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Browsing - Study contributors endpoint"})]
    
    public async Task<IActionResult> GetStudyContributor(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var studyContributor = await _studyService.GetStudyContributor(id);
            return studyContributor != null
                ? Ok(SingleSuccessResponse(new List<StudyContributor>() { studyContributor }))
                : Ok(ErrorResponse("r", _attType, _parType, sdSid, sdSid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }
}