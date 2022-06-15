using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

public class StudyApiController : BaseApiController
{
    private readonly IStudyService _studyService;
    private OkObjectResult? _response;               // given a new value on each API call

    public StudyApiController(IStudyService studyService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
    }
      
    /****************************************************************
    * FETCH data for a single study (including attribute data)
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> GetStudyById(string sd_sid)
    {
        var fullStudy = await _studyService.GetFullStudyByIdAsync(sd_sid);
        _response = (fullStudy == null)
            ? Ok(NoAttributesResponse<FullStudy>("No study found with that id."))
            : Ok(SingleSuccessResponse(new List<FullStudy>() { fullStudy }));
        return _response;
    }
    
    /****************************************************************
    * DELETE an entire study record (with attributes)
    ****************************************************************/

    [HttpDelete("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"Study endpoint"})]
    
    public async Task<IActionResult> DeleteStudy(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid)) 
        {
            var count = await _studyService.DeleteFullStudyAsync(sd_sid);
            _response = (count == 0)
                ? Ok(ErrorInActionResponse<FullStudy>("Deletion does not appear to have occured."))
                : Ok(DeletionSuccessResponse<FullStudy>(count, $"Full study {sd_sid} removed."));
        } 
        else
            _response = Ok(StudyDoesNotExistResponse<FullStudy>());
        return _response;        
    }
    
    /****************************************************************
    * FETCH ALL study records (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyData()
    {
        var allStudyRecordData = await _studyService.GetStudyRecordsDataAsync();
        _response = (allStudyRecordData == null)
            ? Ok(NoAttributesResponse<StudyData>("No studies found."))
            : Ok(CollectionSuccessResponse(allStudyRecordData.Count, allStudyRecordData));
        return _response;
    }

    /****************************************************************
    * FETCH n MOST RECENT study data (without attributes)
    ****************************************************************/
    
    [HttpGet("studies/data/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetRecentStudyData(int n)
    {
        var recentStudyData = await _studyService.GetRecentStudyRecordsAsync(n);
        _response = (recentStudyData == null)
            ? Ok(NoAttributesResponse<StudyData>("No studies found."))
            : Ok(CollectionSuccessResponse(recentStudyData.Count, recentStudyData));
        return _response;
    }
    
    /****************************************************************
    * FETCH single study record (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> GetStudyData(string sd_sid)
    {
        var study = await _studyService.GetStudyRecordDataAsync(sd_sid);
        _response = (study == null)
            ? Ok(NoAttributesResponse<StudyData>("No study found with that id."))
            : Ok(SingleSuccessResponse(new List<StudyData>() { study }));
        return _response;
    }
    
    /****************************************************************
    * CREATE a new study record (in studies table only)
    ****************************************************************/

    [HttpPost("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> CreateStudyData(string sd_sid, 
                 [FromBody] StudyData studyDataContent)
    {
        studyDataContent.SdSid = sd_sid;
        var newStudyData = await _studyService.CreateStudyRecordDataAsync(studyDataContent);
        _response = (newStudyData == null)
            ? Ok(ErrorInActionResponse<StudyData>("Error during study data creation."))
            : Ok(SingleSuccessResponse(new List<StudyData>() { newStudyData }));
        return _response;  
    }
    
    /****************************************************************
    * UPDATE a specified study record (in studies table only)
    ****************************************************************/

    [HttpPut("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new []{"Study data endpoint"})]
    
    public async Task<IActionResult> UpdateStudyData(string sd_sid, 
                 [FromBody] StudyData studyDataContent)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            var updatedStudyData = await _studyService.CreateStudyRecordDataAsync(studyDataContent);
            _response = (updatedStudyData == null)
                ? Ok(ErrorInActionResponse<StudyData>("Error during study data update."))
                : Ok(SingleSuccessResponse(new List<StudyData>() { updatedStudyData }));
        } 
        else 
            _response = Ok(StudyDoesNotExistResponse<StudyData>());
        return _response;  
    }
    
    /****************************************************************
    * DELETE a specified study record (from studies table only) 
    ****************************************************************/

    [HttpDelete("studies/{sd_sid}/data")]
    [SwaggerOperation(Tags = new[] { "Study data endpoint" })]

    public async Task<IActionResult> DeleteStudyData(string sd_sid)
    {
        if (await _studyService.StudyExistsAsync(sd_sid))
        {
            var count = await _studyService.DeleteStudyRecordDataAsync(sd_sid);
            _response = (count == 0)
                ? Ok(ErrorInActionResponse<StudyData>("Deletion does not appear to have occured."))
                : Ok(DeletionSuccessResponse<StudyData>(count, $"Study data record {sd_sid} removed."));
        } 
        else 
           _response = Ok(StudyDoesNotExistResponse<StudyData>());
        return _response;  
    }
}