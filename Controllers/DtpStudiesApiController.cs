using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rmsbe.Contracts;
using RmsService.Contracts.Responses;
using RmsService.DTO;
using RmsService.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace rmsbe.Controllers;

    public class DtpStudiesApiController : BaseApiController
    {
        
        private readonly IDtpRepository _dtpRepository;

        public DtpStudiesApiController(IDtpRepository dtpRepository)
        {
            _dtpRepository = dtpRepository ?? throw new ArgumentNullException(nameof(dtpRepository));
        }
        
        
        [HttpGet("data-transfers/{dtp_id:int}/studies")]
        [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
        public async Task<IActionResult> GetDtpStudyList(int dtp_id)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });
            var dtpStudies = await _dtpRepository.GetAllDtpStudies(dtp_id);
            if (dtpStudies == null)
                return Ok(new ApiResponse<DtpStudyDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No DTP studies have been found." },
                    Data = null
                });
            return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = dtpStudies.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpStudies
            });
        }

        [HttpGet("data-transfers/{dtp_id:int}/studies/{id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
        public async Task<IActionResult> GetDtpStudy(int dtp_id, int id)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });

            var dtpStudy = await _dtpRepository.GetDtpStudy(id);
            if (dtpStudy == null) return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP study has been found."},
                Data = null
            });
            var dtpStudyList = new List<DtpStudyDto>() { dtpStudy };
            return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = dtpStudyList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpStudyList
            });
        }

        [HttpPost("data-transfers/{dtp_id:int}/studies")]
        [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
        public async Task<IActionResult> CreateDtpStudy(int dtp_id, [FromBody] DtpStudyDto dtpStudyDto)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });

            var dtpStudy = await _dtpRepository.CreateDtpStudy(dtp_id, dtpStudyDto.study_id, dtpStudyDto);
            if (dtpStudy == null)
                return Ok(new ApiResponse<DtpStudyDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during DTP study creation." },
                    Data = null
                });
            var dtpStudyList = new List<DtpStudyDto>() { dtpStudy };
            return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = dtpStudyList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpStudyList
            });
        }

        [HttpPut("data-transfers/{dtp_id:int}/studies/{id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
        public async Task<IActionResult> UpdateDtpStudy(int dtp_id, int id, [FromBody] DtpStudyDto dtpStudyDto)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });
            
            var dtpStudy = await _dtpRepository.GetDtpStudy(id);
            if (dtpStudy == null) return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP study has been found."},
                Data = null
            });

            var updatedDtpStudy = await _dtpRepository.UpdateDtpStudy(dtpStudyDto);
            if (updatedDtpStudy == null)
                return Ok(new ApiResponse<DtpStudyDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during DTP study update." },
                    Data = null
                });
            var dtpStudyList = new List<DtpStudyDto>() { updatedDtpStudy };
            return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = dtpStudyList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpStudyList
            });
        }

        [HttpDelete("data-transfers/{dtp_id:int}/studies/{id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
        public async Task<IActionResult> DeleteDtpStudy(int dtp_id, int id)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });
            
            var dtpStudy = await _dtpRepository.GetDtpStudy(id);
            if (dtpStudy == null) return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP study has been found."},
                Data = null
            });
            
            var count = await _dtpRepository.DeleteDtpStudy(id);
            return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>(){"DTP study has been removed."},
                Data = null
            });
        }

        [HttpDelete("data-transfers/{dtp_id:int}/studies")]
        [SwaggerOperation(Tags = new []{"Data transfer process studies endpoint"})]
        public async Task<IActionResult> DeleteAllDtpStudies(int dtp_id)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTP has been found."},
                Data = null
            });
            
            var count = await _dtpRepository.DeleteAllDtpStudies(dtp_id);
            return Ok(new ApiResponse<DtpStudyDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>(){"All DTP studies have been found."},
                Data = null
            });
        }
        
    }
}