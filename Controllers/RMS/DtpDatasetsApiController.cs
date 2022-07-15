using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpDatasetsApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _entityType;

    public DtpDatasetsApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "Object"; _parIdType = "sd_oid"; _entityType = "DtpDataset";
        _attType = "object dataset"; 
    }
    
    /****************************************************************
    * FETCH a particular dateset record, for a specified object
    ****************************************************************/
    
    [HttpGet("data-transfers/{dtpId}/objects/{sd_oid}/dataset/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> GetDtpDataset(int dtpId, string sdOid, int id)
    {
        if (await _dtpService.DtpObjectAttributeExists (dtpId, sdOid, _entityType, id)) {
            var dtpDataset = await _dtpService.GetDtpDataset(id);
            return dtpDataset != null
                ? Ok(SingleSuccessResponse(new List<DtpDataset>() { dtpDataset }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }

    /****************************************************************
    * CREATE a new dataset record, linked to a specified object
    ****************************************************************/
    
    [HttpPost("data-transfers/{dtpId}/objects/{sd_oid}/dataset")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> CreateDtpDataset(int dtpId, string sdOid, 
                 [FromBody] DtpDataset dtpDatasetContent)
    {
        if (await _dtpService.DtpObjectExists(dtpId, sdOid)) {
            dtpDatasetContent.DtpId = dtpId;
            dtpDatasetContent.SdOid = sdOid;
            var dataset = await _dtpService.CreateDtpDataset(dtpDatasetContent);
            return dataset != null
                ? Ok(SingleSuccessResponse(new List<DtpDataset>() { dataset }))
                : Ok(ErrorResponse("c", _attType, _parType, sdOid, dtpId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
    
    /****************************************************************
    * UPDATE a specific dataset record details
    ****************************************************************/
    
    [HttpPut("data-transfers/{dtpId}/objects/{sd_oid}/dataset/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> UpdateDtpDataset(int dtpId, string sdOid, int id, 
                 [FromBody] DtpDataset dtpDatasetContent)
    {
        if (await _dtpService.DtpObjectAttributeExists (dtpId, sdOid, _entityType, id)) {
            var updatedDataset = await _dtpService.UpdateDtpDataset(id, dtpDatasetContent);
            return updatedDataset != null
                ? Ok(SingleSuccessResponse(new List<DtpDataset>() { updatedDataset }))
                : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified dataset record
    ****************************************************************/
    
    [HttpDelete("data-transfers/{dtpId}/objects/{sd_oid}/dataset/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> DeleteDtpDataset(int dtpId, string sdOid, int id)
    {
        if (await _dtpService.DtpObjectAttributeExists (dtpId, sdOid, _entityType, id)) {
            var count = await _dtpService.DeleteDtpDataset(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}