using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Context;

public class PeopleRolesApiController : BaseApiController
{
    private readonly IPeopleService _peopleService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public PeopleRolesApiController(IPeopleService peopleService)
    {
        _peopleService = peopleService ?? throw new ArgumentNullException(nameof(peopleService));
        _parType = "person"; _parIdType = "id"; _entityType = "PeopleRole";
        _attType = "person role"; _attTypes = "person roles";
    }
    
    /****************************************************************
    * FETCH ALL roles for a specified people
    ****************************************************************/
    
    [HttpGet("people/{parId:int}/roles")]
    [SwaggerOperation(Tags = new []{"People roles endpoint"})]
    
    public async Task<IActionResult> GetPeopleRoles(int parId)
    {
        if (await _peopleService.PersonExistsAsync(parId)) {
            var peopleRoles = await _peopleService.GetPersonRolesAsync(parId);
            return peopleRoles != null
                    ? Ok(ListSuccessResponse(peopleRoles.Count, peopleRoles))
                    : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, parId.ToString()));
    }
    
    /****************************************************************
    * FETCH A SINGLE people role 
    ****************************************************************/

    [HttpGet("people/{parId:int}/roles/{id:int}")]
    [SwaggerOperation(Tags = new[] { "People roles endpoint" })]

    public async Task<IActionResult> GetPersonRole(int parId, int id)
    {
        if (await _peopleService.PersonAttributeExistsAsync(parId, _entityType, id)) {
            var personRole = await _peopleService.GetPersonRoleAsync(id);
            return personRole != null
                    ? Ok(SingleSuccessResponse(new List<PersonRole>() { personRole }))
                    : Ok(ErrorResponse("r", _attType, _parType, parId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, parId.ToString(), id.ToString()));
    }

    /****************************************************************
     * CREATE a new role for a specified people
     ****************************************************************/
    
    [HttpPost("people/{parId:int}/roles")]
    [SwaggerOperation(Tags = new []{"People roles endpoint"})]
    
    public async Task<IActionResult> CreatePersonRole(int parId,
                 [FromBody] PersonRole personRoleContent)
    {
        if (await _peopleService.PersonExistsAsync(parId)) {
            personRoleContent.PersonId = parId;
            var newPeopleRole = await _peopleService.CreatePersonRoleAsync(personRoleContent);
            return newPeopleRole != null
                    ? Ok(SingleSuccessResponse(new List<PersonRole>() { newPeopleRole }))
                    : Ok(ErrorResponse("c", _attType, _parType, parId.ToString(), parId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, parId.ToString()));
    }
    
    /****************************************************************
     * UPDATE a single specified people role 
     ****************************************************************/
    
    [HttpPut("people/{parId:int}/roles/{id:int}")]
    [SwaggerOperation(Tags = new []{"People roles endpoint"})]
    
    public async Task<IActionResult> UpdatePersonRole(int parId, int id, 
                 [FromBody] PersonRole personRoleContent)
    {
        if (await _peopleService.PersonAttributeExistsAsync(parId, _entityType, id)) {
            var updatedPersonRole = await _peopleService.UpdatePersonRoleAsync(id, personRoleContent);
            return updatedPersonRole != null
                    ? Ok(SingleSuccessResponse(new List<PersonRole>() { updatedPersonRole }))
                    : Ok(ErrorResponse("u", _attType, _parType, parId.ToString(), id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, parId.ToString(), id.ToString()));
    }
 
    /****************************************************************
     * DELETE a single specified people role 
     ****************************************************************/
    
    [HttpDelete("people/{parId:int}/roles/{id:int}")]
    [SwaggerOperation(Tags = new []{"People roles endpoint"})]
    
    public async Task<IActionResult> DeletePeopleRole(int parId, int id)
    {
        if (await _peopleService.PersonAttributeExistsAsync(parId, _entityType, id)) {
            var count = await _peopleService.DeletePersonRoleAsync(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, parId.ToString(), id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, parId.ToString(), id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, parId.ToString(), id.ToString()));   
    }
}
