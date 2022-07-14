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
   
    [HttpGet("data-transfers/{dtp_id:int}/studies")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> GetDtpStudyList(int dtp_id)
    {
        if (await _dtpService.DtpExists(dtp_id)) {
            var dtpStudies = await _dtpService.GetAllDtpStudies(dtp_id);
            return dtpStudies != null
                ? Ok(ListSuccessResponse(dtpStudies.Count, dtpStudies))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtp_id.ToString()));    
    }

    /****************************************************************
    * FETCH a particular study linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> GetDtpStudy(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var dtpStudy = await _dtpService.GetDtpStudy(id);
            return dtpStudy != null
                ? Ok(SingleSuccessResponse(new List<DtpStudy>() { dtpStudy }))
                : Ok(ErrorResponse("r", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new study, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> CreateDtpStudy(int dtp_id, string sd_sid, 
           [FromBody] DtpStudy dtpStudyContent)
    {
        if (await _dtpService.DtpExists(dtp_id)) {
            dtpStudyContent.DtpId = dtp_id;
            dtpStudyContent.StudyId = sd_sid;
            var dtpStudy = await _dtpService.CreateDtpStudy(dtpStudyContent);
            return dtpStudy != null
                ? Ok(SingleSuccessResponse(new List<DtpStudy>() { dtpStudy }))
                : Ok(ErrorResponse("c", _attType, _parType, dtp_id.ToString(), dtp_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtp_id.ToString()));  
    }  
     
    /****************************************************************
    * UPDATE a study, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> UpdateDtpStudy(int dtp_id, int id, 
           [FromBody] DtpStudy dtpStudyContent)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var updatedDtpStudy = await _dtpService.UpdateDtpStudy(id, dtpStudyContent);
            return updatedDtpStudy != null
                ? Ok(SingleSuccessResponse(new List<DtpStudy>() { updatedDtpStudy }))
                : Ok(ErrorResponse("u", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified study, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> DeleteDtpStudy(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var count = await _dtpService.DeleteDtpStudy(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtp_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
}