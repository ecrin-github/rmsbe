using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpPrereqsApiController : BaseApiController
{
    private readonly IDtpService _dtpService;

    public DtpPrereqsApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
    }

    /****************************************************************
    * FETCH ALL pre-requisite records, for a specified object / DTP
    ****************************************************************/
    
    [HttpGet("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> GetDtpPrereqList(int dtp_id, string sd_oid)
    {
        if (await _dtpService.DtpObjectDoesNotExistAsync(dtp_id, sd_oid))
        {
            return Ok(ErrorInActionResponse<DtpPrereq>("No object with that id found for specified DTP."));
        }
        var dtpPrereqs = await _dtpService.GetAllDtpPrereqsAsync(dtp_id, sd_oid);
        if (dtpPrereqs == null || dtpPrereqs.Count == 0)
        {
            return Ok(NoAttributesResponse<DtpObject>("No pre-requisites were found for the specified DTP / Object."));
        }   
        return Ok(new ApiResponse<DtpPrereq>()
        {
            Total = dtpPrereqs.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = dtpPrereqs
        });
    }
    
    /****************************************************************
    * FETCH a particular pre-requisite record, for a specified object
    ****************************************************************/
    
    [HttpGet("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> GetDtpPrereq(int dtp_id, string sd_oid, int id)
    {
        if (await _dtpService.DtpObjectDoesNotExistAsync(dtp_id, sd_oid))
        {
            return Ok(ErrorInActionResponse<DtpPrereq>("No object with that id found for specified DTP."));
        }
        var dtpPrereq = await _dtpService.GetDtpPrereqAsync(id);
        if (dtpPrereq == null) 
        {
            return Ok(NoAttributesResponse<DtpPrereq>("No access pre-requisite with that id found."));
        }       
        return Ok(new ApiResponse<DtpPrereq>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpPrereq>() { dtpPrereq }
        });
    }

    /****************************************************************
    * CREATE a new pre-requisite record, linked to a specified object
    ****************************************************************/
    
    [HttpPost("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> CreateDtpPrereq(int dtp_id, string sd_oid, 
        [FromBody] DtpPrereq dtpPrereqContent)
    {
        if (await _dtpService.DtpObjectDoesNotExistAsync(dtp_id, sd_oid))
        {
            return Ok(ErrorInActionResponse<DtpPrereq>("No object with that id found for specified DTP."));
        }
        dtpPrereqContent.DtpId = dtp_id;
        dtpPrereqContent.ObjectId = sd_oid;
        var dtpPrereq = await _dtpService.CreateDtpPrereqAsync(dtpPrereqContent);
        if (dtpPrereq == null)
        {
            return Ok(ErrorInActionResponse<DtpPrereq>("Error during Dtp pre-requisite creation."));
        }    
        return Ok(new ApiResponse<DtpPrereq>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpPrereq>() { dtpPrereq }
        });
    }

    /****************************************************************
    * UPDATE a specific pre-requisite record details
    ****************************************************************/
    
    [HttpPut("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> UpdateDtpPrereq(int dtp_id, string sd_oid, int id, 
        [FromBody] DtpPrereq dtpPrereqContent)
    {
        if (await _dtpService.ObjectDtpPrereqDoesNotExistAsync(dtp_id, sd_oid, id))
        {
            return Ok(ErrorInActionResponse<DtpPrereq>("No pre-requisite with that id for specified DTP / object."));
        }
        var updatedDtpPrereq = await _dtpService.UpdateDtpPrereqAsync(id, dtpPrereqContent);
        if (updatedDtpPrereq == null)
        {
            return Ok(ErrorInActionResponse<DtpPrereq>("Error during DTP object pre-requisite update."));
        }    
        return Ok(new ApiResponse<DtpPrereq>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpPrereq>() { updatedDtpPrereq }
        });
    }
    
    /****************************************************************
    * DELETE a specified pre-requisite record
    ****************************************************************/
    
    [HttpDelete("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> DeleteDtpPrereq(int dtp_id, string sd_oid, int id)
    {
        if (await _dtpService.ObjectDtpPrereqDoesNotExistAsync(dtp_id, sd_oid, id))
        {
            return Ok(ErrorInActionResponse<DtpPrereq>("No pre-requisite with that id for specified DTP / object."));
        }
        var count = await _dtpService.DeleteDtpPrereqAsync(id);
        var response = (count == 0) 
            ? Ok(ErrorInActionResponse<DtpPrereq>("Deletion does not appear to have occured."))
            : Ok(new ApiResponse<DtpPrereq>()
            {
                Total = count, StatusCode = Ok().StatusCode,
                Messages = new List<string>(){"DTP Object pre-requisite has been removed."}, Data = null
            });
        return response;
    }
}
