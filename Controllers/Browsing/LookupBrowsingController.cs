using Microsoft.AspNetCore.Mvc;
using rmsbe.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace rmsbe.Controllers.Browsing;

public class LookupBrowsingController : BaseBrowsingApiController
{
    private readonly ILookupService _lookupService;

    public LookupBrowsingController(ILookupService lookupService)
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
}