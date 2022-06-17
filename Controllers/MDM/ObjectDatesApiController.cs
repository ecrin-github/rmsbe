using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class ObjectDatesApiController : BaseApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public ObjectDatesApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectDate";
        _attType = "object date"; _attTypes = "object dates";
    }

    /****************************************************************
    * FETCH ALL dates for a specified object
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/dates")]
    [SwaggerOperation(Tags = new[] { "Object dates endpoint" })]
    
    public async Task<IActionResult> GetObjectDates(string sd_oid)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            var objDates = await _objectService.GetObjectDatesAsync(sd_oid);
            return objDates != null
                ? Ok(ListSuccessResponse(objDates.Count, objDates))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));    
    }

    /****************************************************************
    * FETCH A SINGLE object date
    ****************************************************************/
    
    [HttpGet("data-objects/{sd_oid}/dates/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Object dates endpoint" })]
    
    public async Task<IActionResult> GetObjectDate(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var objDate = await _objectService.GetObjectDateAsync(id);
            return objDate != null
                ? Ok(SingleSuccessResponse(new List<ObjectDate>() { objDate }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }

    /****************************************************************
    * CREATE a new date for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sd_oid}/dates")]
    [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
    
    public async Task<IActionResult> CreateObjectDate(string sd_oid, 
                 [FromBody] ObjectDate objDateContent)
    {
        if (await _objectService.ObjectExistsAsync(sd_oid)) {
            objDateContent.SdOid = sd_oid;
            var objDate = await _objectService.CreateObjectDateAsync(objDateContent);
            return objDate != null
                ? Ok(SingleSuccessResponse(new List<ObjectDate>() { objDate }))
                : Ok(ErrorResponse("c", _attType, _parType, sd_oid, sd_oid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object date
    ****************************************************************/
    
    [HttpPut("data-objects/{sd_oid}/dates/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
    
    public async Task<IActionResult> UpdateObjectDate(string sd_oid, int id, 
                 [FromBody] ObjectDate objDateContent)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var updatedObjDate = await _objectService.UpdateObjectDateAsync(id, objDateContent);
            return updatedObjDate != null
                ? Ok(SingleSuccessResponse(new List<ObjectDate>() { updatedObjDate }))
                : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a single specified object date
    ****************************************************************/
    
    [HttpDelete("data-objects/{sd_oid}/dates/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
    
    public async Task<IActionResult> DeleteObjectDate(string sd_oid, int id)
    {
        if (await _objectService.ObjectAttributeExistsAsync(sd_oid, _entityType, id)) {
            var count = await _objectService.DeleteObjectDateAsync(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}