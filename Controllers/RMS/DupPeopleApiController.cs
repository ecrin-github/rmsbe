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
   
    [HttpGet("data-uses/{dupId:int}/people")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> GetDupPeopleList(int dupId)
    {
        if (await _dupService.DupExists(dupId)) {
            var dupPeople = await _dupService.GetAllDupPeople(dupId);
            return dupPeople != null
                ? Ok(ListSuccessResponse(dupPeople.Count, dupPeople))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));    
    }
     
    /****************************************************************
    * FETCH a particular person linked to a specified DUP
    ****************************************************************/

    [HttpGet("data-uses/{dupId:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> GetDupPerson(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var dupPerson = await _dupService.GetDupPerson(id);
            return dupPerson != null
                ? Ok(SingleSuccessResponse(new List<DupPerson>() { dupPerson }))
                : Ok(ErrorResponse("r", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new person, linked to a specified DUP
    ****************************************************************/

    [HttpPost("data-uses/{dupId:int}/people/{person_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> CreateDupPerson(int dupId, int personId, 
                 [FromBody] DupPerson dupPersonContent)
    {
        if (await _dupService.DupExists(dupId)) {
            dupPersonContent.DupId = dupId;   // ensure this is the case
            dupPersonContent.PersonId = personId;
            var dupPerson = await _dupService.CreateDupPerson(dupPersonContent);
            return dupPerson != null
                ? Ok(SingleSuccessResponse(new List<DupPerson>() { dupPerson }))
                : Ok(ErrorResponse("c", _attType, _parType, dupId.ToString(), dupId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));  
    }  
     
    /****************************************************************
    * UPDATE a person, linked to a specified DUP
    ****************************************************************/

    [HttpPut("data-uses/{dupId:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> UpdateDupPerson(int dupId, int id, 
                 [FromBody] DupPerson dupPersonContent)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            dupPersonContent.DupId = dupId;  // ensure this is the case
            dupPersonContent.Id = id;
            var updatedDupPerson = await _dupService.UpdateDupPerson(dupPersonContent);
            return updatedDupPerson != null
                ? Ok(SingleSuccessResponse(new List<DupPerson>() { updatedDupPerson }))
                : Ok(ErrorResponse("u", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified person, linked to a specified DUP
    ****************************************************************/

    [HttpDelete("data-uses/{dupId:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> DeleteDupPerson(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var count = await _dupService.DeleteDupPerson(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dupId.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
}