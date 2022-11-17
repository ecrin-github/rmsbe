using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpStudiesApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DtpStudiesApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "DTP"; _parIdType = "id"; _entityType = "DtpStudy";
        _attType = "DTP study"; _attTypes = "DTP studies";
    }
 
    /****************************************************************
    * FETCH ALL studies linked to a specified DTP
    ****************************************************************/
   
    [HttpGet("data-transfers/{dtpId:int}/studies")]
    [SwaggerOperation(Tags = new []{"DTP studies endpoint"})]
    
    public async Task<IActionResult> GetDtpStudyList(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            var dtpStudies = await _dtpService.GetAllDtpStudies(dtpId);
            return dtpStudies != null
                ? Ok(ListSuccessResponse(dtpStudies.Count, dtpStudies))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));    
    }
    
    /****************************************************************
    * FETCH ALL studies linked to a specified DTP, with foreign key names
    ****************************************************************/
   
    [HttpGet("data-transfers/with-fk-names/{dtpId:int}/studies")]
    [SwaggerOperation(Tags = new []{"DTP studies endpoint"})]
    
    public async Task<IActionResult> GetDtpStudyListWfn(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            var dtpStudiesWfn = await _dtpService.GetAllOutDtpStudies(dtpId);
            return dtpStudiesWfn != null
                ? Ok(ListSuccessResponse(dtpStudiesWfn.Count, dtpStudiesWfn))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));    
    }

    /****************************************************************
    * FETCH a particular study linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtpId:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP studies endpoint"})]
    
    public async Task<IActionResult> GetDtpStudy(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var dtpStudy = await _dtpService.GetDtpStudy(id);
            return dtpStudy != null
                ? Ok(SingleSuccessResponse(new List<DtpStudy>() { dtpStudy }))
                : Ok(ErrorResponse("r", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * FETCH a particular study linked to a specified DTP, with foreign key names
    ****************************************************************/

    [HttpGet("data-transfers/with-fk-names/{dtpId:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP studies endpoint"})]
    
    public async Task<IActionResult> GetDtpStudyWfn(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var dtpStudyWfn = await _dtpService.GetOutDtpStudy(id);
            return dtpStudyWfn != null
                ? Ok(SingleSuccessResponse(new List<DtpStudyOut>() { dtpStudyWfn }))
                : Ok(ErrorResponse("r", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new study, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtpId:int}/studies/{sdSid}")]
    [SwaggerOperation(Tags = new []{"DTP studies endpoint"})]
    
    public async Task<IActionResult> CreateDtpStudy(int dtpId, string sdSid, 
           [FromBody] DtpStudy dtpStudyContent)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            dtpStudyContent.DtpId = dtpId;   // ensure this is the case
            dtpStudyContent.SdSid = sdSid;
            var dtpStudy = await _dtpService.CreateDtpStudy(dtpStudyContent);
            return dtpStudy != null
                ? Ok(SingleSuccessResponse(new List<DtpStudy>() { dtpStudy }))
                : Ok(ErrorResponse("c", _attType, _parType, dtpId.ToString(), dtpId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));  
    }  
     
    /****************************************************************
    * UPDATE a study, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtpId:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP studies endpoint"})]
    
    public async Task<IActionResult> UpdateDtpStudy(int dtpId, int id, 
           [FromBody] DtpStudy dtpStudyContent)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            dtpStudyContent.DtpId = dtpId;  // ensure this is the case
            dtpStudyContent.Id = id;
            var updatedDtpStudy = await _dtpService.UpdateDtpStudy(dtpStudyContent);
            return updatedDtpStudy != null
                ? Ok(SingleSuccessResponse(new List<DtpStudy>() { updatedDtpStudy }))
                : Ok(ErrorResponse("u", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified study, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtpId:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP studies endpoint"})]
    
    public async Task<IActionResult> DeleteDtpStudy(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var count = await _dtpService.DeleteDtpStudy(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtpId.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
    
}