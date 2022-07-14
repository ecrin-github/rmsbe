using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpPeopleApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DtpPeopleApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "DTP"; _parIdType = "id"; _entityType = "DtpPerson";
        _attType = "DTP person"; _attTypes = "DTP people";
    }
 
    /****************************************************************
    * FETCH ALL people linked to a specified DTP
    ****************************************************************/
   
    [HttpGet("data-transfers/{dtp_id:int}/people")]
    [SwaggerOperation(Tags = new []{"Data transfer process people endpoint"})]
    
    public async Task<IActionResult> GetDtpPeopleList(int dtp_id)
    {
        if (await _dtpService.DtpExists(dtp_id)) {
           var dtpPeople = await _dtpService.GetAllDtpPeople(dtp_id);
           return dtpPeople != null
               ? Ok(ListSuccessResponse(dtpPeople.Count, dtpPeople))
               : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtp_id.ToString()));    
    }

    /****************************************************************
    * FETCH a particular person linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process people endpoint"})]
    
    public async Task<IActionResult> GetDtpPerson(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var dtpPerson = await _dtpService.GetDtpPerson(id);
            return dtpPerson != null
                ? Ok(SingleSuccessResponse(new List<DtpPerson>() { dtpPerson }))
                : Ok(ErrorResponse("r", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
     
    /****************************************************************
    * CREATE a new person, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/people/{person_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process people endpoint"})]
    
    public async Task<IActionResult> CreateDtpPerson(int dtp_id, int person_id, 
           [FromBody] DtpPerson dtpPersonContent)
    {
        if (await _dtpService.DtpExists(dtp_id)) {
            dtpPersonContent.DtpId = dtp_id;
            dtpPersonContent.PersonId = person_id;
            var dtpPerson = await _dtpService.CreateDtpPerson(dtpPersonContent);
            return dtpPerson != null
                ? Ok(SingleSuccessResponse(new List<DtpPerson>() { dtpPerson }))
                : Ok(ErrorResponse("c", _attType, _parType, dtp_id.ToString(), dtp_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtp_id.ToString()));  
    }  
    
    /****************************************************************
    * UPDATE a person, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process people endpoint"})]
    
    public async Task<IActionResult> UpdateDtpPerson(int dtp_id, int id, 
           [FromBody] DtpPerson dtpPersonContent)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var updatedDtpPerson = await _dtpService.UpdateDtpPerson(id, dtpPersonContent);
            return updatedDtpPerson != null
                ? Ok(SingleSuccessResponse(new List<DtpPerson>() { updatedDtpPerson }))
                : Ok(ErrorResponse("u", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
     
    /****************************************************************
    * DELETE a specified person, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process people endpoint"})]
    
    public async Task<IActionResult> DeleteDtpPerson(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var count = await _dtpService.DeleteDtpPerson(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtp_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
}
