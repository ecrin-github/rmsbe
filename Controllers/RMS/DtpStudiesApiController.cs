using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpStudiesApiController : BaseApiController
{
    private readonly IDtpService _dtpService;

    public DtpStudiesApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
    }
 
    /****************************************************************
    * FETCH ALL studies linked to a specified DTP
    ****************************************************************/
   
    [HttpGet("data-transfers/{dtp_id:int}/studies")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> GetDtpStudyList(int dtp_id)
    {
        if (await _dtpService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<DtpStudy>());
        }
        var dtpStudies = await _dtpService.GetAllDtpStudiesAsync(dtp_id);
        if (dtpStudies == null || dtpStudies.Count == 0)
        {
            return Ok(NoAttributesResponse<DtpStudy>("No studies were found for the specified DTP."));
        }
        return Ok(new ApiResponse<DtpStudy>()
        {
            Total = dtpStudies.Count, StatusCode = Ok().StatusCode,Messages = null,
            Data = dtpStudies
        });
    }

    /****************************************************************
    * FETCH a particular study linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> GetDtpStudy(int dtp_id, int id)
    {
        if (await _dtpService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<DtpStudy>());
        }
        var dtpStudy = await _dtpService.GetDtpStudyAsync(id);
        if (dtpStudy == null) 
        {
            return Ok(NoAttributesResponse<DtpStudy>("No DTP study with that id found."));
        }        
        return Ok(new ApiResponse<DtpStudy>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpStudy>() { dtpStudy }
        });
    }

    /****************************************************************
    * CREATE a new study, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> CreateDtpStudy(int dtp_id, string sd_sid, 
           [FromBody] DtpStudy dtpStudyContent)
    {
        if (await _dtpService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<DtpStudy>());
        }
        dtpStudyContent.DtpId = dtp_id;
        dtpStudyContent.StudyId = sd_sid;
        var dtpStudy = await _dtpService.CreateDtpStudyAsync(dtpStudyContent);
        if (dtpStudy == null)
        {
            return Ok(ErrorInActionResponse<DtpStudy>("Error during DTP study creation."));
        }
        return Ok(new ApiResponse<DtpStudy>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null, 
            Data = new List<DtpStudy>() { dtpStudy }
        });
    }

    /****************************************************************
    * UPDATE a study, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> UpdateDtpStudy(int dtp_id, int id, 
           [FromBody] DtpStudy dtpStudyContent)
    {
        if (await _dtpService.DtpAttributeDoesNotExistAsync(dtp_id, "DTPStudy", id))
        {
            return Ok(ErrorInActionResponse<DtpStudy>("No study with that id found for specified DTP."));
        }
        var updatedDtpStudy = await _dtpService.UpdateDtpStudyAsync(id, dtpStudyContent);
        if (updatedDtpStudy == null)
        {
            return Ok(ErrorInActionResponse<DtpStudy>("Error during Dtp study update."));
        }        
        return Ok(new ApiResponse<DtpStudy>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpStudy>() { updatedDtpStudy }
        });
    }

    /****************************************************************
    * DELETE a specified study, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/studies/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> DeleteDtpStudy(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeDoesNotExistAsync(dtp_id, "DTPStudy", id))
        {
            return Ok(ErrorInActionResponse<DtpStudy>("No study with that id found for specified DTP."));
        }
        var count = await _dtpService.DeleteDtpStudyAsync(id);
        return Ok(new ApiResponse<DtpStudy>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTP study has been removed."}, Data = null
        });
    }
}
