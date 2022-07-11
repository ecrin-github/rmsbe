using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class MdrApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private readonly IUriService _uriService;
    private readonly string _attType, _fattType;
    
    /****************************************************************
    * This is a specialist controller
    * that is involved in obtaining full study details from the MDR
    * on receipt of a study registry id, and the id of the registry
    ****************************************************************/

    public MdrApiController(IStudyService studyService, IUriService uriService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        _attType = "study"; _fattType = "full study"; 
    }
      
    /****************************************************************
    * FETCH and STORE data for a single study
    * (including attribute data) - not yet object data
    ****************************************************************/
    
    [HttpGet("studies/mdr/{regId:int}/{sdSid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> GetFullStudyFromMdr(int regId, string sdSid)
    {
        var fullStudy = await _studyService.GetFullStudyFromMdr(regId, sdSid);
        return fullStudy != null
            ? Ok(SingleSuccessResponse(new List<FullStudy>() { fullStudy }))
            : Ok(NoEntityResponse(_fattType, sdSid));
    }
    
    
    /****************************************************************
    * FETCH single study record (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/mdr/{regId:int}/{sdSid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyDataFromMdr(int regId, string sdSid)
    {
        var study = await _studyService.GetStudyFromMdr(regId, sdSid);
        return study != null
            ? Ok(SingleSuccessResponse(new List<StudyData>() { study }))
            : Ok(ErrorResponse("r", _attType, "", sdSid, sdSid));
    }
    
}