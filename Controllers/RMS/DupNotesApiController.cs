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
   
    [HttpGet("data-uses/{dupId:int}/notes")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> GetDupNoteList(int dupId)
    {
        if (await _dupService.DupExists(dupId)) {
            var dupNotes = await _dupService.GetAllDupNotes(dupId);
            return dupNotes != null
                ? Ok(ListSuccessResponse(dupNotes.Count, dupNotes))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));    
    }

    /****************************************************************
    * FETCH a particular note linked to a specified DUP
    ****************************************************************/

    [HttpGet("data-uses/{dupId:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> GetDupNote(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var dupNote = await _dupService.GetDupNote(id);
            return dupNote != null
                ? Ok(SingleSuccessResponse(new List<DupNote>() { dupNote }))
                : Ok(ErrorResponse("r", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new note, linked to a specified DUP
    ****************************************************************/

    [HttpPost("data-uses/{dupId:int}/notes/{person_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> CreateDupNote(int dupId, int personId,
                 [FromBody] DupNote dupNoteContent)
    {
        if (await _dupService.DupExists(dupId)) {
            dupNoteContent.DupId = dupId;
            dupNoteContent.Author = personId;
            var dupNote = await _dupService.CreateDupNote(dupNoteContent);
            return dupNote != null
                ? Ok(SingleSuccessResponse(new List<DupNote>() { dupNote }))
                : Ok(ErrorResponse("c", _attType, _parType, dupId.ToString(), dupId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dupId.ToString()));  
    }  

    /****************************************************************
    * UPDATE a note, linked to a specified DUP
    ****************************************************************/

    [HttpPut("data-uses/{dupId:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> UpdateDupNote(int dupId, int id, 
                 [FromBody] DupNote dupNoteContent)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var updatedDupNote = await _dupService.UpdateDupNote(id, dupNoteContent);
            return updatedDupNote != null
                ? Ok(SingleSuccessResponse(new List<DupNote>() { updatedDupNote }))
                : Ok(ErrorResponse("u", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified note, linked to a specified DUP
    ****************************************************************/

    [HttpDelete("data-uses/{dupId:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> DeleteDupNote(int dupId, int id)
    {
        if (await _dupService.DupAttributeExists(dupId, _entityType, id)) {
            var count = await _dupService.DeleteDupNote(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dupId.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dupId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dupId.ToString(), id.ToString()));
    }
}