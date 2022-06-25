using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Context;

public class OrgApiController : BaseApiController
{
    private readonly IContextService _contextService;
    private readonly string _attType, _attTypes;

    public OrgApiController(IContextService contextService)
    {
        _contextService = contextService ?? throw new ArgumentNullException(nameof(contextService));
        _attType = "organisation"; _attTypes = "organisations";
    }
    
    /****************************************************************
    * FETCH All organisations list (id, default_name)
    ****************************************************************/
    
    [HttpGet("context/organisations")]
    [SwaggerOperation(Tags = new[] {"Context-Organisations"})]

    public async Task<IActionResult> GetOrganisations()
    {
        var allOrgs = await _contextService.GetOrgs();
        return allOrgs != null
            ? Ok(ListSuccessResponse(allOrgs.Count, allOrgs))
            : Ok(NoAttributesResponse(_attTypes));
    }

    /****************************************************************
    * FETCH filtered organisations list (id, default_name)
    ****************************************************************/
    
    [HttpGet("context/organisations/{filter}")]
    [SwaggerOperation(Tags = new[] {"Context-Organisations"})]

    public async Task<IActionResult> GetFilteredOrganisations(string filter)
    {
        var filteredOrgs = await _contextService.GetFilteredOrgs(filter);
        return filteredOrgs != null
            ? Ok(ListSuccessResponse(filteredOrgs.Count, filteredOrgs))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH All org name list (id, name from (all) org names)
    ****************************************************************/
    
    [HttpGet("context/orgnames")]
    [SwaggerOperation(Tags = new[] {"Context-Organisations"})]

    public async Task<IActionResult> GetOrgNames()
    {
        var allOrgNames = await _contextService.GetOrgNames();
        return allOrgNames != null
            ? Ok(ListSuccessResponse(allOrgNames.Count, allOrgNames))
            : Ok(NoAttributesResponse(_attTypes));
    }

    /****************************************************************
    * FETCH filtered org name list (id, name from (all) org names)
    ****************************************************************/
    
    [HttpGet("context/orgnames/{filter}")]
    [SwaggerOperation(Tags = new[] {"Context-Organisations"})]

    public async Task<IActionResult> GetFilteredOrgNames(string filter)
    {
        var filteredOrgNames = await _contextService.GetFilteredOrgNames(filter);
        return filteredOrgNames != null
            ? Ok(ListSuccessResponse(filteredOrgNames.Count, filteredOrgNames))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH single org details, from org id
    ****************************************************************/
    /*
     to do...
          
    [HttpGet("organisations/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Context - Organisations" })]

    public async Task<IActionResult> GetOrganisationDetails(int id)
    {
        if (await _contextService.OrgExistsAsync(id)) {
            var org = await _contextService.GetOrgByOrgId(id);
            return org != null
                ? Ok(SingleSuccessResponse(new List<Organisation>() { org }))
                : Ok(ErrorResponse("r", _attType, "", id.ToString(), id.ToString()));
        }
        return Ok(NoEntityResponse(_attType, id.ToString()));
    }
    */

    // All names for organisation, by org id
    // Task<List<string>?> GetSingleOrgNames(int org_id);

}