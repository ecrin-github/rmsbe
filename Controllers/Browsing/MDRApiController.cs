using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class MdrApiController : BaseBrowsingApiController
{
    private readonly IStudyService _studyService;
    private readonly IObjectService _objectService;
    private readonly string _attType, _fattType;
    
    /****************************************************************
    * This is a specialist controller
    * that is involved in obtaining full study details from the MDR
    * on receipt of a study registry id, and the id of the registry
    ****************************************************************/

    public MdrApiController(IStudyService studyService, IObjectService objectService)
    {
        _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _attType = "study"; _fattType = "full study"; 
    }
      
    
    /****************************************************************
    * FETCH single study record (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("studies/mdr/{regId:int}/{sdSid}/data")]
    [SwaggerOperation(Tags = new []{"Browsing - MDR endpoint"})]
    
    public async Task<IActionResult> GetStudyDataFromMdr(int regId, string sdSid)
    {
        var study = await _studyService.GetStudyFromMdr(regId, sdSid);
        if (study?.DisplayTitle == "EXISTING RMS STUDY")
        {
            return Ok(ExistingEntityResponse( _attType, sdSid));
        }
        else
        {
             return study != null
                        ? Ok(SingleSuccessResponse(new List<StudyData>() { study }))
                        : Ok(ErrorResponse(_attType, _attType, "", sdSid, sdSid));
        }
       

    }
    
    
    /****************************************************************
    * FETCH and STORE data for a single study
    * (including attribute data) but not linked object data
    ****************************************************************/
    
    [HttpGet("studies/mdr/{regId:int}/{sdSid}")]
    [SwaggerOperation(Tags = new []{"Browsing - MDR endpoint"})]
    
    public async Task<IActionResult> GetFullStudyFromMdr(int regId, string sdSid)
    {
  
        var fullStudyFromMdr = await _studyService.GetFullStudyFromMdr(regId, sdSid);
        
        /****************************************************************
        Temporary - 
        Whilst adding study and object data to the database for test purposes
        All of the objects as returned should be included as objects in the system.
        Normally this should be a user defined operation...i.e. they would
        select from a list provided by the system byu checking boxes or similar       
        ****************************************************************/
        /*
        if (fullStudyFromMdr?.DataObjectEntries != null 
            && fullStudyFromMdr.DataObjectEntries.Count > 0)
        {
            var dataObjects = fullStudyFromMdr.DataObjectEntries;
            // linkedObjects = List<DataObjectEntryInDb>
            foreach (var d in dataObjects)
            {
                if (d.SdSid != null) await _objectService.GetFullObjectFromMdr(d.SdSid, d.Id);
            }
        }
        */

        if (fullStudyFromMdr?.CoreStudy?.DisplayTitle == "EXISTING RMS STUDY")
        {
            return Ok(ExistingEntityResponse( _attType, sdSid));
        }
        else
        {
            return fullStudyFromMdr != null
                ? Ok(SingleSuccessResponse(new List<FullStudyFromMdr>() { fullStudyFromMdr }))
                : Ok(NoEntityResponse(_fattType, sdSid));
        }
    }
    
    
    /****************************************************************
    * FETCH and STORE data for a single object
    * (including attribute data) 
    ****************************************************************/
    
    [HttpGet("data-objects/mdr/{sdSid}/{mdrId:int}")]
    [SwaggerOperation(Tags = new []{"Browsing - MDR endpoint"})]
    
    public async Task<IActionResult> GetFullObjectFromMdr(string sdSid, int mdrId)
    {
        // The parameters are the sdSid of the study in the RMS,
        // and the integer id of the object in the MDR.
        // This is because the assumption is that this will only be used in the context
        // of importing a study (i.e. objects are not imported in isolation). In this
        // context the sdSid and mdr id will be as returned in the list of data
        // objects linked to the MDR study, and are thus available to be re-used here.
        
        var fullObjectFromMdr = await _objectService.GetFullObjectFromMdr(sdSid, mdrId);
        if (fullObjectFromMdr?.CoreObject?.DisplayTitle == "EXISTING RMS OBJECT")
        {
            return Ok(ExistingEntityResponse( "The Object", mdrId.ToString()));
        }
        return fullObjectFromMdr != null
            ? Ok(SingleSuccessResponse(new List<FullDataObject>() { fullObjectFromMdr }))
            : Ok(NoEntityResponse("full object", mdrId.ToString()));
    }
}