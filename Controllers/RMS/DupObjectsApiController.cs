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
    
    [HttpGet("data-uses/{dup_id:int}/objects")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> GetDupObjectList(int dup_id)
    {
        if (await _dupService.DupExists(dup_id)) {
            var dupObjects = await _dupService.GetAllDupObjects(dup_id);
            return dupObjects != null
                ? Ok(ListSuccessResponse(dupObjects.Count, dupObjects))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));    
    }

    /****************************************************************
    * FETCH a particular object, linked to a specified DUP
    ****************************************************************/
    
    [HttpGet("data-uses/{dup_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> GetDupObject(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {
            var dupObj = await _dupService.GetDupObject(id);
            return dupObj != null
                ? Ok(SingleSuccessResponse(new List<DupObject>() { dupObj }))
                : Ok(ErrorResponse("r", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new object, linked to a specified DUP
    ****************************************************************/
    
    [HttpPost("data-uses/{dup_id:int}/objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> CreateDupObject(int dup_id, string sd_oid,
        [FromBody] DupObject dupObjectContent)
    {
        if (await _dupService.DupExists(dup_id)) {
            dupObjectContent.DupId = dup_id;
            dupObjectContent.ObjectId = sd_oid;
            var dupObj = await _dupService.CreateDupObject(dupObjectContent);
            return dupObj != null
                ? Ok(SingleSuccessResponse(new List<DupObject>() { dupObj }))
                : Ok(ErrorResponse("c", _attType, _parType, dup_id.ToString(), dup_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));  
    }  

    /****************************************************************
    * UPDATE an object, linked to a specified DUP
    ****************************************************************/
    
    [HttpPut("data-uses/{dup_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> UpdateDupObject(int dup_id, int id, 
        [FromBody] DupObject dupObjectContent)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {
            var updatedDupObject = await _dupService.UpdateDupObject(dup_id, dupObjectContent);
            return updatedDupObject != null
                ? Ok(SingleSuccessResponse(new List<DupObject>() { updatedDupObject }))
                : Ok(ErrorResponse("u", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }

    /****************************************************************
    * DELETE a specified object, linked to a specified DUP
    ****************************************************************/
    
    [HttpDelete("data-uses/{dup_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process objects endpoint"})]
    
    public async Task<IActionResult> DeleteDupObject(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExists(dup_id, _entityType, id)) {
            var count = await _dupService.DeleteDupObject(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dup_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
}