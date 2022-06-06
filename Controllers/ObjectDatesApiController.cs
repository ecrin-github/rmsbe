using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers
{
    public class ObjectDatesApiController : BaseApiController
    {
        private readonly IObjectDataService _objectService;

        public ObjectDatesApiController(IObjectDataService objectDataService)
        {
            _objectService = objectDataService ?? throw new ArgumentNullException(nameof(objectDataService));
        }

        
        [HttpGet("data-objects/{sd_oid}/dates")]
        [SwaggerOperation(Tags = new[] { "Object dates endpoint" })]
        public async Task<IActionResult> GetObjectDates(string sd_oid)
        {
            if (await _objectService.ObjectDoesNotExist(sd_oid))
            {
                return Ok(NoObjectResponse<ObjectDate>);
            }
            var objDates = await _objectService.GetObjectDatesAsync(sd_oid);
            if (objDates == null || objDates.Count == 0)
            {
                return Ok(NoAttributesResponse<ObjectDate>("No object dates were found."));
            }
            return Ok(new ApiResponse<ObjectDate>()
            {
                Total = objDates.Count, StatusCode = Ok().StatusCode, Messages = null,
                Data = objDates
            });
        }

        
        [HttpGet("data-objects/{sd_oid}/dates/{id:int}")]
        [SwaggerOperation(Tags = new[] { "Object dates endpoint" })]
        public async Task<IActionResult> GetObjectDate(string sd_oid, int id)
        {
            if (await _objectService.ObjectDoesNotExist(sd_oid))
            {
                return Ok(NoObjectResponse<ObjectDate>);
            }
            var objDate = await _objectService.GetObjectDateAsync(id);
            if (objDate == null)
            {
                return Ok(NoAttributesResponse<ObjectDate>("No object date with that id found."));
            }
            return Ok(new ApiResponse<ObjectDate>()
            {
                Total = 1, StatusCode = Ok().StatusCode, Messages = null,
                Data = new List<ObjectDate>() { objDate }
            });
        }


        [HttpPost("data-objects/{sd_oid}/dates")]
        [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
        public async Task<IActionResult> CreateObjectDate(string sd_oid, [FromBody] ObjectDate objDateContent)
        {
            if (await _objectService.ObjectDoesNotExist(sd_oid))
            {
                return Ok(NoObjectResponse<ObjectDate>);
            }
            objDateContent.SdOid = sd_oid; 
            var accessToken = (await HttpContext.GetTokenAsync("access_token"));
            var objDate = await _objectService.CreateObjectDateAsync(objDateContent, accessToken);
            if (objDate == null)
            {
                return Ok(ErrorInActionResponse<ObjectDate>("Error during object date creation."));
            }
            return Ok(new ApiResponse<ObjectDate>()
            {
                Total = 1, StatusCode = Ok().StatusCode, Messages = null,
                Data = new List<ObjectDate>() { objDate }
            });
        }  

        
        [HttpPut("data-objects/{sd_oid}/dates/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
        public async Task<IActionResult> UpdateObjectDate(string sd_oid, int id, [FromBody] ObjectDate objDateContent)
        {
            if (await _objectService.ObjectAttributeDoesNotExist(sd_oid, "ObjectDate", id))
            {
                return Ok(ErrorInActionResponse<ObjectDate>("No date with that id found for specified object."));
            }
            var accessToken = (await HttpContext.GetTokenAsync("access_token"));
            var updatedObjDate = await _objectService.UpdateObjectDateAsync(id, objDateContent, accessToken);
            if (updatedObjDate == null)
            {
                return Ok(ErrorInActionResponse<ObjectDate>("Error during object date update."));
            }
            return Ok(new ApiResponse<ObjectDate>()
            {
                Total = 1, StatusCode = Ok().StatusCode, Messages = null,
                Data = new List<ObjectDate>() { updatedObjDate }
            });
        }
        
        
        [HttpDelete("data-objects/{sd_oid}/dates/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
        public async Task<IActionResult> DeleteObjectDate(string sd_oid, int id)
        {
            if (await _objectService.ObjectAttributeDoesNotExist(sd_oid, "ObjectDate", id))
            {
                return Ok(ErrorInActionResponse<ObjectDate>("No date with that id found for specified object."));
            }
            var count = await _objectService.DeleteObjectDateAsync(id);
            return Ok(new ApiResponse<ObjectDate>()
            {
                Total = count, StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Object date has been removed." },
                Data = null
            });
        }
    }
}
