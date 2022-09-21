using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupStudiesApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DupStudiesApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _parType = "DUP"; _parIdType = "id"; _entityType = "DupStudy";
        _attType = "DUP study"; _attTypes = "DUP studies";
    }
    
    /****************************************************************
    * FETCH ALL studies linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dupId:int}/studies")]
    [SwaggerOperation(Tags = new []{"DUP studies endpoint"})]
    
    public async Task<IActionResult> GetDupStudyList(int dupId)
    {
        if (await _dupService.DupExists(dupId)) {
            var dupStudies = await _dupService.GetAllDupStudies(dupId);
            return dupStudies != null
                ? Ok(ListSuccessResponse(dupStudies.Count, dupStudies))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));    
    }

    /****************************************************************
    * FETCH ALL studies linked to a specified DUP, with foreign key names
    ****************************************************************/
   
    [HttpGet("data-uses/with-fk-names/{dupId:int}/studies")]
    [SwaggerOperation(Tags = new []{"DUP studies endpoint"})]
    
    public async Task<IActionResult> GetDupStudyListWfn(int dupId)
    {
        if (await _dupService.DupExists(dupId)) {
            var dupStudiesWfn = await _dupService.GetAllOutDupStudies(dupId);
            return dupStudiesWfn != null
                ? Ok(ListSuccessResponse(dupStudiesWfn.Count, dupStudiesWfn))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));    
    }
    
    /****************************************************************
    * FETCH a particular study, linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dupId:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP studies endpoint"})]
    
    public async Task<IActionResult> GetDupStudy(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var dupStudy = await _dupService.GetDupStudy(id);
            return dupStudy != null
                ? Ok(SingleSuccessResponse(new List<DupStudy>() { dupStudy }))
                : Ok(ErrorResponse("r", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * FETCH a particular study linked to a specified DUP, with foreign key names
    ****************************************************************/
    
    [HttpGet("data-uses/with-fk-names/{dupId:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP studies endpoint"})]
    
    public async Task<IActionResult> GetDupStudyWfn(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var dupStudyWfn = await _dupService.GetOutDupStudy(id);
            return dupStudyWfn != null
                ? Ok(SingleSuccessResponse(new List<DupStudyOut>() { dupStudyWfn }))
                : Ok(ErrorResponse("r", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new study, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dupId:int}/studies/{sdSid}")]
    [SwaggerOperation(Tags = new []{"DUP studies endpoint"})]
    
    public async Task<IActionResult> CreateDupStudy(int dupId, string sdSid,
                 [FromBody] DupStudy? dupStudyContent)
    {
        if (await _dupService.DupExists(dupId)) {
            if (dupStudyContent == null)
            {
                // a DupStudy object must be in the body, even if
                // it is an empty object...
                dupStudyContent = new DupStudy();
            }
            dupStudyContent.DupId = dupId;   // ensure this is the case
            dupStudyContent.SdSid = sdSid;
            var dupObj = await _dupService.CreateDupStudy(dupStudyContent);
            return dupObj != null
                ? Ok(SingleSuccessResponse(new List<DupStudy>() { dupObj }))
                : Ok(ErrorResponse("c", _attType, _parType, dupId.ToString(), dupId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));  
    }  

    /****************************************************************
    * UPDATE an study, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dupId:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP studies endpoint"})]
    
    public async Task<IActionResult> UpdateDupStudy(int dupId, int id, 
                 [FromBody] DupStudy dupStudyContent)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            dupStudyContent.DupId = dupId;  // ensure this is the case
            dupStudyContent.Id = id;
            var updatedDupStudy = await _dupService.UpdateDupStudy(dupStudyContent);
            return updatedDupStudy != null
                ? Ok(SingleSuccessResponse(new List<DupStudy>() { updatedDupStudy }))
                : Ok(ErrorResponse("u", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }

    /****************************************************************
    * DELETE a specified study, linked to a specified DUP
    ****************************************************************/
    
    [HttpDelete("data-uses/{dupId:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP studies endpoint"})]
    
    public async Task<IActionResult> DeleteDupStudy(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var count = await _dupService.DeleteDupStudy(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dupId.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
}