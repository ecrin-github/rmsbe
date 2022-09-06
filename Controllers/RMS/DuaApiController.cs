using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DuaApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes;

    public DuaApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _parType = "DUP"; _parIdType = "id"; 
        _attType = "DUA"; _attTypes = "DUAs";
    }
    
    /****************************************************************
    * FETCH the DUA linked to a specified DUP
    ****************************************************************/
 
    [HttpGet("data-uses/{dupId:int}/dua")]
    [SwaggerOperation(Tags = new []{"DUP DUA endpoint"})]
    
    public async Task<IActionResult> GetDua(int dupId)
    {
        if (await _dupService.DupExists(dupId)) {
            var dua = await _dupService.GetDua(dupId);
            return dua != null
                ? Ok(SingleSuccessResponse(new List<Dua>() { dua }))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));    
    }

    /****************************************************************
    * CREATE a new DUA, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dupId:int}/dua")]
    [SwaggerOperation(Tags = new []{"DUP DUA endpoint"})]
    
    public async Task<IActionResult> CreateDua(int dupId, 
        [FromBody] Dua duaContent)
    {
        if (await _dupService.DupExists(dupId)) {
            duaContent.DupId = dupId;   // ensure this is the case
            var dua = await _dupService.CreateDua(duaContent);
            return dua != null
                ? Ok(SingleSuccessResponse(new List<Dua>() { dua }))
                : Ok(ErrorResponse("c", _attType, _parType, dupId.ToString(), dupId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));  
    }
    
    /****************************************************************
    * UPDATE the DUA, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dupId:int}/dua")]
    [SwaggerOperation(Tags = new []{"DUP DUA endpoint"})]
    
    public async Task<IActionResult> UpdateDua(int dupId,
        [FromBody] Dua duaContent)
    {
        if (await _dupService.DupDuaExists(dupId)) {   
            duaContent.DupId = dupId;  // ensure this is the case
            var updatedDua = await _dupService.UpdateDua(duaContent);
            return updatedDua != null
                ? Ok(SingleSuccessResponse(new List<Dua>() { updatedDua }))
                : Ok(ErrorResponse("u", _attType, _parType, dupId.ToString(), dupId.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), dupId.ToString()));
    }
    
    /****************************************************************
    * DELETE the DUA linked to a specified DUP
    ****************************************************************/

    [HttpDelete("data-uses/{dupId:int}/dua")]
    [SwaggerOperation(Tags = new []{"DUP DUA endpoint"})]
    
    public async Task<IActionResult> DeleteDua(int dupId)
    {
        if (await _dupService.DupDuaExists(dupId)) {    
            var count = await _dupService.DeleteDua(dupId);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dupId.ToString(), dupId.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dupId.ToString(), dupId.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), dupId.ToString()));
    }
}