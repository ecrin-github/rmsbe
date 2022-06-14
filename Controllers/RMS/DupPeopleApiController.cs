using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupPeopleApiController : BaseApiController
{
    private readonly IDupService _dupService;

    public DupPeopleApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
    }
 
    /****************************************************************
    * FETCH ALL people linked to a specified DUP
    ****************************************************************/
   
    [HttpGet("data-uses/{dup_id:int}/people")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> GetDupPeopleList(int dup_id)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupPerson>());
        }
        var dupPeople = await _dupService.GetAllDupPeopleAsync(dup_id);
        if (dupPeople == null || dupPeople.Count == 0)
        {
            return Ok(NoAttributesResponse<DupPerson>("No people were found for the specified DUP."));
        }
        return Ok(new ApiResponse<DupPerson>()
        {
            Total = dupPeople.Count, StatusCode = Ok().StatusCode,Messages = null,
            Data = dupPeople
        });
    }

    /****************************************************************
    * FETCH a particular person linked to a specified DUP
    ****************************************************************/

    [HttpGet("data-uses/{dup_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> GetDupPerson(int dup_id, int id)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupPerson>());
        }
        var dupPerson = await _dupService.GetDupPersonAsync(id);
        if (dupPerson == null) 
        {
            return Ok(NoAttributesResponse<DupPerson>("No DUP person with that id found."));
        }        
        return Ok(new ApiResponse<DupPerson>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DupPerson>() { dupPerson }
        });
    }

    /****************************************************************
    * CREATE a new person, linked to a specified DUP
    ****************************************************************/

    [HttpPost("data-uses/{dup_id:int}/people/{person_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> CreateDupPerson(int dup_id, int person_id, 
           [FromBody] DupPerson dupPersonContent)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupPerson>());
        }
        dupPersonContent.DupId = dup_id;
        dupPersonContent.PersonId = person_id;
        var dupPerson = await _dupService.CreateDupPersonAsync(dupPersonContent);
        if (dupPerson == null)
        {
            return Ok(ErrorInActionResponse<DupPerson>("Error during DUP person creation."));
        }
        return Ok(new ApiResponse<DupPerson>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null, 
            Data = new List<DupPerson>() { dupPerson }
        });
    }

    /****************************************************************
    * UPDATE a person, linked to a specified DUP
    ****************************************************************/

    [HttpPut("data-uses/{dup_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> UpdateDupPerson(int dup_id, int id, 
           [FromBody] DupPerson dupPersonContent)
    {
        if (await _dupService.DupAttributeDoesNotExistAsync(dup_id, "DupPerson", id))
        {
            return Ok(ErrorInActionResponse<DupPerson>("No person with that id found for specified DUP."));
        }
        var updatedDupPerson = await _dupService.UpdateDupPersonAsync(id, dupPersonContent);
        if (updatedDupPerson == null)
        {
            return Ok(ErrorInActionResponse<DupPerson>("Error during Dup person update."));
        }        
        return Ok(new ApiResponse<DupPerson>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DupPerson>() { updatedDupPerson }
        });
    }

    /****************************************************************
    * DELETE a specified person, linked to a specified DUP
    ****************************************************************/

    [HttpDelete("data-uses/{dup_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process people endpoint"})]
    
    public async Task<IActionResult> DeleteDupPerson(int dup_id, int id)
    {
        if (await _dupService.DupAttributeDoesNotExistAsync(dup_id, "DupPerson", id))
        {
            return Ok(ErrorInActionResponse<DupPerson>("No person with that id found for specified DUP."));
        }
        var count = await _dupService.DeleteDupPersonAsync(id);
        return Ok(new ApiResponse<DupPerson>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DUP study has been removed."}, Data = null
        });
    }
}
