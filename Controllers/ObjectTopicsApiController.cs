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
    public class ObjectTopicsApiController : BaseApiController
    {
        
        private readonly IObjectRepository _dataObjectRepository;

        public ObjectTopicsApiController(IObjectRepository objectRepository)
        {
            _dataObjectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        }
        
        
        [HttpGet("data-objects/{sd_oid}/topics")]
        [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
        public async Task<IActionResult> GetObjectTopics(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objTopics = await _dataObjectRepository.GetObjectTopics(sd_oid);
            if (objTopics == null)
                return Ok(new ApiResponse<ObjectTopicDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No data object topics have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = objTopics.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objTopics
            });
        }
        
        [HttpGet("data-objects/{sd_oid}/topics/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
        public async Task<IActionResult> GetObjectTopic(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objTopic = await _dataObjectRepository.GetObjectTopic(id);
            if (objTopic == null) return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object topics have been found." },
                Data = null
            });

            var objTopicList = new List<ObjectTopicDto>() { objTopic };
            return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = objTopicList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objTopicList
            });
        }

        [HttpPost("data-objects/{sd_oid}/topics")]
        [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
        public async Task<IActionResult> CreateObjectTopic(string sd_oid,
            [FromBody] ObjectTopicDto objectTopicDto)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            objectTopicDto.sd_oid ??= sd_oid;

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();
            
            var objTopic = await _dataObjectRepository.CreateObjectTopic(objectTopicDto, accessToken);
            if (objTopic == null) return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during object topic creation." },
                Data = null
            });

            var objTopicList = new List<ObjectTopicDto>() { objTopic };
            return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = objTopicList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objTopicList
            });
        }

        [HttpPut("data-objects/{sd_oid}/topics/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
        public async Task<IActionResult> UpdateObjectTopic(string sd_oid, int id, [FromBody] ObjectTopicDto objectTopicDto)
        {
            objectTopicDto.Id ??= id;
            objectTopicDto.sd_oid ??= sd_oid;
            
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objTopic = await _dataObjectRepository.GetObjectTopic(id);
            if (objTopic == null) return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object topics have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedObjectTopic = await _dataObjectRepository.UpdateObjectTopic(objectTopicDto, accessToken);
            if (updatedObjectTopic == null)
                return Ok(new ApiResponse<ObjectTopicDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during object topic update." },
                    Data = null
                });

            var objTopicList = new List<ObjectTopicDto>() { updatedObjectTopic };
            return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = objTopicList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objTopicList
            });
        }
        
        [HttpDelete("data-objects/{sd_oid}/topics/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
        public async Task<IActionResult> DeleteObjectTopic(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objTopic = await _dataObjectRepository.GetObjectTopic(id);
            if (objTopic == null) return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object topics have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteObjectTopic(id);
            return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Object topic has been removed." },
                Data = null
            });
        }

        [HttpDelete("data-objects/{sd_oid}/topics")]
        [SwaggerOperation(Tags = new []{"Object topics endpoint"})]
        public async Task<IActionResult> DeleteAllObjectTopics(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var count = await _dataObjectRepository.DeleteAllObjectTopics(sd_oid);
            return Ok(new ApiResponse<ObjectTopicDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All object topics have been removed." },
                Data = null
            });
        }
        
    }
}