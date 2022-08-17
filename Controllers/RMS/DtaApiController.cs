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

    [HttpGet("data-transfers/{dtpId:int}/dtas")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> GetDtaList(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            var dtas = await _dtpService.GetAllDtas(dtpId);
            return dtas != null
                ? Ok(ListSuccessResponse(dtas.Count, dtas))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));
    }

    /****************************************************************
    * FETCH a particular DTA linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtpId:int}/dtas/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> GetDta(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var dta = await _dtpService.GetDta(id);
            return dta != null
                ? Ok(SingleSuccessResponse(new List<Dta>() { dta }))
                : Ok(ErrorResponse("r", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }

    /****************************************************************
    * CREATE a new DTA, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtpId:int}/dtas")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

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
    * UPDATE a DTA, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtpId:int}/dtas/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> UpdateDta(int dtpId, int id,
                 [FromBody] Dta dtaContent)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            dtaContent.DtpId = dtpId;  // ensure this is the case
            dtaContent.Id = id;
            var updatedDta = await _dtpService.UpdateDta(dtaContent);
            return updatedDta != null
                ? Ok(SingleSuccessResponse(new List<Dta>() { updatedDta }))
                : Ok(ErrorResponse("u", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }

    /****************************************************************
    * DELETE a specified DTA, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtpId:int}/dtas/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> DeleteDta(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var count = await _dtpService.DeleteDta(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtpId.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
}