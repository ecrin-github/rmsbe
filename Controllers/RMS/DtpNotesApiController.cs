using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpNotesApiController : BaseApiController
{
    private readonly IRmsTransferService _rmsService;

    public DtpNotesApiController(IRmsTransferService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
 
    /****************************************************************
    * FETCH ALL notes linked to a specified DTP
    ****************************************************************/
   
    [HttpGet("data-transfers/{dtp_id:int}/notes")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> GetDtpNoteList(int dtp_id)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<DtpNote>());
        }
        var dtpNotes = await _rmsService.GetAllDtpNotesAsync(dtp_id);
        if (dtpNotes == null || dtpNotes.Count == 0)
        {
            return Ok(NoAttributesResponse<DtpNote>("No notes were found for the specified DTP."));
        }
        return Ok(new ApiResponse<DtpNote>()
        {
            Total = dtpNotes.Count, StatusCode = Ok().StatusCode,Messages = null,
            Data = dtpNotes
        });
    }

    /****************************************************************
    * FETCH a particular note linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> GetDtpNote(int dtp_id, int id)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<DtpNote>());
        }
        var dtpNote = await _rmsService.GetDtpNoteAsync(id);
        if (dtpNote == null) 
        {
            return Ok(NoAttributesResponse<DtpNote>("No DTP note with that id found."));
        }        
        return Ok(new ApiResponse<DtpNote>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpNote>() { dtpNote }
        });
    }

    /****************************************************************
    * CREATE a new note, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/notes/{person_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> CreateDtpNote(int dtp_id, int person_id,
           [FromBody] DtpNote dtpNoteContent)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDtpResponse<DtpNote>());
        }
        dtpNoteContent.DtpId = dtp_id;
        dtpNoteContent.Author = person_id;
        var dtpNote = await _rmsService.CreateDtpNoteAsync(dtpNoteContent);
        if (dtpNote == null)
        {
            return Ok(ErrorInActionResponse<DtpNote>("Error during DTP note creation."));
        }
        return Ok(new ApiResponse<DtpNote>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null, 
            Data = new List<DtpNote>() { dtpNote }
        });
    }

    /****************************************************************
    * UPDATE a note, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> UpdateDtpNote(int dtp_id, int id, 
           [FromBody] DtpNote dtpNoteContent)
    {
        if (await _rmsService.DtpAttributeDoesNotExistAsync(dtp_id, "DtpNote", id))
        {
            return Ok(ErrorInActionResponse<DtpNote>("No note with that id found for specified DTP."));
        }
        var updatedDtpNote = await _rmsService.UpdateDtpNoteAsync(id, dtpNoteContent);
        if (updatedDtpNote == null)
        {
            return Ok(ErrorInActionResponse<DtpNote>("Error during Dtp note update."));
        }        
        return Ok(new ApiResponse<DtpNote>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpNote>() { updatedDtpNote }
        });
    }

    /****************************************************************
    * DELETE a specified note, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> DeleteDtpNote(int dtp_id, int id)
    {
        if (await _rmsService.DtpAttributeDoesNotExistAsync(dtp_id, "DtpNote", id))
        {
            return Ok(ErrorInActionResponse<DtpNote>("No note with that id found for specified DTP."));
        }
        var count = await _rmsService.DeleteDtpNoteAsync(id);
        return Ok(new ApiResponse<DtpNote>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTP note has been removed."}, Data = null
        });
    }
}
