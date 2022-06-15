using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupPrereqsApiController : BaseApiController
{
    private readonly IDupService _dupService;

    public DupPrereqsApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
    }
    
    /****************************************************************
    * FETCH ALL pre-requisites linked to a specified DUP / Object
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    
    public async Task<IActionResult> GetDupPrereqList(int dup_id, string sd_oid)
    {
        if (await _dupService.DupObjectDoesNotExistAsync(dup_id, sd_oid))
        {
            return Ok(ErrorInActionResponse<DupPrereq>("No object with that id found for specified DUP."));
        }
        var dupPrereqs = await _dupService.GetAllDupPrereqsAsync(dup_id, sd_oid);
        if (dupPrereqs == null || dupPrereqs.Count == 0)
        {
            return Ok(NoAttributesResponse<DupPrereq>("No pre-requisites were found for the specified object / DUP."));
        }
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = dupPrereqs.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = dupPrereqs
        });
    }
    
    /****************************************************************
    * FETCH a specific pre-requisite met record, on a specified DUP / Object
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    
    public async Task<IActionResult> GetDupPrereq(int dup_id, string sd_oid, int id)
    {
        if (await _dupService.DupObjectDoesNotExistAsync(dup_id, sd_oid))
        {
            return Ok(ErrorInActionResponse<DupPrereq>("No object with that id found for specified DUP."));
        }
        var dupPrereq = await _dupService.GetDupPrereqAsync(id);
        if (dupPrereq == null) 
        {
            return Ok(NoAttributesResponse<DupPrereq>("No pre-requisite study with that id found."));
        }        
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DupPrereq>() { dupPrereq }
        });
    }

    /****************************************************************
    * CREATE a pre-requisite record for a specified DUP / Object
    ****************************************************************/
    
    [HttpPost("data-uses/{dup_id:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    
    public async Task<IActionResult> CreateDupPrereq(int dup_id, string sd_oid, 
           [FromBody] DupPrereq dupPrereqContent)
    {
        if (await _dupService.DupObjectDoesNotExistAsync(dup_id, sd_oid))
        {
            return Ok(ErrorInActionResponse<DupPrereq>("No object with that id found for specified DUP."));
        }
        dupPrereqContent.DupId = dup_id;
        dupPrereqContent.ObjectId = sd_oid;
        var dupPrereq = await _dupService.CreateDupPrereqAsync(dupPrereqContent);
        if (dupPrereq == null)
        {
            return Ok(ErrorInActionResponse<DupPrereq>("Error during Dup pre-requisite creation."));
        }    
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DupPrereq>() { dupPrereq }
        });
    }
    
    /****************************************************************
    * UPDATE a pre-requisite met record, for a specified DUP / Object
    ****************************************************************/

    [HttpPut("data-uses/{dup_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    
    public async Task<IActionResult> UpdateDupPrereq(int dup_id, string sd_oid, int id, 
           [FromBody] DupPrereq dupPrereqContent)
    {
        if (await _dupService.PrereqDoesNotExistAsync(dup_id, sd_oid, id))
        {
            return Ok(ErrorInActionResponse<DupPrereq>("No pre-requisite found with this id on specified DUP / object."));
        }
        var updatedDupPrereq = await _dupService.UpdateDupPrereqAsync(id, dupPrereqContent);
        if (updatedDupPrereq == null) 
        {
            return Ok(ErrorInActionResponse<DupPrereq>("Error during DUP object pre-requisite update."));
        }      
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DupPrereq>() { updatedDupPrereq }
        });
    }
    
    /****************************************************************
    * DELETE a pre-requisite met record, for a specified DUP / Object
    ****************************************************************/
    
    [HttpDelete("data-uses/{dup_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process prereqs endpoint"})]
    
    public async Task<IActionResult> DeleteDupPrereq(int dup_id, string sd_oid, int id)
    {
        if (await _dupService.PrereqDoesNotExistAsync(dup_id, sd_oid, id))
        {
            return Ok(ErrorInActionResponse<DupPrereq>("No pre-requisite found with this id on specified DUP / object."));
        }
        var count = await _dupService.DeleteDupPrereqAsync(id);
        return Ok(new ApiResponse<DupPrereq>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "DUP prereq has been removed." }, Data = null
        });
    }
}
