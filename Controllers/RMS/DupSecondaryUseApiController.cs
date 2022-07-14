using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class SecondaryUseApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public SecondaryUseApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _parType = "DUP"; _parIdType = "id"; _entityType = "SecondaryUse";
        _attType = "secondary use"; _attTypes = "secondary uses";
    }
    
    /****************************************************************
    * FETCH ALL Secondary uses linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/secondary-use")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    
    public async Task<IActionResult> GetSecondaryUseList(int dup_id)
    {
        if (await _dupService.DupExists(dup_id)) {
            var secUses = await _dupService.GetAllSecUses(dup_id);
            return secUses != null
                ? Ok(ListSuccessResponse(secUses.Count, secUses))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));    
    }

    /****************************************************************
    * FETCH a particular Secondary use linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    
    public async Task<IActionResult> GetSecondaryUse(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {
            var secUse = await _dupService.GetSecUse(id);
            return secUse != null
                ? Ok(SingleSuccessResponse(new List<SecondaryUse>() { secUse }))
                : Ok(ErrorResponse("r", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new Secondary use, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dup_id:int}/secondary-use")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    
    public async Task<IActionResult> CreateSecondaryUse(int dup_id, 
           [FromBody] SecondaryUse secondaryUseContent)
    {
        if (await _dupService.DupExists(dup_id)) {
            secondaryUseContent.DupId = dup_id;
            var secUse = await _dupService.CreateSecUse(secondaryUseContent);
            return secUse != null
                ? Ok(SingleSuccessResponse(new List<SecondaryUse>() { secUse }))
                : Ok(ErrorResponse("c", _attType, _parType, dup_id.ToString(), dup_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));  
    }  
    
    /****************************************************************
    * UPDATE a Secondary use record, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dup_id:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    
    public async Task<IActionResult> UpdateSecondaryUse(int dup_id, int id, 
           [FromBody] SecondaryUse secondaryUseContent)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {
            var updateSecUse = await _dupService.UpdateSecUse(dup_id, secondaryUseContent);
            return updateSecUse != null
                ? Ok(SingleSuccessResponse(new List<SecondaryUse>() { updateSecUse }))
                : Ok(ErrorResponse("u", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified Secondary use, linked to a specified DUP
    ****************************************************************/
    
    [HttpDelete("data-uses/{dup_id:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    
    public async Task<IActionResult> DeleteSecondaryUse(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {
            var count = await _dupService.DeleteSecUse(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dup_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
}