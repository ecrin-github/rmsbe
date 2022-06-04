using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MdmService.DTO.Study;
using MdmService.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.Contracts;

namespace rmsbe.Controllers
{
    public class StudyTopicsApiController : BaseApiController
    {
        private readonly IStudyRepository _studyRepository;

        public StudyTopicsApiController(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
        }
        
        
        [HttpGet("studies/{sd_sid}/topics")]
        [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
        public async Task<IActionResult> GetStudyTopics(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyTopics = await _studyRepository.GetStudyTopics(sd_sid);
            if (studyTopics == null)
                return Ok(new ApiResponse<StudyTopicDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No study topics have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = studyTopics.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyTopics
            });
        }
        
        [HttpGet("studies/{sd_sid}/topics/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
        public async Task<IActionResult> GetStudyTopic(string sd_sid, int id)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyTopic = await _studyRepository.GetStudyTopic(id);
            if (studyTopic == null) return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study topics have been found." },
                Data = null
            });

            var studyTopicList = new List<StudyTopicDto>() { studyTopic };
            return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = studyTopicList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyTopicList
            });
        }

        [HttpPost("studies/{sd_sid}/topics")]
        [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
        public async Task<IActionResult> CreateStudyTopic(string sd_sid, [FromBody] StudyTopicDto studyTopicDto)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            studyTopicDto.sd_sid ??= sd_sid;

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();
            
            var studyTopic = await _studyRepository.CreateStudyTopic(studyTopicDto, accessToken);
            if (studyTopic == null) return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { "Error during study topic creation." },
                Data = null
            });

            var studyTopicList = new List<StudyTopicDto>() { studyTopic };
            return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = studyTopicList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyTopicList
            });
        }

        [HttpPut("studies/{sd_sid}/topics/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
        public async Task<IActionResult> UpdateStudyTopic(string sd_sid, int id, [FromBody] StudyTopicDto studyTopicDto)
        {
            studyTopicDto.Id ??= id;
            studyTopicDto.sd_sid ??= sd_sid;
            
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyTopic = await _studyRepository.GetStudyTopic(id);
            if (studyTopic == null) return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study topics have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedStudyTopic = await _studyRepository.UpdateStudyTopic(studyTopicDto, accessToken);
            if (updatedStudyTopic == null)
                return Ok(new ApiResponse<StudyTopicDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study topic update." },
                    Data = null
                });

            var studyTopicList = new List<StudyTopicDto>() { updatedStudyTopic };
            return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = studyTopicList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyTopicList
            });
        }

        [HttpDelete("studies/{sd_sid}/topics/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
        public async Task<IActionResult> DeleteStudyTopic(string sd_sid, int id)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyTopic = await _studyRepository.GetStudyTopic(id);
            if (studyTopic == null) return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study topics have been found." },
                Data = null
            });
            
            var count = await _studyRepository.DeleteStudyTopic(id);
            return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Study topic has been found." },
                Data = null
            });
        }

        [HttpDelete("studies/{sd_sid}/topics")]
        [SwaggerOperation(Tags = new []{"Study topics endpoint"})]
        public async Task<IActionResult> DeleteAllStudyTopics(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            
            var count = await _studyRepository.DeleteAllStudyTopics(sd_sid);
            return Ok(new ApiResponse<StudyTopicDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All study topics have been removed." },
                Data = null
            });
        }
    }
}