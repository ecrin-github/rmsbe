using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rmsbe.SysModels;
using RmsService.Contracts.Responses;
using RmsService.DTO;
using RmsService.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace rmsbe.Controllers
{
    public class DtaApiController : BaseApiController
    {
        
        private readonly IDtpRepository _dtpRepository;

        public DtaApiController(IDtpRepository dtpRepository)
        {
            _dtpRepository = dtpRepository ?? throw new ArgumentNullException(nameof(dtpRepository));
        }
         
        
        [HttpGet("data-transfers/{dtp_id:int}/accesses")]
        [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
        public async Task<IActionResult> GetDtaList(int dtp_id)
        {
            var dt = await _dtpRepository.GetDtp(dtp_id);
            if (dt == null) return Ok(new ApiResponse<DtaDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No related DTPs have been found."},
                Data = null
            });

            var data = await _dtpRepository.GetAllDta(dtp_id);
            
            return Ok(new ApiResponse<DtaDto>()
            {
                Total = data.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = data
            });
        }

        [HttpGet("data-transfers/{dtp_id:int}/accesses/{id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
        public async Task<IActionResult> GetDta(int dtp_id, int id)
        {
            var dt = await _dtpRepository.GetDtp(dtp_id);
            if (dt == null) return Ok(new ApiResponse<DtaDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No related DTPs have been found."},
                Data = null
            });

            var dta = await _dtpRepository.GetDta(id);
            if (dta == null) return Ok(new ApiResponse<DtaDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No related DTAs have been found."},
                Data = null
            });

            var dtaList = new List<DtaDto> { dta };

            return Ok(new ApiResponse<DtaDto>()
            {
                Total = dtaList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtaList
            });
        }

        [HttpPost("data-transfers/{dtp_id:int}/accesses")]
        [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
        public async Task<IActionResult> CreateDta(int dtp_id, [FromBody] DtaDto dtaDto)
        {
            var dt = await _dtpRepository.GetDtp(dtp_id);
            if (dt == null) return Ok(new ApiResponse<DtaDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No related DTPs have been found."},
                Data = null
            });

            var dta = await _dtpRepository.CreateDta(dtp_id, dtaDto);
            if (dta == null)
                return Ok(new ApiResponse<DtaDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during DTA creation." },
                    Data = null
                });

            var dtaList = new List<DtaDto>() { dta };
            
            return Ok(new ApiResponse<DtaDto>()
            {
                Total = dtaList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtaList
            });
        }

        [HttpPut("data-transfers/{dtp_id:int}/accesses/{id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
        public async Task<IActionResult> UpdateDta(int dtp_id, int id, [FromBody] DtaDto dtaDto)
        {
            var dt = await _dtpRepository.GetDtp(dtp_id);
            if (dt == null) return Ok(
                new ApiResponse<DtaDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>(){"No related DTPs have been found."},
                    Data = null
                });

            var dta = await _dtpRepository.GetDta(id);
            if (dta == null) return Ok(new ApiResponse<DtaDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No related DTAs have been found."},
                Data = null
            });

            var updatedDta = await _dtpRepository.UpdateDta(dtaDto);
            if (updatedDta == null) return Ok(new ApiResponse<DtaDto>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>(){"Error during DTA update."},
                Data = null
            });

            var dtaList = new List<DtaDto>() { updatedDta };
            
            return Ok(new ApiResponse<DtaDto>()
            {
                Total = dtaList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtaList
            });
        }

        [HttpDelete("data-transfers/{dtp_id:int}/accesses/{id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
        public async Task<IActionResult> DeleteDta(int dtp_id, int id)
        {
            var dt = await _dtpRepository.GetDtp(dtp_id);
            if (dt == null) return Ok(new ApiResponse<DtaDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No related DTA have been found."},
                Data = null
            });

            var dta = await _dtpRepository.GetDta(id);
            if (dta == null) return Ok(new ApiResponse<DtaDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No related DTA have been found."},
                Data = null
            });
            
            var count = await _dtpRepository.DeleteDta(id);
            return Ok(new ApiResponse<DtpDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>(){"DTA has been removed."},
                Data = null
            });
        }

        [HttpDelete("data-transfers/{dtp_id:int}/accesses")]
        [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
        public async Task<IActionResult> DeleteAllDta(int dtp_id)
        {
            var dt = await _dtpRepository.GetDtp(dtp_id);
            if (dt == null) return Ok(new ApiResponse<DtaDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No related DTA have been found."},
                Data = null
            });

            var count = await _dtpRepository.DeleteAllDta(dtp_id);
            return Ok(new ApiResponse<DtpDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>(){"All DTAs have been removed."},
                Data = null
            });
        }
        
    }
}