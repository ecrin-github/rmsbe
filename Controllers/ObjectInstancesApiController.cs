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
    public class ObjectInstancesApiController : BaseApiController
    {
        
        private readonly IObjectRepository _dataObjectRepository;

        public ObjectInstancesApiController(IObjectRepository objectRepository)
        {
            _dataObjectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        }
        
        
        [HttpGet("data-objects/{sd_oid}/instances")]
        [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
        public async Task<IActionResult> GetObjectInstances(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objInstances = await _dataObjectRepository.GetObjectInstances(sd_oid);
            if (objInstances == null)
                return Ok(new ApiResponse<ObjectInstanceDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No data object instances have been found." },
                    Data = null
                });

            return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = objInstances.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objInstances
            });
        }
        
        [HttpGet("data-objects/{sd_oid}/instances/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
        public async Task<IActionResult> GetObjectInstance(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objInstance = await _dataObjectRepository.GetObjectInstance(id);
            if (objInstance == null) return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object instances have been found." },
                Data = null
            });

            var objInstanceList = new List<ObjectInstanceDto>() { objInstance };
            return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = objInstanceList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objInstanceList
            });
        }

        [HttpPost("data-objects/{sd_oid}/instances")]
        [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
        public async Task<IActionResult> CreateObjectInstance(string sd_oid,
            [FromBody] ObjectInstanceDto objectInstanceDto)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            objectInstanceDto.sd_oid ??= sd_oid;
            var objInstance = await _dataObjectRepository.CreateObjectInstance(objectInstanceDto, accessToken);
            if (objInstance == null) return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during object instance creation." },
                Data = null
            });

            var objInstList = new List<ObjectInstanceDto>() { objInstance };
            return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = objInstList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objInstList
            });
        }

        [HttpPut("data-objects/{sd_oid}/instances/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
        public async Task<IActionResult> UpdateObjectInstance(string sd_oid, int id, [FromBody] ObjectInstanceDto objectInstanceDto)
        {
            objectInstanceDto.Id ??= id;
            objectInstanceDto.sd_oid ??= sd_oid;
            
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var objInstance = await _dataObjectRepository.GetObjectInstance(id);
            if (objInstance == null) return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object instances have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedObjInst = await _dataObjectRepository.UpdateObjectInstance(objectInstanceDto, accessToken);
            if (updatedObjInst == null)
                return Ok(new ApiResponse<ObjectInstanceDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during object instance update." },
                    Data = null
                });

            var objInstList = new List<ObjectInstanceDto>() { updatedObjInst };
            return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = objInstList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objInstList
            });
        }
        
        [HttpDelete("data-objects/{sd_oid}/instances/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
        public async Task<IActionResult> DeleteObjectInstance(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var objInstance = await _dataObjectRepository.GetObjectInstance(id);
            if (objInstance == null) return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object instances have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteObjectInstance(id);
            return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Object instance has been removed." },
                Data = null
            });
        }

        [HttpDelete("data-objects/{sd_oid}/instances")]
        [SwaggerOperation(Tags = new []{"Object instances endpoint"})]
        public async Task<IActionResult> DeleteAllObjectInstances(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteAllObjectInstances(sd_oid);
            return Ok(new ApiResponse<ObjectInstanceDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All object instances have been removed." },
                Data = null
            });
        }
        
    }
}