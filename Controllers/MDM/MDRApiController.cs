using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class MDRApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private readonly IUriService _uriService;
    private readonly string _attType, _fattType;
    
    /****************************************************************
    * This is a specialist controller
    * that is involved in obtaining full study details from the MDR
    * on receipt of a study registry id, and the id of the registry
    ****************************************************************/

    public MDRApiController(IStudyService studyService, IUriService uriService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        _attType = "study"; _fattType = "full study"; 
    }
      
    /****************************************************************
    * FETCH data for a single study (including attribute data)
    ****************************************************************/
    
    [HttpGet("mdr/studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> GetFullStudy(string sd_sid)
    {
        var fullStudy = await _studyService.GetFullStudyByIdAsync(sd_sid);
        return fullStudy != null
            ? Ok(SingleSuccessResponse(new List<FullStudy>() { fullStudy }))
            : Ok(NoEntityResponse(_fattType, sd_sid));
    }
    
    
    /****************************************************************
    * FETCH single study record (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("mdr/studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyData(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) {
            var study = await _studyService.GetStudyRecordDataAsync(sd_sid);
            return study != null
                ? Ok(SingleSuccessResponse(new List<StudyData>() { study }))
                : Ok(ErrorResponse("r", _attType, "", sd_sid, sd_sid));
        }
        return Ok(NoEntityResponse(_attType, sd_sid));
    }
    
   
    
}