using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Context;

public class LupApiController : BaseApiController
{
    private readonly ILookupService _lookupService;

    public LupApiController(ILookupService lookupService)
    {
        _lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
    }


    [HttpGet("lookup/{type_name}/simple")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupValues(string type_name)
    {
        var lookups = await _lookupService.GetLookUpValuesAsync(type_name);
        if (lookups == null || lookups.Count == 0)
        {
            return Ok(NoLupResponse<Lup>(type_name));
        }
        return Ok(new ApiResponse<Lup>
        {
            Total = lookups.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = lookups
        });
    }
 
    
    [HttpGet("lookup/{type_name}/with_descs")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupValuesWithDescriptions(string type_name)
    {
        var lookups = await _lookupService.GetLookUpValuesWithDescsAsync(type_name);
        if (lookups == null || lookups.Count == 0)
        {
            return Ok(NoLupResponse<LupWithDescription>(type_name));
        }
        return Ok(new ApiResponse<LupWithDescription>
        {
            Total = lookups.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = lookups
        });
    }

    
    [HttpGet("lookup/{type_name}/with_list_orders")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupValuesWithListOrders(string type_name)
    {
        var lookups = await _lookupService.GetLookUpValuesWithListOrdersAsync(type_name;
        if (lookups == null || lookups.Count == 0)
        {
            return Ok(NoLupResponse<LupWithListOrder>(type_name));
        }
        return Ok(new ApiResponse<LupWithListOrder>
        {
            Total = lookups.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = lookups
        });
    }

    
    [HttpGet("lookup/{type_name}/with_descs_and_los")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupValuesWithDescsAndLos(string type_name)
    {
        var lookups = await _lookupService.GetLookUpValuesWithDescsAndLosAsync(type_name);
        if (lookups == null || lookups.Count == 0)
        {
            return Ok(NoLupResponse<LupInSys>(type_name));
        }
        return Ok(new ApiResponse<LupFull>
        {
            Total = lookups.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = lookups
        });
    }

    
    [HttpGet("lookup/{type_name}/code/{code:int}")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupTextDecode(string type_name, int code)
    {
        var decode = await _lookupService.GetLookUpTextDecodeAsync(type_name, code);
        if (decode == null)
        {
            return Ok(NoLupDecode<string?>(type_name, code.ToString()));
        }    
        return Ok(new ApiResponse<string?>
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<string?> { decode },
        });
    }
    
    
    [HttpGet("lookup/{type_name}/decode/{decode}")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLookupValue(string type_name, string decode)
    {
        var code = await _lookupService.GetLookUpValueAsync(type_name, decode);
        if (code == null)
        {
            return Ok(NoLupCode<int?>(type_name, decode));
        }    
        return Ok(new ApiResponse<int?>
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<int?> { code },
        });
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
      
      
    [HttpGet("lang-codes")]
    [SwaggerOperation(Tags = new[] { "Context - Language codes" })]
    public async Task<IActionResult> Getlang_codes()
    {
        var data = await _lupRepository.GetLanguageCodes();
        if (data == null) return Ok(new ApiResponse<LanguageCode>()
        {
            Total = 0,
            Data = null,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>(){"There are no records."}
        });
        return Ok(new ApiResponse<LanguageCode>
        {
            Total = data.Count,
            Data = data,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }
    
    [HttpGet("lang-codes/{code}")]
    [SwaggerOperation(Tags = new[] { "Context - Language codes" })]
    public async Task<IActionResult> Getlang_code(string code)
    {
        var data = await _lupRepository.GetLanguageCode(code);
        if (data == null) return Ok(new ApiResponse<LanguageCode>()
        {
            Total = 0,
            Data = null,
            Messages = new List<string>(){"Not found."},
            StatusCode = NotFound().StatusCode
        });
        return Ok(new ApiResponse<LanguageCode>()
        {
            Total = 1,
            Data = new List<LanguageCode>(){data},
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }
    */
}