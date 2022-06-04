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
    public class object_identifiersApiController : BaseApiController
    {
        
        private readonly IObjectRepository _dataObjectRepository;

        public object_identifiersApiController(IObjectRepository objectRepository)
        {
            _dataObjectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        }
        
        
        [HttpGet("data-objects/{sd_oid}/identifiers")]
        [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
        public async Task<IActionResult> Getobject_identifiers(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objIdentifiers = await _dataObjectRepository.Getobject_identifiers(sd_oid);
            if (objIdentifiers == null)
                return Ok(new ApiResponse<object_identifierDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No data object identifiers have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = objIdentifiers.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objIdentifiers
            });
        }
        
        [HttpGet("data-objects/{sd_oid}/identifiers/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
        public async Task<IActionResult> Getobject_identifier(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objIdentifier = await _dataObjectRepository.Getobject_identifier(id);
            if (objIdentifier == null) return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object identifiers have been found." },
                Data = null
            });

            var objIdentList = new List<object_identifierDto>() { objIdentifier };
            return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = objIdentList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objIdentList
            });
        }

        [HttpPost("data-objects/{sd_oid}/identifiers")]
        [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
        public async Task<IActionResult> Createobject_identifier(string sd_oid,
            [FromBody] object_identifierDto object_identifierDto)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            object_identifierDto.sd_oid ??= sd_oid;
            var objIdent = await _dataObjectRepository.Createobject_identifier(object_identifierDto, accessToken);
            if (objIdent == null) return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during object identifier creation." },
                Data = null
            });

            var objIdentList = new List<object_identifierDto>() { objIdent };
            return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = objIdentList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objIdentList
            });
        }

        [HttpPut("data-objects/{sd_oid}/identifiers/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
        public async Task<IActionResult> Updateobject_identifier(string sd_oid, int id, [FromBody] object_identifierDto object_identifierDto)
        {
            object_identifierDto.Id ??= id;
            object_identifierDto.sd_oid ??= sd_oid;
            
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objIdentifier = await _dataObjectRepository.Getobject_identifier(id);
            if (objIdentifier == null) return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object identifiers have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedobject_identifier = await _dataObjectRepository.Updateobject_identifier(object_identifierDto, accessToken);
            if (updatedobject_identifier == null)
                return Ok(new ApiResponse<object_identifierDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during object identifier update." },
                    Data = null
                });

            var objIdentList = new List<object_identifierDto>() { updatedobject_identifier };
            return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = objIdentList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objIdentList
            });
        }
        
        [HttpDelete("data-objects/{sd_oid}/identifiers/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
        public async Task<IActionResult> Deleteobject_identifier(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objIdentifier = await _dataObjectRepository.Getobject_identifier(id);
            if (objIdentifier == null) return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object identifiers have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.Deleteobject_identifier(id);
            return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Object identifier has been removed." },
                Data = null
            });
        }

        [HttpDelete("data-object/{sd_oid}/identifiers")]
        [SwaggerOperation(Tags = new []{"Object identifiers endpoint"})]
        public async Task<IActionResult> DeleteAllobject_identifiers(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var count = await _dataObjectRepository.DeleteAllobject_identifiers(sd_oid);
            return Ok(new ApiResponse<object_identifierDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All object identifiers have been removed." },
                Data = null
            });
        }
        
    }
}