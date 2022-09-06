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
    
    [HttpGet("data-uses/{dupId:int}/secondary-use")]
    [SwaggerOperation(Tags = new []{"DUP Secondary use endpoint"})]
    
    public async Task<IActionResult> GetSecondaryUseList(int dupId)
    {
        if (await _dupService.DupExists(dupId)) {
            var secUses = await _dupService.GetAllSecUses(dupId);
            return secUses != null
                ? Ok(ListSuccessResponse(secUses.Count, secUses))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));    
    }

    /****************************************************************
    * FETCH a particular Secondary use linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dupId:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP Secondary use endpoint"})]
    
    public async Task<IActionResult> GetSecondaryUse(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var secUse = await _dupService.GetSecUse(id);
            return secUse != null
                ? Ok(SingleSuccessResponse(new List<DupSecondaryUse>() { secUse }))
                : Ok(ErrorResponse("r", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new Secondary use, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dupId:int}/secondary-use")]
    [SwaggerOperation(Tags = new []{"DUP Secondary use endpoint"})]
    
    public async Task<IActionResult> CreateSecondaryUse(int dupId, 
           [FromBody] DupSecondaryUse secondaryUseContent)
    {
        if (await _dupService.DupExists(dupId)) {
            secondaryUseContent.DupId = dupId;   // ensure this is the case
            var secUse = await _dupService.CreateSecUse(secondaryUseContent);
            return secUse != null
                ? Ok(SingleSuccessResponse(new List<DupSecondaryUse>() { secUse }))
                : Ok(ErrorResponse("c", _attType, _parType, dupId.ToString(), dupId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));  
    }  
    
    /****************************************************************
    * UPDATE a Secondary use record, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dupId:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP Secondary use endpoint"})]
    
    public async Task<IActionResult> UpdateSecondaryUse(int dupId, int id, 
           [FromBody] DupSecondaryUse secondaryUseContent)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            secondaryUseContent.DupId = dupId;  // ensure this is the case
            secondaryUseContent.Id = id;
            var updatedSecUse = await _dupService.UpdateSecUse(secondaryUseContent);
            return updatedSecUse != null
                ? Ok(SingleSuccessResponse(new List<DupSecondaryUse>() { updatedSecUse }))
                : Ok(ErrorResponse("u", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified Secondary use, linked to a specified DUP
    ****************************************************************/
    
    [HttpDelete("data-uses/{dupId:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP Secondary use endpoint"})]
    
    public async Task<IActionResult> DeleteSecondaryUse(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var count = await _dupService.DeleteSecUse(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dupId.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
}