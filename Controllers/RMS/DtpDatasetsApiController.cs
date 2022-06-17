using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpDatasetsApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DtpDatasetsApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "Object"; _parIdType = "sd_oid"; _entityType = "DtpDataset";
        _attType = "object dataset"; 
    }
    
    /****************************************************************
    * FETCH a particular dateset record, for a specified object
    ****************************************************************/
    
    [HttpGet("data-transfers/{dtp_id}/objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> GetDtpDataset(int dtp_id, string sd_oid, int id)
    {
        if (await _dtpService.DtpObjectAttributeExistsAsync (dtp_id, sd_oid, _entityType, id)) {
            var dtpDataset = await _dtpService.GetDtpDatasetAsync(id);
            return dtpDataset != null
                ? Ok(SingleSuccessResponse(new List<DtpDataset>() { dtpDataset }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }

    /****************************************************************
    * CREATE a new dataset record, linked to a specified object
    ****************************************************************/
    
    [HttpPost("data-transfers/{dtp_id}/objects/{sd_oid}/datasets")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> CreateDtpDataset(int dtp_id, string sd_oid, 
                 [FromBody] DtpDataset dtpDatasetContent)
    {
        if (await _dtpService.DtpObjectExistsAsync(dtp_id, sd_oid)) {
            dtpDatasetContent.DtpId = dtp_id;
            dtpDatasetContent.SdOid = sd_oid;
            var dataset = await _dtpService.CreateDtpDatasetAsync(dtpDatasetContent);
            return dataset != null
                ? Ok(SingleSuccessResponse(new List<DtpDataset>() { dataset }))
                : Ok(ErrorResponse("c", _attType, _parType, sd_oid, dtp_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
    
    /****************************************************************
    * UPDATE a specific dataset record details
    ****************************************************************/
    
    [HttpPut("data-transfers/{dtp_id}/objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> UpdateDtpDataset(int dtp_id, string sd_oid, int id, 
                 [FromBody] DtpDataset dtpDatasetContent)
    {
        if (await _dtpService.DtpObjectAttributeExistsAsync (dtp_id, sd_oid, _entityType, id)) {
            var updatedDataset = await _dtpService.UpdateDtpDatasetAsync(id, dtpDatasetContent);
            return updatedDataset != null
                ? Ok(SingleSuccessResponse(new List<DtpDataset>() { updatedDataset }))
                : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified dataset record
    ****************************************************************/
    
    [HttpDelete("data-transfers/{dtp_id}/objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> DeleteDtpDataset(int dtp_id, string sd_oid, int id)
    {
        if (await _dtpService.DtpObjectAttributeExistsAsync (dtp_id, sd_oid, _entityType, id)) {
            var count = await _dtpService.DeleteDtpDatasetAsync(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}