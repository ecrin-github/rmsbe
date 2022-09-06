using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Context;

public class LupApiController : BaseApiController
{
    private readonly ILookupService _lookupService;

    public LupApiController(ILookupService lookupService)
    {
        _lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
    }

    [HttpGet("lookup/{typeName}/simple")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupValues(string typeName)
    {
        var lookups = await _lookupService.GetLookUpValues(typeName);
        return lookups.Count > 0 
            ? Ok(ListSuccessResponse(lookups.Count, lookups))
            : Ok(NoAttributesResponse(typeName));
    }
    
    [HttpGet("lookup/{typeName}/with-descs")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupValuesWithDescriptions(string typeName)
    {
        var lookups = await _lookupService.GetLookUpValuesWithDescs(typeName);
        return lookups.Count > 0 
            ? Ok(ListSuccessResponse(lookups.Count, lookups))
            : Ok(NoAttributesResponse(typeName));
    }

    
    [HttpGet("lookup/{typeName}/with-los")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupValuesWithListOrders(string typeName)
    {
        var lookups = await _lookupService.GetLookUpValuesWithListOrders(typeName);
        return lookups.Count > 0 
            ? Ok(ListSuccessResponse(lookups.Count, lookups))
            : Ok(NoAttributesResponse(typeName));
    }

    
    [HttpGet("lookup/{typeName}/with-descs-and-los")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupValuesWithDescsAndLos(string typeName)
    {
        var lookups = await _lookupService.GetLookUpValuesWithDescsAndLos(typeName);
        return lookups.Count > 0 
            ? Ok(ListSuccessResponse(lookups.Count, lookups))
            : Ok(NoAttributesResponse(typeName));
    }

    
    [HttpGet("lookup/{typeName}/code/{code:int}")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupTextDecode(string typeName, int code)
    {
        var decode = await _lookupService.GetLookUpTextDecode(typeName, code);
        return decode != null
            ? Ok(SingleSuccessResponse(new List<string?>() { decode }))
            : Ok(ErrorResponse("r", typeName, "", "", code.ToString()));
    }
    
    
    [HttpGet("lookup/{typeName}/decode/{decode}")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupValue(string typeName, string decode)
    {
        var code = await _lookupService.GetLookUpValue(typeName, decode);
        return code != null
            ? Ok(SingleSuccessResponse(new List<int?>() { code }))
            : Ok(ErrorResponse("r", typeName, "", "", decode));
    }

    
    /*

     contribution-types
     contribution-types-for-individuals
     contribution-types-for-organisations
     dataset-consent-types
     dataset-deidentification-types
     dataset-recordkey-types
     date-types
     description-types
     gender-eligibility-types
     identifier-types
     identifier-types-for-studies
     identifier-types-for-objects
     object-access-types
     object-classes
     object-filter-types
     object-relationship-types
     object-types
     object-types-text
     object-types-data
     object-types-other
     resource-types
     rms-user-types
     role-classes
     role-types
     size-units
     study-feature-types
     study-feature-categories
     study-feature-phase-categories
     study-feature-purpose-categories
     study-feature-allocation-categories
     study-feature-intervention-categories
     study-feature-masking-categories
     study-feature-obs_model-categories
     study-feature-obs_timeframe-categories
     study-feature-samples-categories
     study-relationship-types
     study-statuses
     study-types
     time-units
     title-types     
     title-types-for-studies
     title-types-for-objects
     topic-types
     topic-vocabularies
     
     check-status-types    
     dtp-status-types  
     dup-status-types    
     legal-status-types   
     prerequisite-types
     repo-access-types
     
     trial-registries

    */
}