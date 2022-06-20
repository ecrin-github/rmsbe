using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Context;

public class CtxApiController : BaseApiController
{
    private readonly IContextService _contextService;
    public CtxApiController(IContextService contextService)
    {
        _contextService = contextService ?? throw new ArgumentNullException(nameof(contextService));
    }
    
    // Needs to provide
    // All organisations (id, name)
    // All organisation names (id, name, org id, default name)
    // All organisation names containing string (id, name, org id, default name)
    // single organisation, from org name id
    // single organisation, from org id
    // All names for organisation, by org id
    
  /*

    [HttpGet("organisations")]
    [SwaggerOperation(Tags = new[] { "Context - Organisations" })]
    public async Task<IActionResult> GetOrganisations()
    {
        var data = await _ctxRepository.GetOrganisations();
        if (data == null)
            return Ok(new ApiResponse<Organisation>
            {
                Total = 0,
                Data = null,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "There are no records." }
            });
        return Ok(new ApiResponse<Organisation>
        {
            Total = data.Count,
            Data = data,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }

    [HttpGet("organisations/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Context - Organisations" })]
    public async Task<IActionResult> GetOrganisation(int id)
    {
        var data = await _ctxRepository.GetOrganisation(id);
        if (data == null)
            return Ok(new ApiResponse<Organisation>
            {
                Total = 0,
                Data = null,
                Messages = new List<string> { "Not found." },
                StatusCode = NotFound().StatusCode
            });
        return Ok(new ApiResponse<Organisation>
        {
            Total = 1,
            Data = new List<Organisation> { data },
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }

    [HttpPost("organisations/search/by-title")]
    [SwaggerOperation(Tags = new[] { "Context - Organisations" })]
    public async Task<IActionResult> GetOrganisationsByName(SearchByTitleRequest searchByTitleRequest)
    {
        var data = await _ctxRepository.GetOrganisationsByName(searchByTitleRequest.organisation_name);
        if (data == null)
            return Ok(new ApiResponse<Organisation>
            {
                Total = 0,
                Data = null,
                Messages = new List<string> { "Not found." },
                StatusCode = NotFound().StatusCode
            });
        return Ok(new ApiResponse<Organisation>
        {
            Total = data.Count,
            Data = data,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }

    
    [HttpGet("organisations/{id:int}/names")]
    [SwaggerOperation(Tags = new[] { "Context - Organisations" })]
    public async Task<IActionResult> GetOrgNames(int id)
    {
        var data = await _ctxRepository.GetOrgNames(id);
        if (data == null)
            return Ok(new ApiResponse<OrgName>
            {
                Total = 0,
                Data = null,
                Messages = new List<string> { "Not found." },
                StatusCode = NotFound().StatusCode
            });
        return Ok(new ApiResponse<OrgName>
        {
            Total = data.Count,
            Data = data,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }
    */
    
    // language codes
    
    // geog entities???
    
}