using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MdmService.Contracts.Responses;
using MdmService.DTO.Study;
using MdmService.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.Contracts;

namespace rmsbe.Controllers
{
    public class StudyTitlesApiController : BaseApiController
    {
        private readonly IStudyRepository _studyRepository;

        public StudyTitlesApiController(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
        }
        
        [HttpGet("studies/{sd_sid}/titles")]
        [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
        public async Task<IActionResult> GetStudyTitles(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyTitles = await _studyRepository.GetStudyTitles(sd_sid);
            if (studyTitles == null)
                return Ok(new ApiResponse<StudyTitleDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No study titles have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = studyTitles.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyTitles
            });
        }
        
        [HttpGet("studies/{sd_sid}/titles/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
        public async Task<IActionResult> GetStudyTitle(string sd_sid, int id)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyTitle = await _studyRepository.GetStudyTitle(id);
            if (studyTitle == null) return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study titles have been found." },
                Data = null
            });

            var studyTitleList = new List<StudyTitleDto>() { studyTitle };
            return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = studyTitleList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyTitleList
            });
        }

        [HttpPost("studies/{sd_sid}/titles")]
        [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
        public async Task<IActionResult> CreateStudyTitle(string sd_sid, [FromBody] StudyTitleDto studyTitleDto)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            studyTitleDto.sd_sid ??= sd_sid;

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();
            
            var studyTitle = await _studyRepository.CreateStudyTitle(studyTitleDto, accessToken);
            if (studyTitle == null) return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during study title creation." },
                Data = null
            });

            var studyTitleList = new List<StudyTitleDto>() { studyTitle };
            return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = studyTitleList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyTitleList
            });
        }

        [HttpPut("studies/{sd_sid}/titles/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
        public async Task<IActionResult> UpdateStudyTitle(string sd_sid, int id, [FromBody] StudyTitleDto studyTitleDto)
        {
            studyTitleDto.Id ??= id;
            studyTitleDto.sd_sid ??= sd_sid;
            
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyTitle = await _studyRepository.GetStudyTitle(id);
            if (studyTitle == null) return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study titles have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedStudyTitle = await _studyRepository.UpdateStudyTitle(studyTitleDto, accessToken);
            if (updatedStudyTitle == null)
                return Ok(new ApiResponse<StudyTitleDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study title update." },
                    Data = null
                });

            var studyTitleList = new List<StudyTitleDto>() { updatedStudyTitle };
            return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = studyTitleList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyTitleList
            });
        }

        [HttpDelete("studies/{sd_sid}/titles/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
        public async Task<IActionResult> DeleteStudyTitle(string sd_sid, int id)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyTitle = await _studyRepository.GetStudyTitle(id);
            if (studyTitle == null) return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study titles have been found." },
                Data = null
            });
            
            var count = await _studyRepository.DeleteStudyTitle(id);
            return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Study title has been removed." },
                Data = null
            });
        }

        [HttpDelete("studies/{sd_sid}/titles")]
        [SwaggerOperation(Tags = new []{"Study titles endpoint"})]
        public async Task<IActionResult> DeleteAllStudyTitles(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            
            var count = await _studyRepository.DeleteAllStudyTitles(sd_sid);
            return Ok(new ApiResponse<StudyTitleDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All study titles have been removed." },
                Data = null
            });
        }
        
    }
}