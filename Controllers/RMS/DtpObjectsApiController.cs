using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpObjectsApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DtpObjectsApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "DTP"; _parIdType = "id"; _entityType = "DtpObject";
        _attType = "DTP object"; _attTypes = "DTP objects";
    }
    
    /****************************************************************
    * FETCH ALL objects linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/objects")]
    [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
    
    public async Task<IActionResult> GetDtpObjectList(int dtp_id)
    {
        if (await _dtpService.DtpExists(dtp_id)) {
            var dtpObjects = await _dtpService.GetAllDtpObjects(dtp_id);
            return dtpObjects != null
                ? Ok(ListSuccessResponse(dtpObjects.Count, dtpObjects))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtp_id.ToString()));    
    }
    
    /****************************************************************
    * FETCH a particular object, linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
    
    public async Task<IActionResult> GetDtpObject(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var dtpObj = await _dtpService.GetDtpObject(id);
            return dtpObj != null
                ? Ok(SingleSuccessResponse(new List<DtpObject>() { dtpObj }))
                : Ok(ErrorResponse("r", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
   
    /****************************************************************
    * CREATE a new object, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
    
    public async Task<IActionResult> CreateDtpObject(int dtp_id, string sd_oid,
           [FromBody] DtpObject dtpObjectContent)
    {
        if (await _dtpService.DtpExists(dtp_id)) {
            dtpObjectContent.DtpId = dtp_id;
            dtpObjectContent.ObjectId = sd_oid;
            var dtpObj = await _dtpService.CreateDtpObject(dtpObjectContent);
            return dtpObj != null
                ? Ok(SingleSuccessResponse(new List<DtpObject>() { dtpObj }))
                : Ok(ErrorResponse("c", _attType, _parType, dtp_id.ToString(), dtp_id.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtp_id.ToString()));  
    }  
    
    /****************************************************************
    * UPDATE an object, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
    
    public async Task<IActionResult> UpdateDtpObject(int dtp_id, int id, 
        [FromBody] DtpObject dtpObjectContent)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var updatedDtpObject = await _dtpService.UpdateDtpObject(id, dtpObjectContent);
            return updatedDtpObject != null
                ? Ok(SingleSuccessResponse(new List<DtpObject>() { updatedDtpObject }))
                : Ok(ErrorResponse("u", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }

    /****************************************************************
    * DELETE a specified object, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
    
    public async Task<IActionResult> DeleteDtpObject(int dtp_id, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtp_id, _entityType, id)) {
            var count = await _dtpService.DeleteDtpObject(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtp_id.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtp_id.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtp_id.ToString(), id.ToString()));
    }
}