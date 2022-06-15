using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyFeaturesApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private OkObjectResult? _response;               // given a new value on each API call

    public StudyFeaturesApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }

    /****************************************************************
    * FETCH ALL features for a specified study
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/features")]
    [SwaggerOperation(Tags = new[] { "Study features endpoint" })]

    public async Task<IActionResult> GetStudyFeatures(string sd_sid) 
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) 
        {
            var studyFeatures = await _studyService.GetStudyFeaturesAsync(sd_sid);
            _response = (studyFeatures == null)
                     ? Ok(NoAttributesFoundResponse<StudyFeature>("No study features were found."))
                     : Ok(CollectionSuccessResponse(studyFeatures.Count, studyFeatures));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyFeature>());
        return _response;
    }
    
    /****************************************************************
    * FETCH A SINGLE study feature 
    ****************************************************************/

    [HttpGet("studies/{sd_sid}/features/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study features endpoint" })]
    
    public async Task<IActionResult> GetStudyFeature(string sd_sid, int id) 
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) 
        {
            var studyFeature = await _studyService.GetStudyFeatureAsync(id);
            _response = (studyFeature == null)
                ? Ok(NoAttributesResponse<StudyFeature>("No study feature with that id found."))
                : Ok(SingleSuccessResponse(new List<StudyFeature>() { studyFeature }));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyFeature>());
        return _response;
    }
    
    /****************************************************************
     * CREATE a new feature for a specified study
     ****************************************************************/

    [HttpPost("studies/{sd_sid}/features")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> CreateStudyFeature(string sd_sid, 
                 [FromBody] StudyFeature studyFeatureContent) 
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            studyFeatureContent.SdSid = sd_sid;
            var newStudyFeature = await _studyService.CreateStudyFeatureAsync(studyFeatureContent);
            _response = (newStudyFeature == null)
                ? Ok(ErrorInActionResponse<StudyFeature>("Error during study feature creation."))
                : Ok(SingleSuccessResponse(new List<StudyFeature>() { newStudyFeature }));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyFeature>());
        return _response;  
    }

    /****************************************************************
    * UPDATE a single specified study feature 
    ****************************************************************/

    [HttpPut("studies/{sd_sid}/features/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> UpdateStudyFeature(string sd_sid, int id,  
                 [FromBody] StudyFeature studyFeatureContent)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyFeature", id)) 
        {
            var updatedStudyFeature = await _studyService.UpdateStudyFeatureAsync(id, studyFeatureContent);
            _response = (updatedStudyFeature == null)
                ? Ok(ErrorInActionResponse<StudyFeature>("Error during study feature update."))
                : Ok(SingleSuccessResponse(new List<StudyFeature>() { updatedStudyFeature }));
        } 
        else 
            _response = Ok(MissingAttributeResponse<StudyFeature>("No feature with that id found for this study."));
        return _response;  
    }

    /****************************************************************
     * DELETE a single specified study feature 
     ****************************************************************/

    [HttpDelete("studies/{sd_sid}/features/{id:int}")]
    [SwaggerOperation(Tags = new []{"Study features endpoint"})]
    
    public async Task<IActionResult> DeleteStudyFeature(string sd_sid, int id)
    {
        if (await _studyService.StudyAttributeExistsAsync(sd_sid, "StudyFeature", id)) 
        {
            var count = await _studyService.DeleteStudyFeatureAsync(id);
            _response = (count == 0)
                ? Ok(ErrorInActionResponse<StudyFeature>("Deletion does not appear to have occured."))
                : Ok(DeletionSuccessResponse<StudyFeature>(count, $"Study feature {id.ToString()} removed."));
        } 
        else
            _response = Ok(MissingAttributeResponse<StudyFeature>("No feature with that id found for this study."));
        return _response;        
    }
}
