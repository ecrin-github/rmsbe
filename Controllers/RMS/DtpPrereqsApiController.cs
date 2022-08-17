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
    
    [HttpGet("data-transfers/{dtpId:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> GetDtpPrereqList(int dtpId, string sdOid)
    {
        if (await _dtpService.DtpObjectExists(dtpId, sdOid)) {
            var dtpPrereqs = await _dtpService.GetAllDtpPrereqs(dtpId, sdOid);
            return dtpPrereqs != null    
                ? Ok(ListSuccessResponse(dtpPrereqs.Count, dtpPrereqs))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }

    /****************************************************************
    * FETCH a particular pre-requisite record, for a specified object
    ****************************************************************/
    
    [HttpGet("data-transfers/{dtpId:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> GetDtpPrereq(int dtpId, string sdOid, int id)
    {
        if (await _dtpService.DtpObjectAttributeExists (dtpId, sdOid, _entityType, id)) {
            var dtpPrereq = await _dtpService.GetDtpPrereq(id);
            return dtpPrereq != null
                ? Ok(SingleSuccessResponse(new List<DtpPrereq>() { dtpPrereq }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new pre-requisite record, linked to a specified object
    ****************************************************************/
    
    [HttpPost("data-transfers/{dtpId:int}/objects/{sd_oid}/prereqs")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> CreateDtpPrereq(int dtpId, string sdOid, 
        [FromBody] DtpPrereq dtpPrereqContent)
    {
        if (await _dtpService.DtpObjectExists(dtpId, sdOid)) {
            dtpPrereqContent.DtpId = dtpId;   // ensure this is the case
            dtpPrereqContent.SdOid = sdOid;
            var dtpPrereq = await _dtpService.CreateDtpPrereq(dtpPrereqContent);
            return dtpPrereq != null
                ? Ok(SingleSuccessResponse(new List<DtpPrereq>() { dtpPrereq }))
                : Ok(ErrorResponse("c", _attType, _parType, dtpId.ToString(), dtpId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
     
    /****************************************************************
    * UPDATE a specific pre-requisite record details
    ****************************************************************/
    
    [HttpPut("data-transfers/{dtpId:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> UpdateDtpPrereq(int dtpId, string sdOid, int id, 
        [FromBody] DtpPrereq dtpPrereqContent)
    {
        if (await _dtpService.DtpObjectAttributeExists (dtpId, sdOid, _entityType, id)) {
            dtpPrereqContent.DtpId = dtpId;  // ensure this is the case
            dtpPrereqContent.SdOid = sdOid;
            dtpPrereqContent.Id = id;
            var updatedDtpPrereq = await _dtpService.UpdateDtpPrereq(dtpPrereqContent);
            return updatedDtpPrereq != null
                ? Ok(SingleSuccessResponse(new List<DtpPrereq>() { updatedDtpPrereq }))
                : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
   
    /****************************************************************
    * DELETE a specified pre-requisite record
    ****************************************************************/
    
    [HttpDelete("data-transfers/{dtpId:int}/objects/{sd_oid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process object pre-requisites endpoint"})]
    
    public async Task<IActionResult> DeleteDtpPrereq(int dtpId, string sdOid, int id)
    {
        if (await _dtpService.DtpObjectAttributeExists (dtpId, sdOid, _entityType, id)) {
            var count = await _dtpService.DeleteDtpPrereq(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}