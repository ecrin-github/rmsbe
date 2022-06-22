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

    [HttpGet("lookup/langs/{name_lang?}")]
    [SwaggerOperation(Tags = new []{"Lookups endpoint"})]
    
    public async Task<IActionResult> GetLangCodes(string name_lang = "en")
    {
        var langs = await _contextService.GetLangCodes(name_lang);
        return langs != null
            ? Ok(ListSuccessResponse(langs.Count, langs))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH major lang codes, names (by language, en, de, fr) en=default
    ****************************************************************/
    
    [HttpGet("lookup/majorlangs/{name_lang?}")]
    [SwaggerOperation(Tags = new []{"Lookups endpoint"})]
    
    public async Task<IActionResult> GetMajorLangCodes(string name_lang = "en")
    {
        var langs = await _contextService.GetMajorLangCodes(name_lang);
        return langs != null
            ? Ok(ListSuccessResponse(langs.Count, langs))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH lang details from code
    ****************************************************************/

    [HttpGet("lookup/lang/{code}")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLangDetsFromCode(string code)
    {
        if (await _contextService.LangCodeExistsAsync(code)) {
            var lang = await _contextService.GetLangDetailsFromCodeAsync(code);
            return lang != null
                ? Ok(SingleSuccessResponse(new List<LangDetails>() { lang }))
                : Ok(ErrorResponse("r", _attType, "", code, code));
        }
        return Ok(NoEntityResponse(_attType, code));
    }

    /****************************************************************
    * FETCH lang details from (lang name, name lang)
    ****************************************************************/

    [HttpGet("lookup/langfromname/{name}/{name_lang?}")]
    [SwaggerOperation(Tags = new[] { "Lookups endpoint" })]

    public async Task<IActionResult> GetLangDetsFromName(string name, string name_lang = "en")
    {
        if (await _contextService.LangNameExistsAsync(name, name_lang)) {
            var lang = await _contextService.GetLangDetailsFromNameAsync(name, name_lang);
            return lang != null
                ? Ok(SingleSuccessResponse(new List<LangDetails>() { lang }))
                : Ok(ErrorResponse("r", _attType, "", name, name));
        }
        return Ok(NoEntityResponse(_attType, name));
    }
}