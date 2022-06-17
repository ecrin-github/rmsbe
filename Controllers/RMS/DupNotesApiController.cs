using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupNotesApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DupNotesApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _parType = "DUP"; _parIdType = "id"; _entityType = "DupNote";
        _attType = "DUP note"; _attTypes = "notes";
    }
 
    /****************************************************************
    * FETCH ALL notes linked to a specified DUP
    ****************************************************************/
   
    [HttpGet("data-uses/{dup_id:int}/notes")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> GetDupNoteList(int dup_id)
    {
        if (await _dupService.DupExistsAsync(dup_id)) {
            var dupNotes = await _dupService.GetAllDupNotesAsync(dup_id);
            return dupNotes != null
                ? Ok(ListSuccessResponse(dupNotes.Count, dupNotes))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));    
    }

    /****************************************************************
    * FETCH a particular note linked to a specified DUP
    ****************************************************************/

    [HttpGet("data-uses/{dup_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> GetDupNote(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExistsAsync(dup_id, _entityType, id)) {
            var dupNote = await _dupService.GetDupNoteAsync(id);
            return dupNote != null
                ? Ok(SingleSuccessResponse(new List<DupNote>() { dupNote }))
                : Ok(ErrorResponse("r", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new note, linked to a specified DUP
    ****************************************************************/

    [HttpPost("data-uses/{dup_id:int}/notes/{person_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> CreateDupNote(int dup_id, int person_id,
                 [FromBody] DupNote dupNoteContent)
    {
        if (await _dupService.DupExistsAsync(dup_id)) {
            dupNoteContent.DupId = dup_id;
            dupNoteContent.Author = person_id;
            var dupNote = await _dupService.CreateDupNoteAsync(dupNoteContent);
            return dupNote != null
                ? Ok(SingleSuccessResponse(new List<DupNote>() { dupNote }))
                : Ok(ErrorResponse("c", _attType, _parType, dup_id.ToString(), dup_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dup_id.ToString()));  
    }  

    /****************************************************************
    * UPDATE a note, linked to a specified DUP
    ****************************************************************/

    [HttpPut("data-uses/{dup_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> UpdateDupNote(int dup_id, int id, 
                 [FromBody] DupNote dupNoteContent)
    {
        if (await _dupService.DupAttributeExistsAsync(dup_id, _entityType, id)) {
            var updatedDupNote = await _dupService.UpdateDupNoteAsync(id, dupNoteContent);
            return updatedDupNote != null
                ? Ok(SingleSuccessResponse(new List<DupNote>() { updatedDupNote }))
                : Ok(ErrorResponse("u", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified note, linked to a specified DUP
    ****************************************************************/

    [HttpDelete("data-uses/{dup_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> DeleteDupNote(int dup_id, int id)
    {
        if (await _dupService.DupAttributeExistsAsync(dup_id, _entityType, id)) {
            var count = await _dupService.DeleteDupNoteAsync(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dup_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dup_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dup_id.ToString(), id.ToString()));
    }
}