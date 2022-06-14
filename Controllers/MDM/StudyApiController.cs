using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyApiController : BaseApiController
{
    private readonly IStudyService _studyService;

    public StudyApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
      
     
    /****************************************************************
    * FETCH data for a single study (including attribute data)
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> GetStudyById(string sd_sid)
    {
        var fullStudy = await _studyService.GetFullStudyByIdAsync(sd_sid);
        if (fullStudy == null) 
        {
            return Ok(NoAttributesResponse<FullStudy>("No study date found with that id."));
        }
        return Ok(new ApiResponse<FullStudy>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<FullStudy>() { fullStudy }
        });
    }
    
    /****************************************************************
    * DELETE an entire study record (with attributes)
    ****************************************************************/

    [HttpDelete("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> DeleteStudy(string sd_sid)
    {
        if (await _studyService.StudyDoesNotExistAsync(sd_sid))
        {
            return Ok(NoStudyResponse<FullStudy>);
        }
        var count = await _studyService.DeleteFullStudyAsync(sd_sid);
        return Ok(new ApiResponse<FullStudy>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Full study data has been removed." }, Data = null
        });
    }
}