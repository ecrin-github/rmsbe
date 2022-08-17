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
   
    [HttpGet("data-transfers/{dtpId:int}/notes")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> GetDtpNoteList(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId)) {
           var dtpNotes = await _dtpService.GetAllDtpNotes(dtpId);
           return dtpNotes != null
               ? Ok(ListSuccessResponse(dtpNotes.Count, dtpNotes))
               : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));    
    }
    
    /****************************************************************
    * FETCH a particular note linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtpId:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> GetDtpNote(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var dtpNote = await _dtpService.GetDtpNote(id);
            return dtpNote != null
                ? Ok(SingleSuccessResponse(new List<DtpNote>() { dtpNote }))
                : Ok(ErrorResponse("r", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }

    /****************************************************************
    * CREATE a new note, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtpId:int}/notes/{personId:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> CreateDtpNote(int dtpId, int personId,
                 [FromBody] DtpNote dtpNoteContent)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            dtpNoteContent.DtpId = dtpId;    // ensure this is the case
            dtpNoteContent.Author = personId;
            var dtpNote = await _dtpService.CreateDtpNote(dtpNoteContent);
            return dtpNote != null
                ? Ok(SingleSuccessResponse(new List<DtpNote>() { dtpNote }))
                : Ok(ErrorResponse("c", _attType, _parType, dtpId.ToString(), dtpId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));  
    }  

    /****************************************************************
    * UPDATE a note, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtpId:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> UpdateDtpNote(int dtpId, int id, 
           [FromBody] DtpNote dtpNoteContent)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            dtpNoteContent.DtpId = dtpId;  // ensure this is the case
            dtpNoteContent.Id = id;
            var updatedDtpNote = await _dtpService.UpdateDtpNote(dtpNoteContent);
            return updatedDtpNote != null
                ? Ok(SingleSuccessResponse(new List<DtpNote>() { updatedDtpNote }))
                : Ok(ErrorResponse("u", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified note, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtpId:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process notes endpoint"})]
    
    public async Task<IActionResult> DeleteDtpNote(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var count = await _dtpService.DeleteDtpNote(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtpId.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
}