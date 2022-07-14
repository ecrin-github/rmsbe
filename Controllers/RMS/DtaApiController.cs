using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtaApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DtaApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "DTP"; _parIdType = "id"; _entityType = "Dta";
        _attType = "DTA"; _attTypes = "DTAs";
    }

    /****************************************************************
    * FETCH ALL DTAs linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/accesses")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> GetDtaList(int dtp_id)
    {
        if (await _dtpService.DtpExists(dtp_id)) {
            var dtas = await _dtpService.GetAllDtas(dtp_id);
            return dtas != null
                ? Ok(ListSuccessResponse(dtas.Count, dtas))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtp_id.ToString()));
    }

    /****************************************************************
    * FETCH a particular DTA linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> GetDta(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var dta = await _dtpService.GetDta(id);
            return dta != null
                ? Ok(SingleSuccessResponse(new List<Dta>() { dta }))
                : Ok(ErrorResponse("r", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }

    /****************************************************************
    * CREATE a new DTA, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/accesses")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> CreateDta(int dtp_id,
                 [FromBody] Dta dtaContent)
    {
        if (await _dtpService.DtpExists(dtp_id)) {
            dtaContent.DtpId = dtp_id;
            var dta = await _dtpService.CreateDta(dtaContent);
            return dta != null
                ? Ok(SingleSuccessResponse(new List<Dta>() { dta }))
                : Ok(ErrorResponse("c", _attType, _parType, dtp_id.ToString(), dtp_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtp_id.ToString()));
    }

    /****************************************************************
    * UPDATE a DTA, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> UpdateDta(int dtp_id, int id,
                 [FromBody] Dta dtaContent)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var updatedDta = await _dtpService.UpdateDta(id, dtaContent);
            return updatedDta != null
                ? Ok(SingleSuccessResponse(new List<Dta>() { updatedDta }))
                : Ok(ErrorResponse("u", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }

    /****************************************************************
    * DELETE a specified DTA, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> DeleteDta(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var count = await _dtpService.DeleteDta(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtp_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
}