using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rmsbe.Contracts;
using RmsService.Contracts.Responses;
using RmsService.DTO;
using RmsService.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace rmsbe.Controllers
{
    public class DtpObjectsApiController : BaseApiController
    {
        
        private readonly IDtpRepository _dtpRepository;

        public DtpObjectsApiController(IDtpRepository dtpRepository)
        {
            _dtpRepository = dtpRepository ?? throw new ArgumentNullException(nameof(dtpRepository));
        }
        
        
        [HttpGet("data-transfers/{dtp_id:int}/objects")]
        [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
        public async Task<IActionResult> GetDtpObjectList(int dtp_id)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });

            var dtpObjects = await _dtpRepository.GetAllDtpObjects(dtp_id);
            if (dtpObjects == null) return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP objects have been found."},
                Data = null
            });
            
            return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = dtpObjects.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpObjects
            });
        }

        [HttpGet("data-transfers/{dtp_id:int}/objects/{id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
        public async Task<IActionResult> GetDtpObject(int dtp_id, int id)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });

            var dtpObj = await _dtpRepository.GetDtpObject(id);
            if (dtpObj == null) return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP object has been found."},
                Data = null
            });
            
            var dtpObjectList = new List<DtpObjectDto>() { dtpObj };
            return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = dtpObjectList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpObjectList
            });
        }

        [HttpPost("data-transfers/{dtp_id:int}/objects")]
        [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
        public async Task<IActionResult> CreateDtpObject(int dtp_id, [FromBody] DtpObjectDto dtpObjectDto)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });

            var dtpObj = await _dtpRepository.CreateDtpObject(dtp_id, dtpObjectDto.object_id, dtpObjectDto);
            if (dtpObj == null)
                return Ok(new ApiResponse<DtpObjectDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during DTP object creation." },
                    Data = null
                });

            var dtpObjList = new List<DtpObjectDto>() { dtpObj };
            return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = dtpObjList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpObjList
            });
        }

        [HttpPut("data-transfers/{dtp_id:int}/objects/{id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
        public async Task<IActionResult> UpdateDtpObject(int dtp_id, int id, [FromBody] DtpObjectDto dtpObjectDto)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });
            
            var dtpObj = await _dtpRepository.GetDtpObject(id);
            if (dtpObj == null) return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP object has been found."},
                Data = null
            });

            var updatedDtpObj = await _dtpRepository.UpdateDtpObject(dtpObjectDto);
            if (updatedDtpObj == null)
                return Ok(new ApiResponse<DtpObjectDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during DTP object update." },
                    Data = null
                });

            var dtpObjectList = new List<DtpObjectDto>() { updatedDtpObj };
            return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = dtpObjectList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpObjectList
            });
        }

        [HttpDelete("data-transfers/{dtp_id:int}/objects/{id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
        public async Task<IActionResult> DeleteDtpObject(int dtp_id, int id)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });
            
            var dtpObj = await _dtpRepository.GetDtpObject(id);
            if (dtpObj == null) return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP object has been found."},
                Data = null
            });
            
            var count = await _dtpRepository.DeleteDtpObject(id);
            return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>(){"DTP object has been removed."},
                Data = null
            });
        }

        [HttpDelete("data-transfers/{dtp_id:int}/objects")]
        [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
        public async Task<IActionResult> DeleteAllDtpObjects(int dtp_id)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });
            
            var count = await _dtpRepository.DeleteAllDtpObjects(dtp_id);
            return Ok(new ApiResponse<DtpObjectDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>(){"All DTP objects have been removed."},
                Data = null
            });
        }
        
    }
}