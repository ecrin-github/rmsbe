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
    public class StudyReferencesApiController : BaseApiController
    {
        
        private readonly IStudyRepository _studyRepository;

        public StudyReferencesApiController(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
        }
        
        [HttpGet("studies/{sd_sid}/references")]
        [SwaggerOperation(Tags = new []{"Study references endpoint"})]
        public async Task<IActionResult> GetStudyReferences(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyRefs = await _studyRepository.GetStudyReferences(sd_sid);
            if (studyRefs == null)
                return Ok(new ApiResponse<StudyReferenceDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No study references have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = studyRefs.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyRefs
            });
        }
        
        [HttpGet("studies/{sd_sid}/references/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study references endpoint"})]
        public async Task<IActionResult> GetStudyReferences(string sd_sid, int id)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyRef = await _studyRepository.GetStudyReference(id);
            if (studyRef == null) return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study references have been found." },
                Data = null
            });

            var studyRefList = new List<StudyReferenceDto>() { studyRef };
            return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = studyRefList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyRefList
            });
        }

        [HttpPost("studies/{sd_sid}/references")]
        [SwaggerOperation(Tags = new []{"Study references endpoint"})]
        public async Task<IActionResult> CreateStudyReference(string sd_sid,
            [FromBody] StudyReferenceDto studyReferenceDto)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            studyReferenceDto.sd_sid ??= sd_sid;

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();
            
            var studyRef = await _studyRepository.CreateStudyReference(studyReferenceDto, accessToken);
            if (studyRef == null)
                return Ok(new ApiResponse<StudyReferenceDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study reference creation." },
                    Data = null
                });

            var studyRefList = new List<StudyReferenceDto>() { studyRef };
            return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = studyRefList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyRefList
            });
        }

        [HttpPut("studies/{sd_sid}/references/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study references endpoint"})]
        public async Task<IActionResult> UpdateStudyReference(string sd_sid, int id, [FromBody] StudyReferenceDto studyReferenceDto)
        {
            studyReferenceDto.Id ??= id;
            studyReferenceDto.sd_sid ??= sd_sid;
            
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            var studyRef = await _studyRepository.GetStudyReference(id);
            if (studyRef == null) return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study references have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedStudyRef = await _studyRepository.UpdateStudyReference(studyReferenceDto, accessToken);
            if (updatedStudyRef == null)
                return Ok(new ApiResponse<StudyReferenceDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study reference update." },
                    Data = null
                });

            var studyRefList = new List<StudyReferenceDto>() { updatedStudyRef };
            return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = studyRefList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyRefList
            });
        }

        [HttpDelete("studies/{sd_sid}/references/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study references endpoint"})]
        public async Task<IActionResult> DeleteStudyReference(string sd_sid, int id)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyRef = await _studyRepository.GetStudyReference(id);
            if (studyRef == null) return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study references have been found." },
                Data = null
            });
            
            var count = await _studyRepository.DeleteStudyReference(id);
            return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Study reference has been removed." },
                Data = null
            });
        }

        [HttpDelete("studies/{sd_sid}/references")]
        [SwaggerOperation(Tags = new []{"Study references endpoint"})]
        public async Task<IActionResult> DeleteAllStudyReferences(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            
            var count = await _studyRepository.DeleteAllStudyReferences(sd_sid);
            return Ok(new ApiResponse<StudyReferenceDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All study references have been removed." },
                Data = null
            });
        }
        
    }
}