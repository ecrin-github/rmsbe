using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class DtpDatasetsApiController : BaseApiController
{
    private readonly IRmsService _rmsService;

    public DtpDatasetsApiController(IRmsService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }

    /****************************************************************
    * FETCH ALL datasets
    ****************************************************************/

    [HttpGet("data-transfers/datasets")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> GetDtpDatasetList()
    {
        var datasets = await _rmsService.GetAllDtpDatasetsAsync();
        if (datasets == null || datasets.Count == 0)
        {
            return Ok(NoAttributesResponse<DtpDataset>("No datasets were found."));
        }   
        return Ok(new ApiResponse<DtpDataset>()
        {
            Total = datasets.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = datasets
        });
    }
    
    /****************************************************************
    * FETCH a particular dateset record
    ****************************************************************/
    
    [HttpGet("data-transfers/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> GetDtpDataset(int id)
    {
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
    
    [HttpPost("data-transfers/datasets/{object_id}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> CreateDtpDataset(string object_id, 
        [FromBody] DtpDataset dtpDatasetContent)
    {
        dtpDatasetContent.ObjectId = object_id;
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
    
    [HttpPut("data-transfers/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> UpdateDtpDataset(int id, 
        [FromBody] DtpDataset dtpDatasetContent)
    {
 // check object and dataset exist
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
    
    [HttpDelete("data-transfers/datasets/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process datasets endpoint"})]
    
    public async Task<IActionResult> DeleteDtpDataset(int id)
    {
        // check object and dataset exist
        var count = await _rmsService.DeleteDtpDatasetAsync(id);
        return Ok(new ApiResponse<DtpDataset>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTP dataset has been removed."}, Data = null
        });
    }
}
