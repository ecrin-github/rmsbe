using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpNotesApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public DtpNotesApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "DTP"; _parIdType = "id"; _entityType = "DtpNote";
        _attType = "DTP note"; _attTypes = "notes";
    }
 
    /****************************************************************
    * FETCH ALL notes linked to a specified DTP
    ****************************************************************/
   
    [HttpGet("data-transfers/{dtp_id:int}/notes")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> GetDtpNoteList(int dtp_id)
    {
        if (await _dtpService.DtpExistsAsync(dtp_id)) {
           var dtpNotes = await _dtpService.GetAllDtpNotesAsync(dtp_id);
           return dtpNotes != null
               ? Ok(ListSuccessResponse(dtpNotes.Count, dtpNotes))
               : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtp_id.ToString()));    
    }
    
    /****************************************************************
    * FETCH a particular note linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> GetDtpNote(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeExistsAsync(dtp_id, _entityType, id)) {
            var dtpNote = await _dtpService.GetDtpNoteAsync(id);
            return dtpNote != null
                ? Ok(SingleSuccessResponse(new List<DtpNote>() { dtpNote }))
                : Ok(ErrorResponse("r", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }

    /****************************************************************
    * CREATE a new note, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/notes/{person_id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> CreateDtpNote(int dtp_id, int person_id,
                 [FromBody] DtpNote dtpNoteContent)
    {
        if (await _dtpService.DtpExistsAsync(dtp_id)) {
            dtpNoteContent.DtpId = dtp_id;
            dtpNoteContent.Author = person_id;
            var dtpNote = await _dtpService.CreateDtpNoteAsync(dtpNoteContent);
            return dtpNote != null
                ? Ok(SingleSuccessResponse(new List<DtpNote>() { dtpNote }))
                : Ok(ErrorResponse("c", _attType, _parType, dtp_id.ToString(), dtp_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtp_id.ToString()));  
    }  

    /****************************************************************
    * UPDATE a note, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> UpdateDtpNote(int dtp_id, int id, 
           [FromBody] DtpNote dtpNoteContent)
    {
        if (await _dtpService.DtpAttributeExistsAsync(dtp_id, _entityType, id)) {
            var updatedDtpNote = await _dtpService.UpdateDtpNoteAsync(id, dtpNoteContent);
            return updatedDtpNote != null
                ? Ok(SingleSuccessResponse(new List<DtpNote>() { updatedDtpNote }))
                : Ok(ErrorResponse("u", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified note, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> DeleteDtpNote(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeExistsAsync(dtp_id, _entityType, id)) {
            var count = await _dtpService.DeleteDtpNoteAsync(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtp_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
}