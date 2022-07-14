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
    
    [HttpGet("data-uses/{dup_id:int}/studies")]
    [SwaggerOperation(Tags = new []{"Data use process studies endpoint"})]
    
    public async Task<IActionResult> GetDupStudyList(int dup_id)
    {
        if (await _dupService.DupExists(dup_id)) {
            var dupStudies = await _dupService.GetAllDupStudies(dup_id);
            return dupStudies != null
                ? Ok(ListSuccessResponse(dupStudies.Count, dupStudies))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));    
    }

    /****************************************************************
    * FETCH a particular study, linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process studies endpoint"})]
    
    public async Task<IActionResult> GetDupStudy(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {
            var dupStudy = await _dupService.GetDupStudy(id);
            return dupStudy != null
                ? Ok(SingleSuccessResponse(new List<DupStudy>() { dupStudy }))
                : Ok(ErrorResponse("r", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new study, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dup_id:int}/studies/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data use process studies endpoint"})]
    
    public async Task<IActionResult> CreateDupStudy(int dup_id, string sd_oid,
                 [FromBody] DupStudy dupStudyContent)
    {
        if (await _dupService.DupExists(dup_id)) {
            dupStudyContent.DupId = dup_id;
            dupStudyContent.StudyId = sd_oid;
            var dupObj = await _dupService.CreateDupStudy(dupStudyContent);
            return dupObj != null
                ? Ok(SingleSuccessResponse(new List<DupStudy>() { dupObj }))
                : Ok(ErrorResponse("c", _attType, _parType, dup_id.ToString(), dup_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));  
    }  

    /****************************************************************
    * UPDATE an study, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dup_id:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process studies endpoint"})]
    
    public async Task<IActionResult> UpdateDupStudy(int dup_id, int id, 
                 [FromBody] DupStudy dupStudyContent)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {
            var updatedDupStudy = await _dupService.UpdateDupStudy(dup_id, dupStudyContent);
            return updatedDupStudy != null
                ? Ok(SingleSuccessResponse(new List<DupStudy>() { updatedDupStudy }))
                : Ok(ErrorResponse("u", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }

    /****************************************************************
    * DELETE a specified study, linked to a specified DUP
    ****************************************************************/
    
    [HttpDelete("data-uses/{dup_id:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process studies endpoint"})]
    
    public async Task<IActionResult> DeleteDupStudy(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {
            var count = await _dupService.DeleteDupStudy(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dup_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
}