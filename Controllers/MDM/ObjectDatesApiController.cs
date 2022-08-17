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
    
    [HttpGet("data-objects/{sdOid}/dates")]
    [SwaggerOperation(Tags = new[] { "Object dates endpoint" })]
    
    public async Task<IActionResult> GetObjectDates(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objDates = await _objectService.GetObjectDates(sdOid);
            return objDates != null
                ? Ok(ListSuccessResponse(objDates.Count, objDates))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }

    /****************************************************************
    * FETCH A SINGLE object date
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/dates/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Object dates endpoint" })]
    
    public async Task<IActionResult> GetObjectDate(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objDate = await _objectService.GetObjectDate(id);
            return objDate != null
                ? Ok(SingleSuccessResponse(new List<ObjectDate>() { objDate }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }

    /****************************************************************
    * CREATE a new date for a specified object
    ****************************************************************/
    
    [HttpPost("data-objects/{sdOid}/dates")]
    [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
    
    public async Task<IActionResult> CreateObjectDate(string sdOid, 
                 [FromBody] ObjectDate objDateContent)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            objDateContent.SdOid = sdOid;   // ensure this is the case
            var objDate = await _objectService.CreateObjectDate(objDateContent);
            return objDate != null
                ? Ok(SingleSuccessResponse(new List<ObjectDate>() { objDate }))
                : Ok(ErrorResponse("c", _attType, _parType, sdOid, sdOid));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
    
    /****************************************************************
    * UPDATE a single specified object date
    ****************************************************************/
    
    [HttpPut("data-objects/{sdOid}/dates/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
    
    public async Task<IActionResult> UpdateObjectDate(string sdOid, int id, 
                 [FromBody] ObjectDate objDateContent)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            objDateContent.SdOid = sdOid;  // ensure this is the case
            objDateContent.Id = id;
            var updatedObjDate = await _objectService.UpdateObjectDate(objDateContent);
            return updatedObjDate != null
                ? Ok(SingleSuccessResponse(new List<ObjectDate>() { updatedObjDate }))
                : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a single specified object date
    ****************************************************************/
    
    [HttpDelete("data-objects/{sdOid}/dates/{id:int}")]
    [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
    
    public async Task<IActionResult> DeleteObjectDate(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var count = await _objectService.DeleteObjectDate(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}