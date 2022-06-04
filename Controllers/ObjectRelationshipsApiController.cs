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
    public class ObjectRelationshipsApiController : BaseApiController
    {
        
        private readonly IObjectRepository _dataObjectRepository;

        public ObjectRelationshipsApiController(IObjectRepository objectRepository)
        {
            _dataObjectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        }
        
        
        [HttpGet("data-objects/{sd_oid}/relationships")]
        [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
        public async Task<IActionResult> GetObjectRelationships(string sd_oid)
        {
            var dataObject = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objRel = await _dataObjectRepository.GetObjectRelationships(sd_oid);
            if (objRel == null)
                return Ok(new ApiResponse<ObjectRelationshipDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No data object relationships have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = objRel.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objRel
            });
        }
        
        [HttpGet("data-objects/{sd_oid}/relationships/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
        public async Task<IActionResult> GetObjectRelationship(string sd_oid, int id)
        {
            var dataObject = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objRel = await _dataObjectRepository.GetObjectRelationship(id);
            if (objRel == null) return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object relationships have been found." },
                Data = null
            });

            var objRelList = new List<ObjectRelationshipDto>() { objRel };
            return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = objRelList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objRelList
            });
        }

        [HttpPost("data-objects/{sd_oid}/relationships")]
        [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
        public async Task<IActionResult> CreateObjectRelationship(string sd_oid,
            [FromBody] ObjectRelationshipDto objectRelationshipDto)
        {
            var dataObject = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            objectRelationshipDto.sd_oid ??= sd_oid;
            var objRel = await _dataObjectRepository.CreateObjectRelationship(objectRelationshipDto, accessToken);
            if (objRel == null)
                return Ok(new ApiResponse<ObjectRelationshipDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during object relationship creation." },
                    Data = null
                });

            var objRelList = new List<ObjectRelationshipDto>() { objRel };
            return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = objRelList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objRelList
            });
        }

        [HttpPut("data-objects/{sd_oid}/relationships/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
        public async Task<IActionResult> UpdateObjectRelationship(string sd_oid, int id, [FromBody] ObjectRelationshipDto objectRelationshipDto)
        {
            objectRelationshipDto.Id ??= id;
            objectRelationshipDto.sd_oid ??= sd_oid;
            
            var dataObject = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var objRel = _dataObjectRepository.GetObjectRelationship(id);
            if (objRel == null) return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object relationships have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedObjectRel = await _dataObjectRepository.UpdateObjectRelationship(objectRelationshipDto, accessToken);
            if (updatedObjectRel == null) return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during object relationship update." },
                Data = null
            });

            var objRelList = new List<ObjectRelationshipDto>() { updatedObjectRel };
            return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = objRelList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objRelList
            });
        }
        
        [HttpDelete("data-objects/{sd_oid}/relationships/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
        public async Task<IActionResult> DeleteObjectRelationship(string sd_oid, int id)
        {
            var dataObject = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var objRel = await _dataObjectRepository.GetObjectRelationship(id);
            if (objRel == null) return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object relationships have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteObjectRelationship(id);
            return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Object relationship has been removed." },
                Data = null
            });
        }

        [HttpDelete("data-objects/{sd_oid}/relationships")]
        [SwaggerOperation(Tags = new []{"Object relationships endpoint"})]
        public async Task<IActionResult> DeleteAllObjectRelationships(string sd_oid)
        {
            var dataObject = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteAllObjectRelationships(sd_oid);
            return Ok(new ApiResponse<ObjectRelationshipDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All object relationships have been removed." },
                Data = null
            });
        }
        
    }
}