using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectApiController : BaseApiController
{
    private readonly IObjectService _objectService;

    public ObjectApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }
    
    /****************************************************************
    * FETCH a specific data object (including attribute data)
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
    
    public async Task<IActionResult> GetObjectById(string sd_oid)
    {
        var fullDdataObject = await _objectService.GetFullObjectByIdAsync(sd_oid);
        if (fullDdataObject == null) 
        {
            return Ok(NoAttributesResponse<FullDataObject>("No data object found with that id."));
        }   
        return Ok(new ApiResponse<FullDataObject>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<FullDataObject>() { fullDdataObject }
        });
    }
    
    /****************************************************************
    * DELETE a specific data object (including all attribute data)
    ****************************************************************/

    [HttpDelete("data-objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
    
    public async Task<IActionResult> DeleteDataObject(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<FullDataObject>());
        }
        var count = await _objectService.DeleteFullObjectAsync(sd_oid);
        return Ok(new ApiResponse<FullDataObject>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>() { "Full data object has been removed." }, Data = null
        });
    }
    
    /****************************************************************
    * FETCH ALL data objects (without attributes)
    ****************************************************************/
    
    [HttpGet("data-objects/data")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetObjectData()
    {
        var objectData = await _objectService.GetAllObjectsDataAsync();
        if (objectData == null || objectData.Count == 0)
        {
            return Ok(NoAttributesResponse<DataObjectData>("No data object records were found."));
        }    
        return Ok(new ApiResponse<DataObjectData>()
        {
            Total = objectData.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = objectData
        });
    }
    
    /****************************************************************
    * FETCH n RECENT data objects (without attributes)
    ****************************************************************/
    
    [HttpGet("data-objects/data/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetRecentObjectData(int n)
    {
        var recentObjectData = await _objectService.GetRecentObjectsDataAsync(n);
        if (recentObjectData == null || recentObjectData.Count == 0)
        {
            return Ok(NoAttributesResponse<DataObjectData>("No data object records were found."));
        }    
        return Ok(new ApiResponse<DataObjectData>()
        {
            Total = recentObjectData.Count, StatusCode = Ok().StatusCode, Messages = null, 
            Data = recentObjectData
        });
    }

    /****************************************************************
    * FETCH a single specified data object (without attributes)
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/data")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> GetObjectData(string sd_oid)
    {
        var dataObject = await _objectService.GetObjectDataAsync(sd_oid);
        if (dataObject == null) 
        {
            return Ok(NoAttributesResponse<DataObjectData>("No data object found with that id."));
        }
        return Ok(new ApiResponse<DataObjectData>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DataObjectData>() { dataObject }
        });
    }

    /****************************************************************
    * CREATE a single specified data object (without attributes)
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Object data endpoint"})]
    
    public async Task<IActionResult> CreateObjectData(string sd_sid, 
        [FromBody] DataObjectData dataObjectContent)
    {
        dataObjectContent.SdSid = sd_sid;
        var dataObj = await _objectService.CreateDataObjectDataAsync(dataObjectContent);
        if (dataObj == null)
        {
            return Ok(ErrorInActionResponse<DataObjectData>("Error during data object creation."));
        } 
        return Ok(new ApiResponse<DataObjectData>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DataObjectData>() { dataObj }
        });
    }

    /****************************************************************
    * UPDATE a single specified data object (without attributes)
    ****************************************************************/

    [HttpPut("data-objects/{sd_oid}/data")]
    [SwaggerOperation(Tags = new[] { "Object data endpoint" })]
    
    public async Task<IActionResult> UpdateObjectData(string sd_oid, 
        [FromBody] DataObjectData dataObjectContent)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<DataObjectData>);
        }
        dataObjectContent.SdOid = sd_oid;
        var updatedDataObject = await _objectService.UpdateDataObjectDataAsync(dataObjectContent);
        if (updatedDataObject == null)
        {
            return Ok(ErrorInActionResponse<DataObjectData>("Error during data object update."));
        }    
        return Ok(new ApiResponse<DataObjectData>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DataObjectData>() { updatedDataObject }
        });
    }
    
    /****************************************************************
    * DELETE a single specified data object (without attributes)
    ****************************************************************/
    
    [HttpDelete("data-objects/{sd_oid}/data")]
    [SwaggerOperation(Tags = new[] { "Study data endpoint" })]

    public async Task<IActionResult> DeleteStudyData(string sd_oid)
    {
        if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
        {
            return Ok(NoObjectResponse<DataObjectData>());
        }
        var count = await _objectService.DeleteDataObjectAsync(sd_oid);
        return Ok(new ApiResponse<DataObjectData>()
        {
            Total = count, StatusCode = Ok().StatusCode, 
            Messages = new List<string>() { "Data object record data has been removed." }, Data = null
        });
    }
}