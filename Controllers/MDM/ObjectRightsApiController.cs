using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.MDM;

    public class ObjectRightsApiController : BaseApiController
    {
        private readonly IObjectService _objectService;

        public ObjectRightsApiController(IObjectService objectService)
        {
            _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        }
        
        /****************************************************************
        * FETCH ALL rights for a specified object
        ****************************************************************/
        
        [HttpGet("data-objects/{sd_oid}/rights")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        
        public async Task<IActionResult> GetObjectRights(string sd_oid)
        {
            if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
            {
                return Ok(NoObjectResponse<ObjectRight>());
            }
            var objRights = await _objectService.GetObjectRightsAsync(sd_oid);
            if (objRights == null|| objRights.Count == 0)
            {
                return Ok(NoAttributesResponse<ObjectRight>("No object rights were found."));
            }
            return Ok(new ApiResponse<ObjectRight>()
            {
                Total = objRights.Count, StatusCode = Ok().StatusCode, Messages = null,
                Data = objRights
            });
        }
        
        /****************************************************************
        * FETCH A SINGLE object right
        ****************************************************************/
        
        [HttpGet("data-objects/{sd_oid}/rights/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        public async Task<IActionResult> GetObjectRight(string sd_oid, int id)
        {
            if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
            {
                return Ok(NoObjectResponse<ObjectRight>());
            }

            var objRight = await _objectService.GetObjectRightAsync(id);
            if (objRight == null) 
            {
                return Ok(NoAttributesResponse<ObjectRight>("No object right with that id found."));
            }    
            return Ok(new ApiResponse<ObjectRight>()
            {
                Total = 1, StatusCode = Ok().StatusCode, Messages = null,
                Data = new List<ObjectRight>() { objRight }
            });
        }

        /****************************************************************
        * CREATE a new right for a specified object
        ****************************************************************/
        
        [HttpPost("data-objects/{sd_oid}/rights")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        
        public async Task<IActionResult> CreateObjectRight(string sd_oid,
            [FromBody] ObjectRight objectRightContent)
        {
            if (await _objectService.ObjectDoesNotExistAsync(sd_oid))
            {
                return Ok(NoObjectResponse<ObjectRight>());
            }
            objectRightContent.SdOid = sd_oid;
            var objRight = await _objectService.CreateObjectRightAsync(objectRightContent);
            if (objRight == null) 
            {
                return Ok(ErrorInActionResponse<ObjectRight>("Error during object right creation."));
            }    
            return Ok(new ApiResponse<ObjectRight>()
            {
                Total = 1, StatusCode = Ok().StatusCode, Messages = null,
                Data = new List<ObjectRight>() { objRight }
            });
        }
            
        /****************************************************************
        * UPDATE a single specified object right
        ****************************************************************/
        
        [HttpPut("data-objects/{sd_oid}/rights/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        
        public async Task<IActionResult> UpdateObjectRight(string sd_oid, int id, 
            [FromBody] ObjectRight objectRightContent)
        {
            if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectRight", id))
            {
                return Ok(ErrorInActionResponse<ObjectRight>("No right with that id found for specified object."));
            }
            var updatedObjRight = await _objectService.UpdateObjectRightAsync(id, objectRightContent);
            if (updatedObjRight == null)
            {
                return Ok(ErrorInActionResponse<ObjectRight>("Error during object right update."));
            }    
            return Ok(new ApiResponse<ObjectRight>()
            {
                Total = 1, StatusCode = Ok().StatusCode, Messages = null,
                Data = new List<ObjectRight>() { updatedObjRight }
            });
        }
        
        /****************************************************************
        * DELETE a single specified object right
        ****************************************************************/
        
        [HttpDelete("data-objects/{sd_oid}/rights/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        
        public async Task<IActionResult> DeleteObjectRight(string sd_oid, int id)
        {
            if (await _objectService.ObjectAttributeDoesNotExistAsync(sd_oid, "ObjectRight", id))
            {
                return Ok(ErrorInActionResponse<ObjectRight>("No right with that id found for specified object."));
            }
            var count = await _objectService.DeleteObjectRightAsync(id);
            return Ok(new ApiResponse<ObjectRight>()
            {
                Total = count, StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Object right has been removed." }, Data = null
            });
        }
    }
