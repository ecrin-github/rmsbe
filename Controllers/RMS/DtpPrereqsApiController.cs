using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpPrereqsApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DtpPrereqsApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "Object"; _parIdType = "sd_oid"; _entityType = "DtpPrereq";
        _attType = "object prerequisite"; _attTypes = "object prerequisites"; 
    }

    /****************************************************************
    * FETCH ALL pre-requisite records, for a specified object / DTP
    ****************************************************************/
    
    [HttpGet("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> GetDtpPrereqList(int dtp_id, string sd_oid)
    {
        if (await _dtpService.DtpObjectExists(dtp_id, sd_oid)) {
            var dtpPrereqs = await _dtpService.GetAllDtpPrereqs(dtp_id, sd_oid);
            return dtpPrereqs != null    
                ? Ok(ListSuccessResponse(dtpPrereqs.Count, dtpPrereqs))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));    
    }

    /****************************************************************
    * FETCH a particular pre-requisite record, for a specified object
    ****************************************************************/
    
    [HttpGet("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> GetDtpPrereq(int dtp_id, string sd_oid, int id)
    {
        if (await _dtpService.DtpObjectAttributeExists (dtp_id, sd_oid, _entityType, id)) {
            var dtpPrereq = await _dtpService.GetDtpPrereq(id);
            return dtpPrereq != null
                ? Ok(SingleSuccessResponse(new List<DtpPrereq>() { dtpPrereq }))
                : Ok(ErrorResponse("r", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new pre-requisite record, linked to a specified object
    ****************************************************************/
    
    [HttpPost("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> CreateDtpPrereq(int dtp_id, string sd_oid, 
        [FromBody] DtpPrereq dtpPrereqContent)
    {
        if (await _dtpService.DtpObjectExists(dtp_id, sd_oid)) {
            dtpPrereqContent.DtpId = dtp_id;
            dtpPrereqContent.SdOid = sd_oid;
            var dtpPrereq = await _dtpService.CreateDtpPrereq(dtpPrereqContent);
            return dtpPrereq != null
                ? Ok(SingleSuccessResponse(new List<DtpPrereq>() { dtpPrereq }))
                : Ok(ErrorResponse("c", _attType, _parType, dtp_id.ToString(), dtp_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sd_oid));  
    }  
     
    /****************************************************************
    * UPDATE a specific pre-requisite record details
    ****************************************************************/
    
    [HttpPut("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> UpdateDtpPrereq(int dtp_id, string sd_oid, int id, 
        [FromBody] DtpPrereq dtpPrereqContent)
    {
        if (await _dtpService.DtpObjectAttributeExists (dtp_id, sd_oid, _entityType, id)) {
            var updatedDtpPrereq = await _dtpService.UpdateDtpPrereq(id, dtpPrereqContent);
            return updatedDtpPrereq != null
                ? Ok(SingleSuccessResponse(new List<DtpPrereq>() { updatedDtpPrereq }))
                : Ok(ErrorResponse("u", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
   
    /****************************************************************
    * DELETE a specified pre-requisite record
    ****************************************************************/
    
    [HttpDelete("data-transfers/{dtp_id:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> DeleteDtpPrereq(int dtp_id, string sd_oid, int id)
    {
        if (await _dtpService.DtpObjectAttributeExists (dtp_id, sd_oid, _entityType, id)) {
            var count = await _dtpService.DeleteDtpPrereq(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sd_oid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sd_oid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sd_oid, id.ToString()));
    }
}