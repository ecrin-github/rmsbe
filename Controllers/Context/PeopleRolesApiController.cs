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
        _parType = "person"; _parIdType = "id"; _entityType = "PersonRole";
        _attType = "person role"; _attTypes = "person roles";
    }
    
    /****************************************************************
    * FETCH ALL roles for a specified person
    ****************************************************************/
    
    [HttpGet("people/{parId:int}/roles")]
    [SwaggerOperation(Tags = new []{"People roles endpoint"})]
    
    public async Task<IActionResult> GetPersonRoles(int parId)
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
    * FETCH the current role for a specified person (if one exists)
    ****************************************************************/
    
    [HttpGet("people/{parId:int}/current_roles")]
    [SwaggerOperation(Tags = new []{"People roles endpoint"})]
    
    public async Task<IActionResult> GetPersonCurrentRoles(int parId)
    {
        if (await _peopleService.PersonExistsAsync(parId)) {
            var personRole = await _peopleService.GetPersonCurrentRoleAsync(parId);
            return personRole != null
                ? Ok(SingleSuccessResponse(new List<PersonRole>() { personRole }))
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
     * CREATE a new role for a specified person
     ****************************************************************/
    
    [HttpPost("people/{parId:int}/roles")]
    [SwaggerOperation(Tags = new []{"People roles endpoint"})]
    
    // Only the role id is actually required in the body - the rest can be completed
    // using automatic defaults... 
    // If the person already has a current role should not be allowed
    
    public async Task<IActionResult> CreatePersonRole(int parId,
                 [FromBody] PersonRole personRoleContent)
    {
        if (await _peopleService.PersonExistsAsync(parId)) {
            if (await _peopleService.PersonHasNoCurrentRole(parId))
            {
                personRoleContent.PersonId = parId;
                personRoleContent.IsCurrent = true;
                personRoleContent.Granted = DateTime.Now;
                personRoleContent.Revoked = null;
                var newPeopleRole = await _peopleService.CreatePersonRoleAsync(personRoleContent);
                return newPeopleRole != null
                    ? Ok(SingleSuccessResponse(new List<PersonRole>() { newPeopleRole }))
                    : Ok(ErrorResponse("c", _attType, _parType, 
                        parId.ToString(), parId.ToString()));
            }
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
     * DELETE (=REVOKE) a single specified people role 
     ****************************************************************/
    
    [HttpPut("people/{parId:int}/roles/revoke/{id:int}")]
    [SwaggerOperation(Tags = new []{"People roles endpoint"})]
    
    // Creates a revocation date time and makes is_current = false
    
    public async Task<IActionResult> DeletePeopleRole(int parId, int id)
    {
        if (await _peopleService.PersonAttributeExistsAsync(parId, _entityType, id)) {
            var count = await _peopleService.RevokePersonRoleAsync(id);
            return count > 0
                    ? Ok(DeletionSuccessResponse(count, _attType, parId.ToString(), id.ToString()))
                    : Ok(ErrorResponse("d", _attType, _parType, parId.ToString(), id.ToString()));
        } 
        return Ok(NoParentAttResponse(_attType, _parType, parId.ToString(), id.ToString()));   
    }
}
