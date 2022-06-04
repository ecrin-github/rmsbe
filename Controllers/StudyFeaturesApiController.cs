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
    public class StudyFeaturesApiController : BaseApiController
    {
        
        private readonly IStudyRepository _studyRepository;

        public StudyFeaturesApiController(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
        }

        [HttpGet("studies/{sd_sid}/features")]
        [SwaggerOperation(Tags = new[] { "Study features endpoint" })]
        public async Task<IActionResult> GetStudyFeatures(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            
            var studyFeatures = await _studyRepository.GetStudyFeatures(sd_sid);
            if (studyFeatures == null)
                return Ok(new ApiResponse<StudyFeatureDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No study features have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = studyFeatures.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyFeatures
            });
        }

        [HttpGet("studies/{sd_sid}/features/{id:int}")]
        [SwaggerOperation(Tags = new[] { "Study features endpoint" })]
        public async Task<IActionResult> GetStudyFeature(string sd_sid, int id)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyFeature = await _studyRepository.GetStudyFeature(id);
            if (studyFeature == null) return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study features have been found." },
                Data = null
            });

            var studyFeatureList = new List<StudyFeatureDto>() { studyFeature };
            return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = studyFeatureList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyFeatureList
            });
        }

        [HttpPost("studies/{sd_sid}/features")]
        [SwaggerOperation(Tags = new []{"Study features endpoint"})]
        public async Task<IActionResult> CreateStudyFeature(string sd_sid, [FromBody] StudyFeatureDto studyFeatureDto)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            studyFeatureDto.sd_sid ??= sd_sid;

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();
            
            var studyFeature = await _studyRepository.CreateStudyFeature(studyFeatureDto, accessToken);
            if (studyFeature == null)
                return Ok(new ApiResponse<StudyFeatureDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study feature creation." },
                    Data = null
                });

            var studyFeatureList = new List<StudyFeatureDto>() { studyFeature };
            return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = studyFeatureList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyFeatureList
            });
        }

        [HttpPut("studies/{sd_sid}/features/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study features endpoint"})]
        public async Task<IActionResult> UpdateStudyFeature(string sd_sid, int id, [FromBody] StudyFeatureDto studyFeatureDto)
        {
            studyFeatureDto.Id ??= id;
            studyFeatureDto.sd_sid ??= sd_sid;
            
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            
            var studyFeature = await _studyRepository.GetStudyFeature(id);
            if (studyFeature == null) return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study features have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedStudyFeature = await _studyRepository.UpdateStudyFeature(studyFeatureDto, accessToken);
            if (updatedStudyFeature == null)
                return Ok(new ApiResponse<StudyFeatureDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study feature update." },
                    Data = null
                });

            var studyFeatureList = new List<StudyFeatureDto>() { updatedStudyFeature };
            return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = studyFeatureList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyFeatureList
            });
        }

        [HttpDelete("studies/{sd_sid}/features/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study features endpoint"})]
        public async Task<IActionResult> DeleteStudyFeature(string sd_sid, int id)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            
            var studyFeature = await _studyRepository.GetStudyFeature(id);
            if (studyFeature == null) return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study features have been found." },
                Data = null
            });
            
            var count = await _studyRepository.DeleteStudyFeature(id);
            return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Study feature has been removed." },
                Data = null
            });
        }

        [HttpDelete("studies/{sd_sid}/features")]
        [SwaggerOperation(Tags = new []{"Study features endpoint"})]
        public async Task<IActionResult> DeleteAllStudyFeatures(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            
            var count = await _studyRepository.DeleteAllStudyFeatures(sd_sid);
            return Ok(new ApiResponse<StudyFeatureDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All study features have been removed." },
                Data = null
            });
        }
        
    }
}