using Microsoft.AspNetCore.Mvc;
using rmsbe.Contracts;
using rmsbe.SysModels;
using Swashbuckle.AspNetCore.Annotations;

namespace rmsbe.Controllers;

public class CtxApiController : BaseApiController
{
    private readonly ICtxRepository _ctxRepository;

    public CtxApiController(ICtxRepository ctxRepository)
    {
        _ctxRepository = ctxRepository ?? throw new ArgumentNullException(nameof(ctxRepository));
    }


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

    

}