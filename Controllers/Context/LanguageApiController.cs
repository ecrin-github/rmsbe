using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Context;

public class LanguageApiController : BaseApiController
{
    private readonly IContextService _contextService;
    private readonly string _attType, _attTypes;

    public LanguageApiController(IContextService contextService)
    {
        _contextService = contextService ?? throw new ArgumentNullException(nameof(contextService));
        _attType = "language"; _attTypes = "languages";
    }
    
    /****************************************************************
    * FETCH lang codes, names (by language, en, de, fr) en=default
    ****************************************************************/

    [HttpGet("lookup/langs/{nameLang?}")]
    [SwaggerOperation(Tags = new []{"Lookups endpoint"})]
    
    public async Task<IActionResult> GetLangCodes(string nameLang = "en")
    {
        var langCodes = await _contextService.GetLangCodes(nameLang);
        return langCodes != null
            ? Ok(ListSuccessResponse(langCodes.Count, langCodes))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH major lang codes, names (by language, en, de, fr) en=default
    ****************************************************************/
    
    [HttpGet("lookup/major-langs/{nameLang?}")]
    [SwaggerOperation(Tags = new []{"Lookups endpoint"})]
    
    public async Task<IActionResult> GetMajorLangCodes(string nameLang = "en")
    {
        var langCodes = await _contextService.GetMajorLangCodes(nameLang);
        return langCodes != null
            ? Ok(ListSuccessResponse(langCodes.Count, langCodes))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH lang details from code
    ****************************************************************/

    [HttpGet("lookup/lang/{code}")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLangDetsFromCode(string code)
    {
        if (await _contextService.LangCodeExists(code)) {
            var lang = await _contextService.GetLangDetailsFromCode(code);
            return lang != null
                ? Ok(SingleSuccessResponse(new List<LangDetails>() { lang }))
                : Ok(ErrorResponse("r", _attType, "", code, code));
        }
        return Ok(NoEntityResponse(_attType, code));
    }

    /****************************************************************
    * FETCH lang details from (lang name, name lang)
    ****************************************************************/

    [HttpGet("lookup/lang-from-name/{name}/{nameLang?}")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLangDetsFromName(string name, string nameLang = "en")
    {
        if (await _contextService.LangNameExists(name, nameLang)) {
            var lang = await _contextService.GetLangDetailsFromName(name, nameLang);
            return lang != null
                ? Ok(SingleSuccessResponse(new List<LangDetails>() { lang }))
                : Ok(ErrorResponse("r", _attType, "", name, name));
        }
        return Ok(NoEntityResponse(_attType, name));
    }
}