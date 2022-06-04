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
    public class ObjectDatasetsApiController : BaseApiController
    {
        
        private readonly IObjectRepository _dataObjectRepository;

        public ObjectDatasetsApiController(IObjectRepository objectRepository)
        {
            _dataObjectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        }
        
        
        [HttpGet("data-objects/{sd_oid}/datasets")]
        [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
        public async Task<IActionResult> GetObjectDatasets(string sd_oid)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objDatasets = await _dataObjectRepository.GetObjectDatasets(sd_oid);
            if (objDatasets == null)
                return Ok(new ApiResponse<ObjectDatasetDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No data object datasets have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = 1,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = new List<ObjectDatasetDto>(){objDatasets}
            });
        }
        
        
        [HttpGet("data-objects/{sd_oid}/datasets/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
        public async Task<IActionResult> GetObjectDatasets(string sd_oid, int id)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objDataset = await _dataObjectRepository.GetObjectDataset(id);
            if (objDataset == null) return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects datasets have been found." },
                Data = null
            });

            var objDatasetList = new List<ObjectDatasetDto>() { objDataset };
            return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = objDatasetList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objDatasetList
            });
        }
        

        [HttpPost("data-objects/{sd_oid}/datasets")]
        [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
        public async Task<IActionResult> CreateObjectDataset(string sd_oid,
            [FromBody] ObjectDatasetDto objectDatasetDto)
        {
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();
            
            objectDatasetDto.sd_oid ??= sd_oid;
            var objDataset = await _dataObjectRepository.CreateObjectDataset(objectDatasetDto, accessToken);
            if (objDataset == null)
                return Ok(new ApiResponse<ObjectDatasetDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during dataset creation." },
                    Data = null
                });

            var objDatasetList = new List<ObjectDatasetDto>() { objDataset };
            return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = objDatasetList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = objDatasetList
            });
        }

        [HttpPut("data-objects/{sd_oid}/datasets/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
        public async Task<IActionResult> UpdateObjectDataset(string sd_oid, int id, [FromBody] ObjectDatasetDto objectDatasetDto)
        {
            objectDatasetDto.Id ??= id;
            objectDatasetDto.sd_oid ??= sd_oid;
            
            var dataObj = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObj == null) return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objDataset = await _dataObjectRepository.GetObjectDataset(id);
            if (objDataset == null) return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object datasets have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedObjDataset = await _dataObjectRepository.UpdateObjectDataset(objectDatasetDto, accessToken);
            if (updatedObjDataset == null)
                return Ok(new ApiResponse<ObjectDatasetDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during dataset update." },
                    Data = null
                });

            var datasetList = new List<ObjectDatasetDto>() { updatedObjDataset };
            return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = datasetList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = datasetList
            });
        }
        
        [HttpDelete("data-objects/{sd_oid}/datasets/{id:int}")]
        [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
        public async Task<IActionResult> DeleteObjectDataset(string sd_oid, int id)
        {
            var dataObject = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });

            var objectDataset = await _dataObjectRepository.GetObjectDataset(id);
            if (objectDataset == null) return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object datasets have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteObjectDataset(id);
            return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Object dataset has been removed." },
                Data = null
            });
        }

        [HttpDelete("data-objects/{sd_oid}/datasets")]
        [SwaggerOperation(Tags = new []{"Object datasets endpoint"})]
        public async Task<IActionResult> DeleteAllObjectDatasets(string sd_oid)
        {
            var dataObject = await _dataObjectRepository.GetObjectById(sd_oid);
            if (dataObject == null) return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data objects have been found." },
                Data = null
            });
            
            var count = await _dataObjectRepository.DeleteAllObjectDatasets(sd_oid);
            return Ok(new ApiResponse<ObjectDatasetDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All object datasets have been removed." },
                Data = null
            });
        }
        
    }
}