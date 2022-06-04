using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rmsbe.Controllers;
using MdmService.DTO.Object;
using MdmService.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.Contracts;

namespace rmsbe.Controllers
{
    public class ObjectApiController : BaseApiController
    {
        private readonly IObjectRepository _objectRepository;

        public ObjectApiController(IObjectRepository objectRepository)
        {
            _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        }
        
        [HttpGet("data-objects")]
        [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
        public async Task<IActionResult> GetAllDataObjects()
        {
            var dataObjects = await _objectRepository.GetAllDataObjects();
            if (dataObjects == null)
                return Ok(new ApiResponse<DataObjectDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No data objects have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<DataObjectDto>()
            {
                Total = dataObjects.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dataObjects
            });
        }
        
        [HttpGet("data-objects/{sd_oid}")]
        [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
        public async Task<IActionResult> GetObjectById(string sd_oid)
        {
            var dataObject = await _objectRepository.GetObjectById(sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<DataObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object has been found." },
                Data = null
            });

            var objectList = new List<DataObjectDto>() { dataObject };
            return Ok(new ApiResponse<DataObjectDto>()
            {
                Total = objectList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objectList
            });
        }

        [HttpPost("data-objects")]
        [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
        public async Task<IActionResult> CreateDataObject([FromBody] DataObjectDto dataObjectDto)
        {
            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var dataObj = await _objectRepository.CreateDataObject(dataObjectDto, accessToken);
            if (dataObj == null)
                return Ok(new ApiResponse<DataObjectDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during data object creation." },
                    Data = null
                });

            var dataObjList = new List<DataObjectDto>() { dataObj };
            return Ok(new ApiResponse<DataObjectDto>()
            {
                Total = dataObjList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dataObjList
            });
        }
        
        [HttpPut("data-objects/{sd_oid}")]
        [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
        public async Task<IActionResult> UpdateDataObject(string sd_oid, [FromBody] DataObjectDto dataObjectDto)
        {
            dataObjectDto.sd_oid ??= sd_oid;
            
            var dataObject = await _objectRepository.GetObjectById(dataObjectDto.sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<DataObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object has been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedDataObj = await _objectRepository.UpdateDataObject(dataObjectDto, accessToken);
            if (updatedDataObj == null)
                return Ok(new ApiResponse<DataObjectDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during data object update." },
                    Data = null
                });

            var dataObjList = new List<DataObjectDto>() { updatedDataObj };
            return Ok(new ApiResponse<DataObjectDto>()
            {
                Total = dataObjList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dataObjList
            });
        }

        [HttpDelete("data-objects/{sd_oid}")]
        [SwaggerOperation(Tags = new []{"Data objects endpoint"})]
        public async Task<IActionResult> DeleteDataObject(string sd_oid)
        {
            var dataObject = await _objectRepository.GetObjectById(sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<DataObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object has been found." },
                Data = null
            });
            
            var count = await _objectRepository.DeleteDataObject(sd_oid);
            return Ok(new ApiResponse<DataObjectDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Data object has been removed." },
                Data = null
            });
        }
    }
}