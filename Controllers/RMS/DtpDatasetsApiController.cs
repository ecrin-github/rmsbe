using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpDatasetsApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType;

    public DtpDatasetsApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "Object"; _parIdType = "sd_oid"; 
        _attType = "object dataset"; 
    }
    
    /****************************************************************
    * FETCH a particular dateset record, for a specified object
    ****************************************************************/
    
    [HttpGet("data-transfers/{dtpId:int}/objects/{sdOid}/dataset")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> GetDtpDataset(int dtpId, string sdOid)
    {
        if (await _dtpService.DtpObjectDatasetExists (dtpId, sdOid)) {
            var dtpDataset = await _dtpService.GetDtpDataset(dtpId, sdOid);
            return dtpDataset != null
                ? Ok(SingleSuccessResponse(new List<DtpDataset>() { dtpDataset }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, sdOid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, sdOid));
    }

    /****************************************************************
    * CREATE a new dataset record, linked to a specified object
    ****************************************************************/
    
    [HttpPost("data-transfers/{dtpId:int}/objects/{sdOid}/dataset")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> CreateDtpDataset(int dtpId, string sdOid, 
                 [FromBody] DtpDataset dtpDatasetContent)
    {
        if (await _dtpService.DtpObjectExists(dtpId, sdOid)) {
            dtpDatasetContent.DtpId = dtpId;   // ensure this is the case
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
    
    [HttpPut("data-transfers/{dtpId:int}/objects/{sdOid}/dataset")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> UpdateDtpDataset(int dtpId, string sdOid,
                 [FromBody] DtpDataset dtpDatasetContent)
    {
        if (await _dtpService.DtpObjectDatasetExists (dtpId, sdOid)) {
            dtpDatasetContent.DtpId = dtpId;  // ensure this is the case
            dtpDatasetContent.SdOid = sdOid;
            var updatedDataset = await _dtpService.UpdateDtpDataset(dtpDatasetContent);
            return updatedDataset != null
                ? Ok(SingleSuccessResponse(new List<DtpDataset>() { updatedDataset }))
                : Ok(ErrorResponse("u", _attType, _parType, dtpId.ToString(), sdOid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), sdOid));
    }
    
    /****************************************************************
    * DELETE a specified dataset record
    ****************************************************************/
    
    [HttpDelete("data-transfers/{dtpId:int}/objects/{sdOid}/dataset")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> DeleteDtpDataset(int dtpId, string sdOid)
    {
        if (await _dtpService.DtpObjectDatasetExists (dtpId, sdOid)) {
            var count = await _dtpService.DeleteDtpDataset(dtpId, sdOid);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtpId.ToString(), sdOid))
                : Ok(ErrorResponse("d", _attType, _parType, dtpId.ToString(), sdOid));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, sdOid));
    }
}