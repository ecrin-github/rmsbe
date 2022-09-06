using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtaApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes;

    public DtaApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "DTP"; _parIdType = "id"; 
        _attType = "DTA"; _attTypes = "DTAs";
    }

    /****************************************************************
    * FETCH the DTA linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtpId:int}/dta")]
    [SwaggerOperation(Tags = new[] { "DTP DTA endpoint" })]

    public async Task<IActionResult> GetDta(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId))
        {
            var dta = await _dtpService.GetDta(dtpId);
            return dta != null
                ? Ok(SingleSuccessResponse(new List<Dta>() { dta }))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));
    }

    /****************************************************************
    * CREATE a new DTA, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtpId:int}/dta")]
    [SwaggerOperation(Tags = new[] { "DTP DTA endpoint" })]

    public async Task<IActionResult> CreateDta(int dtpId,
                 [FromBody] Dta dtaContent)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            dtaContent.DtpId = dtpId;    // ensure this is the case
            var dta = await _dtpService.CreateDta(dtaContent);
            return dta != null
                ? Ok(SingleSuccessResponse(new List<Dta>() { dta }))
                : Ok(ErrorResponse("c", _attType, _parType, dtpId.ToString(), dtpId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));
    }

    /****************************************************************
    * UPDATE the DTA linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtpId:int}/dta")]
    [SwaggerOperation(Tags = new[] { "DTP DTA endpoint" })]

    public async Task<IActionResult> UpdateDta(int dtpId,
                 [FromBody] Dta dtaContent)
    {
        if (await _dtpService.DtpDtaExists(dtpId)) {
            dtaContent.DtpId = dtpId;  // ensure this is the case
            var updatedDta = await _dtpService.UpdateDta(dtaContent);
            return updatedDta != null
                ? Ok(SingleSuccessResponse(new List<Dta>() { updatedDta }))
                : Ok(ErrorResponse("u", _attType, _parType, dtpId.ToString(), dtpId.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), dtpId.ToString()));
    }

    /****************************************************************
    * DELETE the DTA linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtpId:int}/dta")]
    [SwaggerOperation(Tags = new[] { "DTP DTA endpoint" })]

    public async Task<IActionResult> DeleteDta(int dtpId)
    {
        if (await _dtpService.DtpDtaExists(dtpId)) {
            var count = await _dtpService.DeleteDta(dtpId);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtpId.ToString(), dtpId.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtpId.ToString(), dtpId.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), dtpId.ToString()));
    }
}