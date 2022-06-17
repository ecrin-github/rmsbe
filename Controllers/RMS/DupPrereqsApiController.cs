using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupPrereqsApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public DupPrereqsApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _parType = "Object"; _parIdType = "sd_oid"; _entityType = "DupPrereq";
        _attType = "object prerequisite"; _attTypes = "object prerequisites"; 
    }
    
    /****************************************************************
    * FETCH ALL pre-requisites linked to a specified DUP / Object
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    
    public async Task<IActionResult> GetDupPrereqList(int dup_id, string sd_oid)
    {
        if (await _dupService.DupObjectExistsAsync(dup_id, sd_oid)) {
            var dupPrereqs = await _dupService.GetAllDupPrereqsAsync(dup_id, sd_oid);
            return dupPrereqs != null    
                ? Ok(ListSuccessResponse(dupPrereqs.Count, dupPrereqs))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));    
    }
    
    /****************************************************************
    * FETCH a specific pre-requisite met record, on a specified DUP / Object
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    
    public async Task<IActionResult> GetDupPrereq(int dup_id, string sd_oid, int id)
    {
        if (await _dupService.DupObjectAttributeExistsAsync (dup_id, sd_oid, _entityType, id)) {
            var dupPrereq = await _dupService.GetDupPrereqAsync(id);
            return dupPrereq != null
                ? Ok(SingleSuccessResponse(new List<DupPrereq>() { dupPrereq }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }

    /****************************************************************
    * CREATE a pre-requisite record for a specified DUP / Object
    ****************************************************************/
    
    [HttpPost("data-uses/{dup_id:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    
    public async Task<IActionResult> CreateDupPrereq(int dup_id, string sd_oid, 
        [FromBody] DupPrereq dupPrereqContent)
    {
        if (await _dupService.DupObjectExistsAsync(dup_id, sd_oid)) {
            dupPrereqContent.DupId = dup_id;
            dupPrereqContent.SdOid = sd_oid;
            var dupPrereq = await _dupService.CreateDupPrereqAsync(dupPrereqContent);
            return dupPrereq != null
                ? Ok(SingleSuccessResponse(new List<DupPrereq>() { dupPrereq }))
                : Ok(ErrorResponse("c", _attType, _parType, dup_id.ToString(), dup_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
    
    /****************************************************************
    * UPDATE a pre-requisite met record, for a specified DUP / Object
    ****************************************************************/

    [HttpPut("data-uses/{dup_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    
    public async Task<IActionResult> UpdateDupPrereq(int dup_id, string sd_oid, int id, 
        [FromBody] DupPrereq dupPrereqContent)
    {
        if (await _dupService.DupObjectAttributeExistsAsync (dup_id, sd_oid, _entityType, id)) {
            var updatedDupPrereq = await _dupService.UpdateDupPrereqAsync(id, dupPrereqContent);
            return updatedDupPrereq != null
                ? Ok(SingleSuccessResponse(new List<DupPrereq>() { updatedDupPrereq }))
                : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a pre-requisite met record, for a specified DUP / Object
    ****************************************************************/
    
    [HttpDelete("data-uses/{dup_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    
    public async Task<IActionResult> DeleteDupPrereq(int dup_id, string sd_oid, int id)
    {
        if (await _dupService.DupObjectAttributeExistsAsync (dup_id, sd_oid, _entityType, id)) {
            var count = await _dupService.DeleteDupPrereqAsync(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}