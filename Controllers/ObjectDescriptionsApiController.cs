using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MdmService.Contracts.Responses;
using MdmService.DTO.Object;
using MdmService.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.Contracts;

namespace rmsbe.Controllers
{
    public class ObjectDescriptionsApiController : BaseApiController
    {
        private readonly IObjectRepository _dataObjectRepository;

        public ObjectDescriptionsApiController(IObjectRepository objectRepository)
        {
            _dataObjectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        }
        
        [HttpGet("data-objects/{sd_oid}/descriptions")]
        [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
        public async Task<IActionResult> GetObjectDescriptions(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objDescriptions = await _dataObjectRepository.GetObjectDescriptions(sd_oid);
            if (objDescriptions == null)
                return Ok(new ApiResponse<ObjectDescriptionDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No data object descriptions have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = objDescriptions.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objDescriptions
            });
        }
        
        [HttpGet("data-objects/{sd_oid}/descriptions/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
        public async Task<IActionResult> GetObjectDescription(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objDesc = await _dataObjectRepository.GetObjectDescription(id);
            if (objDesc == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object descriptions have been found." },
                Data = null
            });

            var objDescList = new List<ObjectDescriptionDto>() { objDesc };
            return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = objDescList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objDescList
            });
        }

        [HttpPost("data-objects/{sd_oid}/descriptions")]
        [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
        public async Task<IActionResult> CreateObjectDescription(string sd_oid,
            [FromBody] ObjectDescriptionDto objectDescriptionDto)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            objectDescriptionDto.sd_oid ??= sd_oid;
            var objDesc = await _dataObjectRepository.CreateObjectDescription(objectDescriptionDto, accessToken);
            if (objDesc == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during object description creation." },
                Data = null
            });

            var objDescList = new List<ObjectDescriptionDto>() { objDesc };
            return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = objDescList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objDescList
            });
        }

        [HttpPut("data-objects/{sd_oid}/descriptions/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
        public async Task<IActionResult> UpdateObjectDescription(string sd_oid, int id, [FromBody] ObjectDescriptionDto objectDescriptionDto)
        {
            objectDescriptionDto.Id ??= id;
            objectDescriptionDto.sd_oid ??= sd_oid;
            
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var objDesc = await _dataObjectRepository.GetObjectDescription(id);
            if (objDesc == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object descriptions have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedObjDesc = await _dataObjectRepository.UpdateObjectDescription(objectDescriptionDto, accessToken);
            if (updatedObjDesc == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during data object description update." },
                Data = null
            });

            var objDescList = new List<ObjectDescriptionDto>() { updatedObjDesc };
            return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = objDescList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objDescList
            });
        }
        
        [HttpDelete("data-objects/{sd_oid}/descriptions/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
        public async Task<IActionResult> DeleteObjectDescription(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var objDesc = await _dataObjectRepository.GetObjectDescription(id);
            if (objDesc == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object descriptions have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteObjectDescription(id);
            return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Object description has been removed." },
                Data = null
            });
        }

        [HttpDelete("data-objects/{sd_oid}/descriptions")]
        [SwaggerOperation(Tags = new []{"Object descriptions endpoint"})]
        public async Task<IActionResult> DeleteAllObjectDescriptions(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteAllObjectDescriptions(sd_oid);
            return Ok(new ApiResponse<ObjectDescriptionDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All object descriptions have been removed." },
                Data = null
            });
        }
        
    }
}