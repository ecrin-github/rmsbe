using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupNotesApiController : BaseApiController
{
    private readonly IDupService _dupService;

    public DupNotesApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
    }
 
    /****************************************************************
    * FETCH ALL notes linked to a specified DUP
    ****************************************************************/
   
    [HttpGet("data-uses/{dup_id:int}/notes")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> GetDupNoteList(int dup_id)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupNote>());
        }
        var dupNotes = await _dupService.GetAllDupNotesAsync(dup_id);
        if (dupNotes == null || dupNotes.Count == 0)
        {
            return Ok(NoAttributesResponse<DupNote>("No notes were found for the specified DUP."));
        }
        return Ok(new ApiResponse<DupNote>()
        {
            Total = dupNotes.Count, StatusCode = Ok().StatusCode,Messages = null,
            Data = dupNotes
        });
    }

    /****************************************************************
    * FETCH a particular note linked to a specified DUP
    ****************************************************************/

    [HttpGet("data-uses/{dup_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> GetDupNote(int dup_id, int id)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupNote>());
        }
        var dupNote = await _dupService.GetDupNoteAsync(id);
        if (dupNote == null) 
        {
            return Ok(NoAttributesResponse<DupNote>("No DUP note with that id found."));
        }        
        return Ok(new ApiResponse<DupNote>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DupNote>() { dupNote }
        });
    }

    /****************************************************************
    * CREATE a new note, linked to a specified DUP
    ****************************************************************/

    [HttpPost("data-uses/{dup_id:int}/notes/{person_id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> CreateDupNote(int dup_id, int person_id,
           [FromBody] DupNote dupNoteContent)
    {
        if (await _dupService.DupDoesNotExistAsync(dup_id))
        {
            return Ok(NoDupResponse<DupNote>());
        }
        dupNoteContent.DupId = dup_id;
        dupNoteContent.Author = person_id;
        var dupNote = await _dupService.CreateDupNoteAsync(dupNoteContent);
        if (dupNote == null)
        {
            return Ok(ErrorInActionResponse<DupNote>("Error during DUP note creation."));
        }
        return Ok(new ApiResponse<DupNote>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null, 
            Data = new List<DupNote>() { dupNote }
        });
    }

    /****************************************************************
    * UPDATE a note, linked to a specified DUP
    ****************************************************************/

    [HttpPut("data-uses/{dup_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> UpdateDupNote(int dup_id, int id, 
           [FromBody] DupNote dupNoteContent)
    {
        if (await _dupService.DupAttributeDoesNotExistAsync(dup_id, "DupNote", id))
        {
            return Ok(ErrorInActionResponse<DupNote>("No note with that id found for specified DUP."));
        }
        var updatedDupNote = await _dupService.UpdateDupNoteAsync(id, dupNoteContent);
        if (updatedDupNote == null)
        {
            return Ok(ErrorInActionResponse<DupNote>("Error during Dup note update."));
        }        
        return Ok(new ApiResponse<DupNote>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DupNote>() { updatedDupNote }
        });
    }

    /****************************************************************
    * DELETE a specified note, linked to a specified DUP
    ****************************************************************/

    [HttpDelete("data-uses/{dup_id:int}/notes/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data use process notes endpoint"})]
    
    public async Task<IActionResult> DeleteDupNote(int dup_id, int id)
    {
        if (await _dupService.DupAttributeDoesNotExistAsync(dup_id, "DupNote", id))
        {
            return Ok(ErrorInActionResponse<DupNote>("No note with that id found for specified DUP."));
        }
        var count = await _dupService.DeleteDupNoteAsync(id);
        return Ok(new ApiResponse<DupNote>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DUP note has been removed."}, Data = null
        });
    }
}
