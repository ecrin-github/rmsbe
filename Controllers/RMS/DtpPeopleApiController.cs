using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpPeopleApiController : BaseApiController
{
    private readonly IDtpService _dtpService;

    public DtpPeopleApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
    }
 
    /****************************************************************
    * FETCH ALL people linked to a specified DTP
    ****************************************************************/
   
    [HttpGet("data-transfers/{dtp_id:int}/people")]
    [SwaggerOperation(Tags = new []{"Data transfer process people endpoint"})]
    
    public async Task<IActionResult> GetDtpPeopleList(int dtp_id)
    {
        if (await _dtpService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<DtpPerson>());
        }
        var dtpPeople = await _dtpService.GetAllDtpPeopleAsync(dtp_id);
        if (dtpPeople == null || dtpPeople.Count == 0)
        {
            return Ok(NoAttributesResponse<DtpPerson>("No people were found for the specified DTP."));
        }
        return Ok(new ApiResponse<DtpPerson>()
        {
            Total = dtpPeople.Count, StatusCode = Ok().StatusCode,Messages = null,
            Data = dtpPeople
        });
    }

    /****************************************************************
    * FETCH a particular person linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process people endpoint"})]
    
    public async Task<IActionResult> GetDtpPerson(int dtp_id, int id)
    {
        if (await _dtpService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<DtpPerson>());
        }
        var dtpPerson = await _dtpService.GetDtpPersonAsync(id);
        if (dtpPerson == null) 
        {
            return Ok(NoAttributesResponse<DtpPerson>("No DTP person with that id found."));
        }        
        return Ok(new ApiResponse<DtpPerson>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpPerson>() { dtpPerson }
        });
    }

    /****************************************************************
    * CREATE a new person, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/people/{person_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process people endpoint"})]
    
    public async Task<IActionResult> CreateDtpPerson(int dtp_id, int person_id, 
           [FromBody] DtpPerson dtpPersonContent)
    {
        if (await _dtpService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<DtpPerson>());
        }
        dtpPersonContent.DtpId = dtp_id;
        dtpPersonContent.PersonId = person_id;
        var dtpPerson = await _dtpService.CreateDtpPersonAsync(dtpPersonContent);
        if (dtpPerson == null)
        {
            return Ok(ErrorInActionResponse<DtpPerson>("Error during DTP person creation."));
        }
        return Ok(new ApiResponse<DtpPerson>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null, 
            Data = new List<DtpPerson>() { dtpPerson }
        });
    }

    /****************************************************************
    * UPDATE a person, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process people endpoint"})]
    
    public async Task<IActionResult> UpdateDtpPerson(int dtp_id, int id, 
           [FromBody] DtpPerson dtpPersonContent)
    {
        if (await _dtpService.DtpAttributeDoesNotExistAsync(dtp_id, "DtpPerson", id))
        {
            return Ok(ErrorInActionResponse<DtpPerson>("No person with that id found for specified DTP."));
        }
        var updatedDtpPerson = await _dtpService.UpdateDtpPersonAsync(id, dtpPersonContent);
        if (updatedDtpPerson == null)
        {
            return Ok(ErrorInActionResponse<DtpPerson>("Error during Dtp person update."));
        }        
        return Ok(new ApiResponse<DtpPerson>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpPerson>() { updatedDtpPerson }
        });
    }

    /****************************************************************
    * DELETE a specified person, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process people endpoint"})]
    
    public async Task<IActionResult> DeleteDtpPerson(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeDoesNotExistAsync(dtp_id, "DtpPerson", id))
        {
            return Ok(ErrorInActionResponse<DtpPerson>("No person with that id found for specified DTP."));
        }
        var count = await _dtpService.DeleteDtpPersonAsync(id);
        return Ok(new ApiResponse<DtpPerson>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTP study has been removed."}, Data = null
        });
    }
}
