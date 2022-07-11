using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class MdrApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private readonly IObjectService _objectService;
    private readonly string _attType, _fattType;
    
    /****************************************************************
    * This is a specialist controller
    * that is involved in obtaining full study details from the MDR
    * on receipt of a study registry id, and the id of the registry
    ****************************************************************/

    public MdrApiController(IStudyService studyService, IObjectService objectService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _attType = "study"; _fattType = "full study"; 
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
    
    
    /****************************************************************
    * FETCH and STORE data for a single study
    * (including attribute data) but not linked object data
    ****************************************************************/
    
    [HttpGet("studies/mdr/{regId:int}/{sdSid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> GetFullStudyFromMdr(int regId, string sdSid)
    {
        var fullStudyFromMdr = await _studyService.GetFullStudyFromMdr(regId, sdSid);
        return fullStudyFromMdr != null
            ? Ok(SingleSuccessResponse(new List<FullStudyFromMdr>() { fullStudyFromMdr }))
            : Ok(NoEntityResponse(_fattType, sdSid));
    }
    
    
    /****************************************************************
    * FETCH and STORE data for a single object
    * (including attribute data) - not yet object data
    ****************************************************************/
    
    [HttpGet("data-objects/mdr/{sdSid}/{mdrId:int}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetFullObjectFromMdr(string sdSid, int mdrId)
    {
        var fullObjectFromMdr = await _objectService.GetFullObjectFromMdr(sdSid, mdrId);
        return fullObjectFromMdr != null
            ? Ok(SingleSuccessResponse(new List<FullDataObject>() { fullObjectFromMdr }))
            : Ok(NoEntityResponse("full object", mdrId.ToString()));
    }
}