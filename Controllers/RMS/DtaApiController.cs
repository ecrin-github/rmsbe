using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtaApiController : BaseApiController
{
    private readonly IDtpService _dtpService;

    public DtaApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
    }

    /****************************************************************
    * FETCH ALL DTAs linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/accesses")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> GetDtaList(int dtp_id)
    {
        if (await _dtpService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<Dta>());
        }
        var dtas = await _dtpService.GetAllDtasAsync(dtp_id);
        if (dtas == null || dtas.Count == 0)
        {
            return Ok(NoAttributesResponse<Dta>("No Dtas were found."));
        }
        return Ok(new ApiResponse<Dta>()
        {
            Total = dtas.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = dtas
        });
    }

    /****************************************************************
    * FETCH a particular DTA linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
    
    public async Task<IActionResult> GetDta(int dtp_id, int id)
    {
        if (await _dtpService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<Dta>());
        }
        var dta = await _dtpService.GetDtaAsync(id);
        if (dta == null) 
        {
            return Ok(NoAttributesResponse<Dta>("No DTA with that id found."));
        }       
        return Ok(new ApiResponse<Dta>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dta> { dta }
        });
    }

    /****************************************************************
    * CREATE a new DTA, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/accesses")]
    [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
    
    public async Task<IActionResult> CreateDta(int dtp_id, 
         [FromBody] Dta dtaContent)
    {
        if (await _dtpService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<Dta>());
        }
        dtaContent.DtpId = dtp_id;
        var dta = await _dtpService.CreateDtaAsync(dtaContent);
        if (dta == null)
        {
            return Ok(ErrorInActionResponse<Dta>("Error during DTA creation."));
        }    
        return Ok(new ApiResponse<Dta>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dta>() { dta }
        });
    }

    /****************************************************************
    * UPDATE a DTA, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
    
    public async Task<IActionResult> UpdateDta(int dtp_id, int id, 
           [FromBody] Dta dtaContent)
    {
        if (await _dtpService.DtpAttributeDoesNotExistAsync(dtp_id, "DTA", id))
        {
            return Ok(ErrorInActionResponse<Dta>("No agreement with that id found for specified DTP."));
        }
        var updatedDta = await _dtpService.UpdateDtaAsync(id, dtaContent);
        if (updatedDta == null) 
        {
            return Ok(ErrorInActionResponse<Dta>("Error during DTA update."));
        }   
        return Ok(new ApiResponse<Dta>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dta>() { updatedDta }
        });
    }

    /****************************************************************
    * DELETE a specified DTA, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
    
    public async Task<IActionResult> DeleteDta(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeDoesNotExistAsync(dtp_id, "DTA", id))
        {
            return Ok(ErrorInActionResponse<Dta>("No agreement with that id found for specified DTP."));
        }
        var count = await _dtpService.DeleteDtaAsync(id);
        return Ok(new ApiResponse<Dtp>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTA has been removed."}, Data = null
        });
    }
}