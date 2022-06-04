using MdmService.DTO.Study;
using MdmService.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using rmsbe.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace rmsbe.Controllers;

public class study_identifiersApiController : BaseApiController
{
    private readonly IStudyRepository _studyRepository;

    public study_identifiersApiController(IStudyRepository studyRepository)
    {
        _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
    }


    [HttpGet("studies/{sd_sid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    public async Task<IActionResult> Getstudy_identifiers(string sd_sid)
    {
        var study = await _studyRepository.GetStudyById(sd_sid);
        if (study == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "No studies have been found." },
                Data = null
            });

        var study_idents = await _studyRepository.Getstudy_identifiers(sd_sid);
        if (study_idents == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "No study identifiers have been found." },
                Data = null
            });

        return Ok(new ApiResponse<study_identifierDto>
        {
            Total = study_idents.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = study_idents
        });
    }

    [HttpGet("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    public async Task<IActionResult> Getstudy_identifier(string sd_sid, int id)
    {
        var study = await _studyRepository.GetStudyById(sd_sid);
        if (study == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "No studies have been found." },
                Data = null
            });

        var study_ident = await _studyRepository.Getstudy_identifier(id);
        if (study_ident == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "No study features have been found." },
                Data = null
            });

        var study_identList = new List<study_identifierDto> { study_ident };
        return Ok(new ApiResponse<study_identifierDto>
        {
            Total = study_identList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = study_identList
        });
    }

    [HttpPost("studies/{sd_sid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    public async Task<IActionResult> Createstudy_identifier(string sd_sid,
        [FromBody] study_identifierDto study_identifierDto)
    {
        var study = await _studyRepository.GetStudyById(sd_sid);
        if (study == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "No studies have been found." },
                Data = null
            });

        study_identifierDto.sd_sid ??= sd_sid;

        var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
        var accessToken = accessTokenRes;

        var study_ident = await _studyRepository.Createstudy_identifier(study_identifierDto, accessToken);
        if (study_ident == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string> { "Error during study identifier creation." },
                Data = null
            });

        var study_identList = new List<study_identifierDto> { study_ident };
        return Ok(new ApiResponse<study_identifierDto>
        {
            Total = study_identList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = study_identList
        });
    }

    [HttpPut("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    public async Task<IActionResult> Updatestudy_identifier(string sd_sid, int id,
        [FromBody] study_identifierDto study_identifierDto)
    {
        study_identifierDto.Id ??= id;
        study_identifierDto.sd_sid ??= sd_sid;

        var study = await _studyRepository.GetStudyById(sd_sid);
        if (study == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "No studies have been found." },
                Data = null
            });

        var study_ident = await _studyRepository.Getstudy_identifier(id);
        if (study_ident == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "No study identifiers have been found." },
                Data = null
            });

        var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
        var accessToken = accessTokenRes;

        var updatedstudy_ident = await _studyRepository.Updatestudy_identifier(study_identifierDto, accessToken);
        if (updatedstudy_ident == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string> { "Error during study identifier update." },
                Data = null
            });

        var study_identList = new List<study_identifierDto> { updatedstudy_ident };
        return Ok(new ApiResponse<study_identifierDto>
        {
            Total = study_identList.Count,
            StatusCode = Ok().StatusCode,
            Messages = null,
            Data = study_identList
        });
    }

    [HttpDelete("studies/{sd_sid}/identifiers/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    public async Task<IActionResult> Deletestudy_identifier(string sd_sid, int id)
    {
        var study = await _studyRepository.GetStudyById(sd_sid);
        if (study == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "No studies have been found." },
                Data = null
            });

        var study_ident = await _studyRepository.Getstudy_identifier(id);
        if (study_ident == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "No study identifiers have been found." },
                Data = null
            });

        var count = await _studyRepository.Deletestudy_identifier(id);
        return Ok(new ApiResponse<study_identifierDto>
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string> { "Study identifier has been removed." },
            Data = null
        });
    }

    [HttpDelete("studies/{sd_sid}/identifiers")]
    [SwaggerOperation(Tags = new[] { "Study identifiers endpoint" })]
    public async Task<IActionResult> DeleteAllstudy_identifiers(string sd_sid)
    {
        var study = await _studyRepository.GetStudyById(sd_sid);
        if (study == null)
            return Ok(new ApiResponse<study_identifierDto>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string> { "No studies have been found." },
                Data = null
            });

        var count = await _studyRepository.DeleteAllstudy_identifiers(sd_sid);
        return Ok(new ApiResponse<study_identifierDto>
        {
            Total = count,
            StatusCode = Ok().StatusCode,
            Messages = new List<string> { "All study identifiers have been removed." },
            Data = null
        });
    }
}