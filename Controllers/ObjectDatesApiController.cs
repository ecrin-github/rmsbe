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
    public class ObjectDatesApiController : BaseApiController
    {
        private readonly IObjectRepository _dataObjectRepository;

        public ObjectDatesApiController(IObjectRepository objectRepository)
        {
            _dataObjectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        }
        
        [HttpGet("data-objects/{sd_oid}/dates")]
        [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
        public async Task<IActionResult> GetObjectDates(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objDates = await _dataObjectRepository.GetObjectDates(sd_oid);
            if (objDates == null)
                return Ok(new ApiResponse<ObjectDateDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No data object dates have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = objDates.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objDates
            });
        }
        
        [HttpGet("data-objects/{sd_oid}/dates/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
        public async Task<IActionResult> GetObjectDate(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objDate = await _dataObjectRepository.GetObjectDate(id);
            if (objDate == null) return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object dates have been found." },
                Data = null
            });

            var objDateList = new List<ObjectDateDto>() { objDate };
            return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = objDateList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objDateList
            });
        }

        [HttpPost("data-objects/{sd_oid}/dates")]
        [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
        public async Task<IActionResult> CreateObjectDate(string sd_oid,
            [FromBody] ObjectDateDto objectDateDto)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();
            
            objectDateDto.sd_oid ??= sd_oid;
            var objDate = await _dataObjectRepository.CreateObjectDate(objectDateDto, accessToken);
            if (objDate == null)
                return Ok(new ApiResponse<ObjectDateDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during object date creation." },
                    Data = null
                });

            var objDateList = new List<ObjectDateDto>() { objDate };
            return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = objDateList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objDateList
            });
        }

        [HttpPut("data-objects/{sd_oid}/dates/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
        public async Task<IActionResult> UpdateObjectDate(string sd_oid, int id, [FromBody] ObjectDateDto objectDateDto)
        {
            objectDateDto.Id ??= id;
            objectDateDto.sd_oid ??= sd_oid;
            
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objDate = await _dataObjectRepository.GetObjectDate(id);
            if (objDate == null) return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object dates have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedObjDate = await _dataObjectRepository.UpdateObjectDate(objectDateDto, accessToken);
            if (updatedObjDate == null)
                return Ok(new ApiResponse<ObjectDateDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during object date update." },
                    Data = null
                });

            var objDateList = new List<ObjectDateDto>() { updatedObjDate };
            return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = objDateList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objDateList
            });
        }
        
        [HttpDelete("data-objects/{sd_oid}/dates/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
        public async Task<IActionResult> DeleteObjectDate(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objDate = await _dataObjectRepository.GetObjectDate(id);
            if (objDate == null) return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object dates have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteObjectDate(id);
            return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Object date has been removed." },
                Data = null
            });
        }

        [HttpDelete("data-objects/{sd_oid}/dates")]
        [SwaggerOperation(Tags = new []{"Object dates endpoint"})]
        public async Task<IActionResult> DeleteAllObjectDates(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteAllObjectDates(sd_oid);
            return Ok(new ApiResponse<ObjectDateDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All object dates have been removed." },
                Data = null
            });
        }
    }
}