using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpDatasetsApiController : BaseApiController
{
    private readonly IRmsTransferService _rmsService;

    public DtpDatasetsApiController(IRmsTransferService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
    
    /****************************************************************
    * FETCH a particular dateset record, for a specified object
    ****************************************************************/
    
    [HttpGet("data-transfers/objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> GetDtpDataset(string sd_oid, int id)
    {
        if (await _rmsService.ObjectDatasetDoesNotExistAsync(sd_oid, id))
        {
            return Ok(ErrorInActionResponse<ObjectInstance>("No dataset with that id found for specified object."));
        }
        var dtpDataset = await _rmsService.GetDtpDatasetAsync(id);
        if (dtpDataset == null) 
        {
            return Ok(NoAttributesResponse<DtpDataset>("No DTP dataset with that id found."));
        }       
        return Ok(new ApiResponse<DtpDataset>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpDataset>() { dtpDataset }
        });
    }

    /****************************************************************
    * CREATE a new dataset record, linked to a specified object
    ****************************************************************/
    
    [HttpPost("data-transfers/objects/{sd_oid}/datasets")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> CreateDtpDataset(string sd_oid, 
        [FromBody] DtpDataset dtpDatasetContent)
    {
        dtpDatasetContent.ObjectId = sd_oid;
        var dataset = await _rmsService.CreateDtpDatasetAsync(dtpDatasetContent);
        if (dataset == null)
        {
            return Ok(ErrorInActionResponse<DtpDataset>("Error during Dtp dataset creation."));
        }    
        var datasetList = new List<DtpDataset>() { dataset };
        return Ok(new ApiResponse<DtpDataset>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpDataset>() { dataset }
        });
    }

    /****************************************************************
    * UPDATE a specific dataset record details
    ****************************************************************/
    
    [HttpPut("data-transfers/objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> UpdateDtpDataset(string sd_oid, int id, 
        [FromBody] DtpDataset dtpDatasetContent)
    {
        if (await _rmsService.ObjectDatasetDoesNotExistAsync(sd_oid, id))
        {
            return Ok(ErrorInActionResponse<ObjectInstance>("No dataset with that id found for specified object."));
        }
        var updatedDataset = await _rmsService.UpdateDtpDatasetAsync(id, dtpDatasetContent);
        if (updatedDataset == null)
        {
            return Ok(ErrorInActionResponse<ObjectInstance>("No DTP dataset with that id found."));
        }    
        return Ok(new ApiResponse<DtpDataset>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpDataset>() { updatedDataset }
        });
    }
    
    /****************************************************************
    * DELETE a specified dataset record
    ****************************************************************/
    
    [HttpDelete("data-transfers/objects/{sd_oid}/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> DeleteDtpDataset(string sd_oid, int id)
    {
        if (await _rmsService.ObjectDatasetDoesNotExistAsync(sd_oid, id))
        {
            return Ok(ErrorInActionResponse<ObjectInstance>("No dataset with that id found for specified object."));
        }
        var count = await _rmsService.DeleteDtpDatasetAsync(id);
        return Ok(new ApiResponse<DtpDataset>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTP dataset has been removed."}, Data = null
        });
    }
}
