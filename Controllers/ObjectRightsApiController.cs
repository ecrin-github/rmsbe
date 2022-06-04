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
    public class ObjectRightsApiController : BaseApiController
    {
        
        private readonly IObjectRepository _dataObjectRepository;

        public ObjectRightsApiController(IObjectRepository objectRepository)
        {
            _dataObjectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        }
        
        
        [HttpGet("data-objects/{sd_oid}/rights")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        public async Task<IActionResult> GetObjectRights(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objRights = await _dataObjectRepository.GetObjectRights(sd_oid);
            if (objRights == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object rights have been found." },
                Data = null
            });
            
            return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = objRights.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objRights
            });
        }
        
        [HttpGet("data-objects/{sd_oid}/rights/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        public async Task<IActionResult> GetObjectRight(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objRight = await _dataObjectRepository.GetObjectRight(id);
            if (objRight == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object rights have been found." },
                Data = null
            });

            var objRightList = new List<ObjectRightDto>() { objRight };
            return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = objRightList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objRightList
            });
        }

        [HttpPost("data-objects/{sd_oid}/rights")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        public async Task<IActionResult> CreateObjectRight(string sd_oid,
            [FromBody] ObjectRightDto objectRightDto)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            objectRightDto.sd_oid ??= sd_oid;

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();
            
            var objRight = await _dataObjectRepository.CreateObjectRight(objectRightDto, accessToken);
            if (objRight == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during object right creation." },
                Data = null
            });

            var objRightList = new List<ObjectRightDto>() { objRight };
            return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = objRightList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objRightList
            });
        }

        [HttpPut("data-objects/{sd_oid}/rights/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        public async Task<IActionResult> UpdateObjectRight(string sd_oid, int id, [FromBody] ObjectRightDto objectRightDto)
        {
            objectRightDto.Id ??= id;
            objectRightDto.sd_oid ??= sd_oid;
            
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var objRight = await _dataObjectRepository.GetObjectRight(id);
            if (objRight == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object rights have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedObjRight = await _dataObjectRepository.UpdateObjectRight(objectRightDto, accessToken);
            if (updatedObjRight == null)
                return Ok(new ApiResponse<ObjectRightDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during object right update." },
                    Data = null
                });

            var objRightList = new List<ObjectRightDto>() { updatedObjRight };
            return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = objRightList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objRightList
            });
        }
        
        [HttpDelete("data-objects/{sd_oid}/rights/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        public async Task<IActionResult> DeleteObjectRight(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var objRight = await _dataObjectRepository.GetObjectRight(id);
            if (objRight == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object rights have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteObjectRight(id);
            return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Object right has been removed." },
                Data = null
            });
        }

        [HttpDelete("data-objects/{sd_oid}/rights")]
        [SwaggerOperation(Tags = new []{"Object rights endpoint"})]
        public async Task<IActionResult> DeleteAllObjectRights(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteAllObjectRights(sd_oid);
            return Ok(new ApiResponse<ObjectRightDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All object rights have been removed." },
                Data = null
            });
        }
        
    }
}