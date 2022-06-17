using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupPeopleApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DupPeopleApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _parType = "DUP"; _parIdType = "id"; _entityType = "DupPerson";
        _attType = "DUP person"; _attTypes = "DUP people";
    }
 
    /****************************************************************
    * FETCH ALL people linked to a specified DUP
    ****************************************************************/
   
    [HttpGet("data-uses/{dup_id:int}/people")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> GetDupPeopleList(int dup_id)
    {
        if (await _dupService.DupExistsAsync(dup_id)) {
            var dupPeople = await _dupService.GetAllDupPeopleAsync(dup_id);
            return dupPeople != null
                ? Ok(ListSuccessResponse(dupPeople.Count, dupPeople))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));    
    }
     
    /****************************************************************
    * FETCH a particular person linked to a specified DUP
    ****************************************************************/

    [HttpGet("data-uses/{dup_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> GetDupPerson(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExistsAsync(dup_id, _entityType, id)) {
            var dupPerson = await _dupService.GetDupPersonAsync(id);
            return dupPerson != null
                ? Ok(SingleSuccessResponse(new List<DupPerson>() { dupPerson }))
                : Ok(ErrorResponse("r", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new person, linked to a specified DUP
    ****************************************************************/

    [HttpPost("data-uses/{dup_id:int}/people/{person_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> CreateDupPerson(int dup_id, int person_id, 
                 [FromBody] DupPerson dupPersonContent)
    {
        if (await _dupService.DupExistsAsync(dup_id)) {
            dupPersonContent.DupId = dup_id;
            dupPersonContent.PersonId = person_id;
            var dupPerson = await _dupService.CreateDupPersonAsync(dupPersonContent);
            return dupPerson != null
                ? Ok(SingleSuccessResponse(new List<DupPerson>() { dupPerson }))
                : Ok(ErrorResponse("c", _attType, _parType, dup_id.ToString(), dup_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));  
    }  
     
    /****************************************************************
    * UPDATE a person, linked to a specified DUP
    ****************************************************************/

    [HttpPut("data-uses/{dup_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> UpdateDupPerson(int dup_id, int id, 
                 [FromBody] DupPerson dupPersonContent)
    {
        if (await _dupService.DupAttributeExistsAsync(dup_id, _entityType, id)) {
            var updatedDupPerson = await _dupService.UpdateDupPersonAsync(id, dupPersonContent);
            return updatedDupPerson != null
                ? Ok(SingleSuccessResponse(new List<DupPerson>() { updatedDupPerson }))
                : Ok(ErrorResponse("u", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified person, linked to a specified DUP
    ****************************************************************/

    [HttpDelete("data-uses/{dup_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> DeleteDupPerson(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExistsAsync(dup_id, _entityType, id)) {
            var count = await _dupService.DeleteDupPersonAsync(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dup_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
}