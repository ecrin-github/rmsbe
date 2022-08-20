using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupObjectsApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DupObjectsApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _parType = "DUP"; _parIdType = "id"; _entityType = "DupObject";
        _attType = "DUP object"; _attTypes = "DUP objects";
    }
    
    /****************************************************************
    * FETCH ALL objects linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dupId:int}/objects")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> GetDupObjectList(int dupId)
    {
        if (await _dupService.DupExists(dupId)) {
            var dupObjects = await _dupService.GetAllDupObjects(dupId);
            return dupObjects != null
                ? Ok(ListSuccessResponse(dupObjects.Count, dupObjects))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));    
    }

    /****************************************************************
    * FETCH a particular object, linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dupId:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> GetDupObject(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var dupObj = await _dupService.GetDupObject(id);
            return dupObj != null
                ? Ok(SingleSuccessResponse(new List<DupObject>() { dupObj }))
                : Ok(ErrorResponse("r", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new object, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dupId:int}/objects/{sdOid}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> CreateDupObject(int dupId, string sdOid,
        [FromBody] DupObject dupObjectContent)
    {
        if (await _dupService.DupExists(dupId)) {
            dupObjectContent.DupId = dupId;   // ensure this is the case
            dupObjectContent.SdOid = sdOid;
            var dupObj = await _dupService.CreateDupObject(dupObjectContent);
            return dupObj != null
                ? Ok(SingleSuccessResponse(new List<DupObject>() { dupObj }))
                : Ok(ErrorResponse("c", _attType, _parType, dupId.ToString(), dupId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));  
    }  

    /****************************************************************
    * UPDATE an object, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dupId:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> UpdateDupObject(int dupId, int id, 
        [FromBody] DupObject dupObjectContent)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            dupObjectContent.DupId = dupId;  // ensure this is the case
            dupObjectContent.Id = id;
            var updatedDupObject = await _dupService.UpdateDupObject(dupObjectContent);
            return updatedDupObject != null
                ? Ok(SingleSuccessResponse(new List<DupObject>() { updatedDupObject }))
                : Ok(ErrorResponse("u", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }

    /****************************************************************
    * DELETE a specified object, linked to a specified DUP
    ****************************************************************/
    
    [HttpDelete("data-uses/{dupId:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> DeleteDupObject(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var count = await _dupService.DeleteDupObject(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dupId.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
}