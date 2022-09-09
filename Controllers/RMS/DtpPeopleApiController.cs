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
   
    [HttpGet("data-transfers/{dtpId:int}/people")]
    [SwaggerOperation(Tags = new []{"DTP people endpoint"})]
    
    public async Task<IActionResult> GetDtpPeopleList(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId)) {
           var dtpPeople = await _dtpService.GetAllDtpPeople(dtpId);
           return dtpPeople != null
               ? Ok(ListSuccessResponse(dtpPeople.Count, dtpPeople))
               : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));    
    }
    
    /****************************************************************
    * FETCH ALL people linked to a specified DTP, with foreign key names
    ****************************************************************/
   
    [HttpGet("data-transfers/with-fk-names/{dtpId:int}/people")]
    [SwaggerOperation(Tags = new []{"DTP people endpoint"})]
    
    public async Task<IActionResult> GetWfnDtpPeopleList(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            var dtpPeopleWfn = await _dtpService.GetAllOutDtpPeople(dtpId);
            return dtpPeopleWfn != null
                ? Ok(ListSuccessResponse(dtpPeopleWfn.Count, dtpPeopleWfn))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));    
    }

    /****************************************************************
    * FETCH a particular person linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtpId:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP people endpoint"})]
    
    public async Task<IActionResult> GetDtpPerson(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var dtpPerson = await _dtpService.GetDtpPerson(id);
            return dtpPerson != null
                ? Ok(SingleSuccessResponse(new List<DtpPerson>() { dtpPerson }))
                : Ok(ErrorResponse("r", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
     
    /****************************************************************
    * FETCH a particular person linked to a specified DTP, with foreign key names
    ****************************************************************/

    [HttpGet("data-transfers/with-fk-names/{dtpId:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP people endpoint"})]
    
    public async Task<IActionResult> GetWfnDtpPerson(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var dtpPersonWfn = await _dtpService.GetOutDtpPerson(id);
            return dtpPersonWfn != null
                ? Ok(SingleSuccessResponse(new List<DtpPersonOut>() { dtpPersonWfn }))
                : Ok(ErrorResponse("r", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new person, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtpId:int}/people/{personId:int}")]
    [SwaggerOperation(Tags = new []{"DTP people endpoint"})]
    
    public async Task<IActionResult> CreateDtpPerson(int dtpId, int personId, 
           [FromBody] DtpPerson dtpPersonContent)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            dtpPersonContent.DtpId = dtpId;   // ensure this is the case
            dtpPersonContent.PersonId = personId;
            var dtpPerson = await _dtpService.CreateDtpPerson(dtpPersonContent);
            return dtpPerson != null
                ? Ok(SingleSuccessResponse(new List<DtpPerson>() { dtpPerson }))
                : Ok(ErrorResponse("c", _attType, _parType, dtpId.ToString(), dtpId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));  
    }  
    
    /****************************************************************
    * UPDATE a person, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtpId:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP people endpoint"})]
    
    public async Task<IActionResult> UpdateDtpPerson(int dtpId, int id, 
           [FromBody] DtpPerson dtpPersonContent)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            dtpPersonContent.DtpId = dtpId;  // ensure this is the case
            dtpPersonContent.Id = id;
            var updatedDtpPerson = await _dtpService.UpdateDtpPerson(dtpPersonContent);
            return updatedDtpPerson != null
                ? Ok(SingleSuccessResponse(new List<DtpPerson>() { updatedDtpPerson }))
                : Ok(ErrorResponse("u", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
     
    /****************************************************************
    * DELETE a specified person, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtpId:int}/people/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP people endpoint"})]
    
    public async Task<IActionResult> DeleteDtpPerson(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var count = await _dtpService.DeleteDtpPerson(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtpId.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
}
