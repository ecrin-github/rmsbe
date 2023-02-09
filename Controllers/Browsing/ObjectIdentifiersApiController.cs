using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Browsing;

public class ObjectIdentifiersApiController : BaseBrowsingApiController
{
    private readonly IObjectService _objectService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public ObjectIdentifiersApiController(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        _parType = "data object"; _parIdType = "sd_oid"; _entityType = "ObjectIdentifier";
        _attType = "object identifier"; _attTypes = "object identifiers";
    }
    
    /****************************************************************
    * FETCH ALL identifiers for a specified object
    ****************************************************************/

    [HttpGet("data-objects/{sdOid}/identifiers")]
    [SwaggerOperation(Tags = new []{"Browsing - Object identifiers endpoint"})]
    
    public async Task<IActionResult> GetObjectIdentifiers(string sdOid)
    {
        if (await _objectService.ObjectExists(sdOid)) {
            var objIdentifiers = await _objectService.GetObjectIdentifiers(sdOid);
            return objIdentifiers != null
                ? Ok(ListSuccessResponse(objIdentifiers.Count, objIdentifiers))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }

    /****************************************************************
    * FETCH A SINGLE object identifier
    ****************************************************************/
    
    [HttpGet("data-objects/{sdOid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new []{"Browsing - Object identifiers endpoint"})]
    
    public async Task<IActionResult> GetObjectIdentifier(string sdOid, int id)
    {
        if (await _objectService.ObjectAttributeExists(sdOid, _entityType, id)) {
            var objIdentifier = await _objectService.GetObjectIdentifier(id);
            return objIdentifier != null
                ? Ok(SingleSuccessResponse(new List<ObjectIdentifier>() { objIdentifier }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}