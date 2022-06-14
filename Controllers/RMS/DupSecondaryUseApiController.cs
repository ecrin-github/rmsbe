using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class SecondaryUseApiController : BaseApiController
{
    private readonly IRmsUseService _rmsService;

    public SecondaryUseApiController(IRmsUseService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
    
    /****************************************************************
    * FETCH ALL Secondary uses linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/secondary-use")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    
    public async Task<IActionResult> GetSecondaryUseList(int dup_id)
    {
        if (await _rmsService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDtpResponse<SecondaryUse>());
        }
        var secUses = await _rmsService.GetAllSecondaryUsesAsync(dup_id);
        if (secUses == null || secUses.Count == 0)
        {
            return Ok(NoAttributesResponse<SecondaryUse>("No SecondaryUses were found."));
        }
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = secUses.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = secUses
        });
    }

    /****************************************************************
    * FETCH a particular Secondary use linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    
    public async Task<IActionResult> GetSecondaryUse(int dup_id, int id)
    {
        if (await _rmsService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<SecondaryUse>());
        }
        var secUse = await _rmsService.GetSecondaryUseAsync(id);
        if (secUse == null) 
        {
            return Ok(NoAttributesResponse<SecondaryUse>("No Secondary use with that id found."));
        }        
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<SecondaryUse>() { secUse }
        });
    }
    
    /****************************************************************
    * CREATE a new Secondary use, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dup_id:int}/secondary-use")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    
    public async Task<IActionResult> CreateSecondaryUse(int dup_id, 
           [FromBody] SecondaryUse secondaryUseContent)
    {
        if (await _rmsService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<SecondaryUse>());
        }
        secondaryUseContent.DupId = dup_id;
        var secUse = await _rmsService.CreateSecondaryUseAsync(secondaryUseContent);
        if (secUse == null)
        {
            return Ok(ErrorInActionResponse<SecondaryUse>("Error during Secondary use creation."));
        }       
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<SecondaryUse>() { secUse }
        });
    }
    
    /****************************************************************
    * UPDATE a Secondary use record, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dup_id:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    
    public async Task<IActionResult> UpdateSecondaryUse(int dup_id, int id, 
           [FromBody] SecondaryUse secondaryUseContent)
    {
        if (await _rmsService.DupAttributeDoesNotExistAsync(dup_id, "SecondaryUse", id))
        {
            return Ok(ErrorInActionResponse<SecondaryUse>("No secondary use with that id found for specified DUP."));
        }
        var updateSecUse = await _rmsService.UpdateSecondaryUseAsync(dup_id, secondaryUseContent);
        if (updateSecUse == null)
        {
            return Ok(ErrorInActionResponse<SecondaryUse>("Error during Secondary use update."));
        }          
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<SecondaryUse>() { updateSecUse }
        });
    }
    
    /****************************************************************
    * DELETE a specified Secondary use, linked to a specified DUP
    ****************************************************************/
    
    [HttpDelete("data-uses/{dup_id:int}/secondary-use/{id:int}")]
    [SwaggerOperation(Tags = new []{"Secondary use endpoint"})]
    
    public async Task<IActionResult> DeleteSecondaryUse(int dup_id, int id)
    {
        if (await _rmsService.DupAttributeDoesNotExistAsync(dup_id, "SecondaryUse", id))
        {
            return Ok(ErrorInActionResponse<SecondaryUse>("No secondary use with that id found for specified DUP."));
        }
        var count = await _rmsService.DeleteSecondaryUseAsync(id);
        return Ok(new ApiResponse<SecondaryUse>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Secondary use has been removed." }, Data = null
        });
    }
}