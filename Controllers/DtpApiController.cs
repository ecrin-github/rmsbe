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
    public class DtpApiController : BaseApiController
    {
     
        private readonly IDtpRepository _dtpRepository;

        public DtpApiController(IDtpRepository dtpRepository)
        {
            _dtpRepository = dtpRepository ?? throw new ArgumentNullException(nameof(dtpRepository));
        }
        
        
        [HttpGet("data-transfers/processes")]
        [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
        public async Task<IActionResult> GetDtpList()
        {
            var data = await _dtpRepository.GetAllDtp();
            if (data == null)
                return Ok(new ApiResponse<DtpDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>(){"No DTPs have been found."},
                    Data = null
                });
            return Ok(new ApiResponse<DtpDto>()
            {
                Total = data.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = data
            });
        }
        
        [HttpGet("data-transfers/processes/recent/{number:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
        public async Task<IActionResult> GetRecentDtp(int number)
        {
            var recentData = await _dtpRepository.GetRecentDtp(number);
            if (recentData == null) return Ok(new ApiResponse<DtpDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTPs have been found."},
                Data = null
            });
            return Ok(new ApiResponse<DtpDto>()
            {
                Total = recentData.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = recentData
            });
        }
        
        [HttpGet("data-transfers/processes/{dtp_id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
        public async Task<IActionResult> GetDtp(int dtp_id)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTPs have been found."},
                Data = null
            });

            var dtpList = new List<DtpDto>() { dtp };
            return Ok(new ApiResponse<DtpDto>()
            {
                Total = dtpList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpList
            });
        }

        [HttpPost("data-transfers/processes")]
        [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
        public async Task<IActionResult> CreateDtp([FromBody] DtpDto dtpDto)
        {
            var dtp = await _dtpRepository.CreateDtp(dtpDto);
            if (dtp == null)
                return Ok(new ApiResponse<DtpDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during DTP creation." },
                    Data = null
                });
            var dtpList = new List<DtpDto>() { dtp };
            return Ok(new ApiResponse<DtpDto>()
            {
                Total = dtpList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpList
            });
        }
        
        [HttpPut("data-transfers/processes/{dtp_id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
        public async Task<IActionResult> UpdateDtp(int dtp_id, [FromBody] DtpDto dtpDto)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTPs have been found."},
                Data = null
            });

            var updatedDtp = await _dtpRepository.UpdateDtp(dtpDto);
            if (updatedDtp == null)
                return Ok(new ApiResponse<DtpDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during DTP update." },
                    Data = null
                });

            var dtpList = new List<DtpDto>() { updatedDtp };
            return Ok(new ApiResponse<DtpDto>()
            {
                Total = dtpList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = dtpList
            });
        }

        [HttpDelete("data-transfers/processes/{dtp_id:int}")]
        [SwaggerOperation(Tags = new []{"Data transfer process endpoint"})]
        public async Task<IActionResult> DeleteDtp(int dtp_id)
        {
            var dtp = await _dtpRepository.GetDtp(dtp_id);
            if (dtp == null) return Ok(new ApiResponse<DtpDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>(){"No DTPs have been found."},
                Data = null
            });
            
            var count = await _dtpRepository.DeleteDtp(dtp_id);
            return Ok(new ApiResponse<DtpDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>(){"DTP has been removed."},
                Data = null
            });
        }
        
    }
}