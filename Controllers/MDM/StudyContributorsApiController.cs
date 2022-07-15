using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyContributorsApiController : BaseApiController
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
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
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
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
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

    /****************************************************************
     * CREATE a new contributor for a specified study
     ****************************************************************/

    [HttpPost("studies/{sdSid}/contributors")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> CreateStudyContributor(string sdSid, 
                 [FromBody] StudyContributor studyContContent)
    {
        if (await _studyService.StudyExists(sdSid)) {
            studyContContent.SdSid = sdSid;
            var newStudyContrib = await _studyService.CreateStudyContributor(studyContContent);
            return newStudyContrib != null
                ? Ok(SingleSuccessResponse(new List<StudyContributor>() { newStudyContrib }))
                : Ok(ErrorResponse("c", _attType, _parType, sdSid, sdSid));
        } 
        return Ok(NoParentResponse(_parType, _parIdType, sdSid));  
    }

    /****************************************************************
     * UPDATE a single specified study contributor 
     ****************************************************************/

    [HttpPut("studies/{sdSid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> UpdateStudyContributor(string sdSid, int id, 
                 [FromBody] StudyContributor studyContContent)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var updatedStudyContributor = await _studyService.UpdateStudyContributor(id, studyContContent);
            return updatedStudyContributor != null
                ? Ok(SingleSuccessResponse(new List<StudyContributor>() { updatedStudyContributor }))
                : Ok(ErrorResponse("u", _attType, _parType, sdSid, id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }

    /****************************************************************
     * DELETE a single specified study contributor 
     ****************************************************************/

    [HttpDelete("studies/{sdSid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study contributors endpoint" })]

    public async Task<IActionResult> DeleteStudyContributor(string sdSid, int id)
    {
        if (await _studyService.StudyAttributeExists(sdSid, _entityType, id)) {
            var count = await _studyService.DeleteStudyContributor(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdSid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdSid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdSid, id.ToString()));
    }
}