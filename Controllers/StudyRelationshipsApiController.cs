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
    public class StudyRelationshipsApiController : BaseApiController
    {
        
        private readonly IStudyRepository _studyRepository;

        public StudyRelationshipsApiController(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
        }
        
        [HttpGet("studies/{sd_sid}/relationships")]
        [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
        public async Task<IActionResult> GetStudyRelationships(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyRelationships = await _studyRepository.GetStudyRelationships(sd_sid);
            if (studyRelationships == null)
                return Ok(new ApiResponse<StudyRelationshipDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No study relationships have been found." },
                    Data = null
                });
            
            return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = studyRelationships.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyRelationships
            });
        }
        
        [HttpGet("studies/{sd_sid}/relationships/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
        public async Task<IActionResult> GetStudyRelationship(string sd_sid, int id)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyRel = await _studyRepository.GetStudyRelationship(id);
            if (studyRel == null) return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study relationships have been found." },
                Data = null
            });

            var studyRelList = new List<StudyRelationshipDto>() { studyRel };
            return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = studyRelList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyRelList
            });
        }

        [HttpPost("studies/{sd_sid}/relationships")]
        [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
        public async Task<IActionResult> CreateStudyRelationship(string sd_sid,
            [FromBody] StudyRelationshipDto studyRelationshipDto)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            studyRelationshipDto.sd_sid ??= sd_sid;

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var studyRel = await _studyRepository.CreateStudyRelationship(studyRelationshipDto, accessToken);
            if (studyRel == null)
                return Ok(new ApiResponse<StudyRelationshipDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study relationship creation." },
                    Data = null
                });

            var studyRelList = new List<StudyRelationshipDto>() { studyRel };
            return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = studyRelList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyRelList
            });
        }

        [HttpPut("studies/{sd_sid}/relationships/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
        public async Task<IActionResult> UpdateStudyRelationship(string sd_sid, int id, [FromBody] StudyRelationshipDto studyRelationshipDto)
        {
            studyRelationshipDto.Id ??= id;
            studyRelationshipDto.sd_sid ??= sd_sid;
            
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyRel = await _studyRepository.GetStudyRelationship(id);
            if (studyRel == null) return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study relationships have been found." },
                Data = null
            });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedStudyRel = await _studyRepository.UpdateStudyRelationship(studyRelationshipDto, accessToken);
            if (updatedStudyRel == null)
                return Ok(new ApiResponse<StudyRelationshipDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study relationship update." },
                    Data = null
                });

            var studyRelList = new List<StudyRelationshipDto>() { updatedStudyRel };
            return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = studyRelList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyRelList
            });
        }

        [HttpDelete("studies/{sd_sid}/relationships/{id:int}")]
        [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
        public async Task<IActionResult> DeleteStudyRelationship(string sd_sid, int id)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyRel = await _studyRepository.GetStudyRelationship(id);
            if (studyRel == null) return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study relationships have been found." },
                Data = null
            });
            
            var count = await _studyRepository.DeleteStudyRelationship(id);
            return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Study relationship has been removed." },
                Data = null
            });
        }

        [HttpDelete("studies/{sd_sid}/relationships")]
        [SwaggerOperation(Tags = new []{"Study relationships endpoint"})]
        public async Task<IActionResult> DeleteAllStudyRelationships(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var count = await _studyRepository.DeleteAllStudyRelationships(sd_sid);
            return Ok(new ApiResponse<StudyRelationshipDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "All study relationships have been removed." },
                Data = null
            });
        }

    }
}