using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class DtpStudiesApiController : BaseApiController
{
    private readonly IRmsService _rmsService;

    public DtpStudiesApiController(IRmsService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
 
    /****************************************************************
    * FETCH ALL studies linked to a specified DTP
    ****************************************************************/
   
    [HttpGet("data-transfers/{dtp_id:int}/studies")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> GetDtpStudyList(int dtp_id)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDTPResponse<DtpStudy>);
        }
        var dtpStudies = await _rmsService.GetAllDtpStudiesAsync(dtp_id);
        if (dtpStudies == null || dtpStudies.Count == 0)
        {
            return Ok(NoAttributesResponse<DtpStudy>("No study were found."));
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
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDTPResponse<DtpStudy>);
        }
        var dtpStudy = await _rmsService.GetDtpStudyAsync(id);
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

    [HttpPost("data-transfers/{dtp_id:int}/studies")]
    [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
    
    public async Task<IActionResult> CreateDtpStudy(int dtp_id, 
        [FromBody] DtpStudy dtpStudyContent)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDTPResponse<DtpStudy>);
        }
        dtpStudyContent.DtpId = dtp_id;
        var dtpStudy = await _rmsService.CreateDtpStudyAsync(dtpStudyContent);
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
        if (await _rmsService.DtpAttributeDoesNotExistAsync(dtp_id, "DTPStudy", id))
        {
            return Ok(ErrorInActionResponse<DtpStudy>("No study with that id found for specified DTP."));
        }
        var updatedDtpStudy = await _rmsService.UpdateDtpStudyAsync(id, dtpStudyContent);
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
        if (await _rmsService.DtpAttributeDoesNotExistAsync(dtp_id, "DTPStudy", id))
        {
            return Ok(ErrorInActionResponse<DtpStudy>("No study with that id found for specified DTP."));
        }
        var count = await _rmsService.DeleteDtpStudyAsync(id);
        return Ok(new ApiResponse<DtpStudy>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTP study has been removed."}, Data = null
        });
    }
}
