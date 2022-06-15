using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyRelationshipsApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private OkObjectResult? _response;               // given a new value on each API call
    
    public StudyRelationshipsApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
    
    /****************************************************************
     * FETCH ALL relationships for a specified study
     ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/relationships")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> GetStudyRelationships(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            var studyRelationships = await _studyService.GetStudyRelationshipsAsync(sd_sid);
            _response = (studyRelationships == null)
                ? Ok(NoAttributesFoundResponse<StudyRelationship>("No study relationships were found."))
                : Ok(CollectionSuccessResponse(studyRelationships.Count, studyRelationships));
        }
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyRelationship>());
        return _response; 
    }
    
    /****************************************************************
     * FETCH A SINGLE study relationship 
     ****************************************************************/

    [HttpGet("studies/{sd_sid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study relationships endpoint" })]

    public async Task<IActionResult> GetStudyRelationship(string sd_sid, int id)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            var studyRel = await _studyService.GetStudyRelationshipAsync(id);
            _response = (studyRel == null)
                ? Ok(NoAttributesResponse<StudyRelationship>("No study relationship with that id found."))
                : Ok(SingleSuccessResponse(new List<StudyRelationship>() { studyRel }));
        }
        else
            _response = Ok(StudyDoesNotExistResponse<StudyRelationship>());
        return _response;
    }
    
    /****************************************************************
     * CREATE a new relationship for a specified study
     ****************************************************************/
    
    [HttpPost("studies/{sd_sid}/relationships")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> CreateStudyRelationship(string sd_sid, 
                 [FromBody] StudyRelationship studyRelationshipContent)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
             studyRelationshipContent.SdSid = sd_sid;
             var newStudyRel = await _studyService.CreateStudyRelationshipAsync(studyRelationshipContent);
             _response = (newStudyRel == null)
                 ? Ok(ErrorInActionResponse<StudyRelationship>("Error during study relationship creation."))
                 : Ok(SingleSuccessResponse(new List<StudyRelationship>() { newStudyRel }));
        }
        else
            _response = Ok(StudyDoesNotExistResponse<StudyRelationship>());
        return _response;
    }

     /****************************************************************
     * UPDATE a single specified study relationship 
     ****************************************************************/
     
    [HttpPut("studies/{sd_sid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study relationships endpoint" })]
    
    public async Task<IActionResult> UpdateStudyRelationship(string sd_sid, int id,
                 [FromBody] StudyRelationship studyRelContent)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyRelationship", id)) 
        {
            var updatedStudyRel = await _studyService.UpdateStudyRelationshipAsync(id, studyRelContent);
            _response = (updatedStudyRel == null)
                ? Ok(ErrorInActionResponse<StudyRelationship>("Error during study relationship update."))
                : Ok(SingleSuccessResponse(new List<StudyRelationship>() { updatedStudyRel }));
        } 
        else 
            _response = Ok(MissingAttributeResponse<StudyRelationship>("No relationship with that id found for this study."));
        return _response;  
    }

    /****************************************************************
     * DELETE a single specified study relationship 
     ****************************************************************/
    
    [HttpDelete("studies/{sd_sid}/relationships/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
    
    public async Task<IActionResult> DeleteStudyRelationship(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyRelationship", id)) 
        {
            var count = await _studyService.DeleteStudyRelationshipAsync(id);
            _response = (count == 0)
                ? Ok(ErrorInActionResponse<StudyRelationship>("Deletion does not appear to have occured."))
                : Ok(DeletionSuccessResponse<StudyRelationship>(count, $"Study relationship {id.ToString()} removed."));
        } 
        else
            _response = Ok(MissingAttributeResponse<StudyRelationship>("No relationship with that id found for this study."));
        return _response;        
    }
}
