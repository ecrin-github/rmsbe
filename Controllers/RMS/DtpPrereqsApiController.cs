using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpPrereqsApiController : BaseApiController
{
    private readonly IRmsTransferService _rmsService;

    public DtpPrereqsApiController(IRmsTransferService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
    
    /****************************************************************
    * FETCH ALL pre-requisite records, for a specified object / DTP
    ****************************************************************/
    
    [HttpGet("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> GetAccessPrereqList(int dtp_id, string sd_oid)
    {
        if (await _rmsService.DtpObjectDoesNotExistAsync(dtp_id, sd_oid))
        {
            return Ok(ErrorInActionResponse<AccessPrereq>("No object with that id found for specified DTP."));
        }
        var accessPrereqs = await _rmsService.GetAllDtpAccessPrereqsAsync(dtp_id, sd_oid);
        if (accessPrereqs == null || accessPrereqs.Count == 0)
        {
            return Ok(NoAttributesResponse<DtpObject>("No pre-requisites were found for the specified DTP / Object."));
        }   
        return Ok(new ApiResponse<AccessPrereq>()
        {
            Total = accessPrereqs.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = accessPrereqs
        });
    }
    
    /****************************************************************
    * FETCH a particular pre-requisite record, for a specified object
    ****************************************************************/
    
    [HttpGet("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> GetAccessPrereq(int dtp_id, string sd_oid, int id)
    {
        if (await _rmsService.DtpObjectDoesNotExistAsync(dtp_id, sd_oid))
        {
            return Ok(ErrorInActionResponse<AccessPrereq>("No object with that id found for specified DTP."));
        }
        var accessPrereq = await _rmsService.GetAccessPrereqAsync(id);
        if (accessPrereq == null) 
        {
            return Ok(NoAttributesResponse<AccessPrereq>("No access pre-requisite with that id found."));
        }       
        return Ok(new ApiResponse<AccessPrereq>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<AccessPrereq>() { accessPrereq }
        });
    }

    /****************************************************************
    * CREATE a new pre-requisite record, linked to a specified object
    ****************************************************************/
    
    [HttpPost("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> CreateAccessPrereq(int dtp_id, string sd_oid, 
        [FromBody] AccessPrereq accessPrereqContent)
    {
        if (await _rmsService.DtpObjectDoesNotExistAsync(dtp_id, sd_oid))
        {
            return Ok(ErrorInActionResponse<AccessPrereq>("No object with that id found for specified DTP."));
        }
        accessPrereqContent.DtpId = dtp_id;
        accessPrereqContent.ObjectId = sd_oid;
        var accessPrereq = await _rmsService.CreateAccessPrereqAsync(accessPrereqContent);
        if (accessPrereq == null)
        {
            return Ok(ErrorInActionResponse<AccessPrereq>("Error during Dtp pre-requisite creation."));
        }    
        return Ok(new ApiResponse<AccessPrereq>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<AccessPrereq>() { accessPrereq }
        });
    }

    /****************************************************************
    * UPDATE a specific pre-requisite record details
    ****************************************************************/
    
    [HttpPut("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> UpdateAccessPrereq(int dtp_id, string sd_oid, int id, 
        [FromBody] AccessPrereq accessPrereqContent)
    {
        if (await _rmsService.ObjectDtpPrereqDoesNotExistAsync(dtp_id, sd_oid, id))
        {
            return Ok(ErrorInActionResponse<AccessPrereq>("No pre-requisite with that id for specified DTP / object."));
        }
        var updatedAccessPrereq = await _rmsService.UpdateAccessPrereqAsync(id, accessPrereqContent);
        if (updatedAccessPrereq == null)
        {
            return Ok(ErrorInActionResponse<AccessPrereq>("Error during DTP object pre-requisite update."));
        }    
        return Ok(new ApiResponse<AccessPrereq>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<AccessPrereq>() { updatedAccessPrereq }
        });
    }
    
    /****************************************************************
    * DELETE a specified pre-requisite record
    ****************************************************************/
    
    [HttpDelete("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> DeleteAccessPrereq(int dtp_id, string sd_oid, int id)
    {
        if (await _rmsService.ObjectDtpPrereqDoesNotExistAsync(dtp_id, sd_oid, id))
        {
            return Ok(ErrorInActionResponse<AccessPrereq>("No pre-requisite with that id for specified DTP / object."));
        }
        var count = await _rmsService.DeleteAccessPrereqAsync(id);
        return Ok(new ApiResponse<AccessPrereq>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTP Object pre-requisite has been removed."}, Data = null
        });
    }
}
