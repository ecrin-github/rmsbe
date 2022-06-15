using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyContributorsApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private OkObjectResult? _response;               // given a new value on each API call

    public StudyContributorsApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }

    /****************************************************************
    * FETCH ALL contributors for a specified study
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/contributors")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> GetStudyContributors(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) 
        {
            var studyContribs = await _studyService.GetStudyContributorsAsync(sd_sid);
            _response = (studyContribs == null)
                ? Ok(NoAttributesFoundResponse<StudyContributor>("No study contributors were found."))
                : Ok(CollectionSuccessResponse(studyContribs.Count, studyContribs));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyContributor>());
        return _response;
    }

    /****************************************************************
    * FETCH A SINGLE study contributor 
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> GetStudyContributor(string sd_sid, int id)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            var studyContributor = await _studyService.GetStudyContributorAsync(id);
            _response = (studyContributor == null)
                ? Ok(NoAttributesResponse<StudyContributor>("No study contributor with that id found."))
                : Ok(SingleSuccessResponse(new List<StudyContributor>() { studyContributor }));
        }
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyContributor>());
        return _response; 
    }

    /****************************************************************
     * CREATE a new contributor for a specified study
     ****************************************************************/

    [HttpPost("studies/{sd_sid}/contributors")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> CreateStudyContributor(string sd_sid, 
                 [FromBody] StudyContributor studyContContent)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            studyContContent.SdSid = sd_sid;
            var newStudyContrib = await _studyService.CreateStudyContributorAsync(studyContContent);
            _response = (newStudyContrib == null)
                        ? Ok(ErrorInActionResponse<StudyContributor>("Error during study contributor creation."))
                        : Ok(SingleSuccessResponse(new List<StudyContributor>() { newStudyContrib }));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyContributor>());
        return _response;     
    }

    /****************************************************************
     * UPDATE a single specified study contributor 
     ****************************************************************/

    [HttpPut("studies/{sd_sid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> UpdateStudyContributor(string sd_sid, int id, 
                 [FromBody] StudyContributor studyContContent)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyContributor", id)) 
        {
            var updatedStudyContributor = await _studyService.UpdateStudyContributorAsync(id, studyContContent);
            _response = (updatedStudyContributor == null)
                ? Ok(ErrorInActionResponse<StudyContributor>("Error during study contributor update."))
                : Ok(SingleSuccessResponse(new List<StudyContributor>() { updatedStudyContributor }));
        } 
        else 
            _response = Ok(MissingAttributeResponse<StudyContributor>("No contributor with that id found for this study."));
        return _response;  
    }

    /****************************************************************
     * DELETE a single specified study contributor 
     ****************************************************************/

    [HttpDelete("studies/{sd_sid}/contributors/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study contributors endpoint"})]
    
    public async Task<IActionResult> DeleteStudyContributor(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyContributor", id)) 
        {
            var count = await _studyService.DeleteStudyContributorAsync(id);
            _response = (count == 0)
                ? Ok(ErrorInActionResponse<StudyContributor>("Deletion does not appear to have occured."))
                : Ok(DeletionSuccessResponse<StudyContributor>(count, $"Study contributor {id.ToString()} removed."));
        } 
        else
            _response = Ok(MissingAttributeResponse<StudyContributor>("No contributor with that id found for this study."));
        return _response;    
    }
}
