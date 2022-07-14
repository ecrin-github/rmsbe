using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DuaApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DuaApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _parType = "DUP"; _parIdType = "id"; _entityType = "Dua";
        _attType = "DUA"; _attTypes = "DUAs";
    }
    
    /****************************************************************
    * FETCH ALL DUAs linked to a specified DUP
    ****************************************************************/
 
    [HttpGet("data-uses/{dup_id:int}/accesses")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    
    public async Task<IActionResult> GetDuaList(int dup_id)
    {
        if (await _dupService.DupExists(dup_id)) {
            var duas = await _dupService.GetAllDuas(dup_id);
            return duas != null
                ? Ok(ListSuccessResponse(duas.Count, duas))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));    
    }

    /****************************************************************
    * FETCH a particular DUA linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    
    public async Task<IActionResult> GetDua(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {    
            var dua = await _dupService.GetDua(id);
            return dua != null
                ? Ok(SingleSuccessResponse(new List<Dua>() { dua }))
                : Ok(ErrorResponse("r", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new DUA, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dup_id:int}/accesses")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    
    public async Task<IActionResult> CreateDua(int dup_id, 
        [FromBody] Dua duaContent)
    {
        if (await _dupService.DupExists(dup_id)) {
            duaContent.DupId = dup_id;
            var dua = await _dupService.CreateDua(duaContent);
            return dua != null
                ? Ok(SingleSuccessResponse(new List<Dua>() { dua }))
                : Ok(ErrorResponse("c", _attType, _parType, dup_id.ToString(), dup_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));  
    }
    
    /****************************************************************
    * UPDATE a DUA, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dup_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    
    public async Task<IActionResult> UpdateDua(int dup_id, int id, 
        [FromBody] Dua duaContent)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {    
            var updatedDua = await _dupService.UpdateDua(id, duaContent);
            return updatedDua != null
                ? Ok(SingleSuccessResponse(new List<Dua>() { updatedDua }))
                : Ok(ErrorResponse("u", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified DUA, linked to a specified DUP
    ****************************************************************/

    [HttpDelete("data-uses/{dup_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    
    public async Task<IActionResult> DeleteDua(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {    
            var count = await _dupService.DeleteDua(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dup_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
}