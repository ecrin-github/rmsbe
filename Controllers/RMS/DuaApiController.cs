using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DuaApiController : BaseApiController
{
    private readonly IDupService _dupService;

    public DuaApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
    }
    
    /****************************************************************
    * FETCH ALL DUAs linked to a specified DUP
    ****************************************************************/
 
    [HttpGet("data-uses/{dup_id:int}/accesses")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    
    public async Task<IActionResult> GetDuaList(int dup_id)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupObject>());
        }
        var duas = await _dupService.GetAllDuasAsync(dup_id);
        if (duas == null || duas.Count == 0)
        {
            return Ok(NoAttributesResponse<Dua>("No Duas were found."));
        }
        return Ok(new ApiResponse<Dua>()
        {
            Total = duas.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = duas
        });
    }

    /****************************************************************
    * FETCH a particular DUA linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    
    public async Task<IActionResult> GetDua(int dup_id, int id)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupObject>());
        }
        var dua = await _dupService.GetDuaAsync(id);
        if (dua == null) 
        {
            return Ok(NoAttributesResponse<Dua>("No DUA with that id found."));
        }        
        return Ok(new ApiResponse<Dua>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dua>() { dua }
        });
    }
    
    /****************************************************************
    * CREATE a new DUA, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dup_id:int}/accesses")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    
    public async Task<IActionResult> CreateDua(int dup_id, 
        [FromBody] Dua duaContent)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupObject>());
        }
        duaContent.DupId = dup_id;
        var dua = await _dupService.CreateDuaAsync(duaContent);
        if (dua == null) 
        {
            return Ok(ErrorInActionResponse<Dua>("Error during DUA creation."));
        }      
        return Ok(new ApiResponse<Dua>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dua>() { dua }
        });
    }
    
    /****************************************************************
    * UPDATE a DUA, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dup_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    
    public async Task<IActionResult> UpdateDua(int dup_id, int id, 
        [FromBody] Dua duaContent)
    {
        if (await _dupService.DupAttributeDoesNotExistAsync(dup_id, "DUA", id))
        {
            return Ok(ErrorInActionResponse<DupObject>("No object with that id found for specified DUP."));
        }
        var updatedDua = await _dupService.UpdateDuaAsync(id, duaContent);
        if (updatedDua == null)
        {
            return Ok(ErrorInActionResponse<Dua>("Error during DUA update."));
        }        
        return Ok(new ApiResponse<Dua>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dua>() { updatedDua }
        });
    }
    
    /****************************************************************
    * DELETE a specified DUA, linked to a specified DUP
    ****************************************************************/

    [HttpDelete("data-uses/{dup_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use access endpoint"})]
    
    public async Task<IActionResult> DeleteDua(int dup_id, int id)
    {
        if (await _dupService.DupAttributeDoesNotExistAsync(dup_id, "DUA", id))
        {
            return Ok(ErrorInActionResponse<DupObject>("No object with that id found for specified DUP."));
        }
        var count = await _dupService.DeleteDuaAsync(id);
        return Ok(new ApiResponse<Dua>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DUA has been removed."}, Data = null
        });
    }
}