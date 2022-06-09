using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class FilteringApiController : BaseApiController
{
    private readonly IDtpRepository _dtpRepository;
    private readonly IDupRepository _dupRepository;

    public FilteringApiController(
        IDtpRepository dtpRepository,
        IDupRepository dupRepository)
    {
        _dtpRepository = dtpRepository ?? throw new ArgumentNullException(nameof(dtpRepository));
        _dupRepository = dupRepository ?? throw new ArgumentNullException(nameof(dupRepository));
    }

    
    [HttpPost("pagination/dtp")]
    [SwaggerOperation(Tags = new []{"Pagination"})]
    public async Task<IActionResult> PaginateStudies(PaginationRequest paginationRequest)
    {
        var data = await _dtpRepository.PaginateDtp(paginationRequest);
        if (data.Total == 0) return Ok(new ApiResponse<DtpDto>
        {
            Total = 0,
            Data = null,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No DTPs have been found."}
        });
        return Ok(new ApiResponse<DtpDto>
        {
            Total = data.Total,
            Data = data.Data,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }

    [HttpPost("pagination/dup")]
    [SwaggerOperation(Tags = new []{"Pagination"})]
    public async Task<IActionResult> PaginateObjects(PaginationRequest paginationRequest)
    {
        var data = await _dupRepository.PaginateDup(paginationRequest);
        if (data.Total == 0) return Ok(new ApiResponse<DupDto>
        {
            Total = 0,
            Data = null,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No DUPs have been found."}
        });
        return Ok(new ApiResponse<DupDto>
        {
            Total = data.Total,
            Data = data.Data,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }

    [HttpPost("filter/dtp/by-title")]
    [SwaggerOperation(Tags = new []{"Filtering - by title"})]
    public async Task<IActionResult> FilterDtpByTitle(FilteringByTitleRequest filteringByTitleRequest)
    {
        var data = await _dtpRepository.FilterDtpByTitle(filteringByTitleRequest);
        if (data.Total == 0) return Ok(new ApiResponse<DtpDto>
        {
            Total = 0,
            Data = null,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No DTPs have been found."}
        });
        return Ok(new ApiResponse<DtpDto>
        {
            Total = data.Total,
            Data = data.Data,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }
    
    [HttpPost("filter/dup/by-title")]
    [SwaggerOperation(Tags = new []{"Filtering - by title"})]
    public async Task<IActionResult> FilterDupByTitle(FilteringByTitleRequest filteringByTitleRequest)
    {
        var data = await _dupRepository.FilterDupByTitle(filteringByTitleRequest);
        if (data.Total == 0) return Ok(new ApiResponse<DupDto>
        {
            Total = 0,
            Data = null,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No DUPs have been found."}
        });
        return Ok(new ApiResponse<DupDto>
        {
            Total = data.Total,
            Data = data.Data,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }


    private readonly IObjectRepository _objectRepository;
    private readonly IStudyRepository _studyRepository;
/*
    public FilteringApiController(
        IObjectRepository objectRepository,
        IStudyRepository studyRepository)
    {
        _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
    }
*/

    [HttpPost("pagination/studies")]
    [SwaggerOperation(Tags = new []{"Pagination"})]
    public async Task<IActionResult> PaginateStudies(PaginationRequest paginationRequest)
    {
        var data = await _studyRepository.PaginateStudies(paginationRequest);
        if (data.Total == 0) return Ok(new ApiResponse<StudyDto>
        {
            Total = 0,
            Data = null,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No studies have been found."}
        });
        return Ok(new ApiResponse<StudyDto>
        {
            Total = data.Total,
            Data = data.Data,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }

    [HttpPost("pagination/data-objects")]
    [SwaggerOperation(Tags = new []{"Pagination"})]
    public async Task<IActionResult> PaginateObjects(PaginationRequest paginationRequest)
    {
        var data = await _objectRepository.PaginateDataObjects(paginationRequest);
        if (data.Total == 0) return Ok(new ApiResponse<DataObjectDto>
        {
            Total = 0,
            Data = null,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No data objects have been found."}
        });
        return Ok(new ApiResponse<DataObjectDto>
        {
            Total = data.Total,
            Data = data.Data,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }

    [HttpPost("filter/studies/by-title")]
    [SwaggerOperation(Tags = new []{"Filtering - by title"})]
    public async Task<IActionResult> FilterStudiesByTitle(FilteringByTitleRequest filteringByTitleRequest)
    {
        var data = await _studyRepository.FilterStudiesByTitle(filteringByTitleRequest);
        if (data.Total == 0) return Ok(new ApiResponse<StudyDto>
        {
            Total = 0,
            Data = null,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No studies have been found."}
        });
        return Ok(new ApiResponse<StudyDto>
        {
            Total = data.Total,
            Data = data.Data,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }

    [HttpPost("filter/data-objects/by-title")]
    [SwaggerOperation(Tags = new []{"Filtering - by title"})]
    public async Task<IActionResult> FilterObjectsByTitle(FilteringByTitleRequest filteringByTitleRequest)
    {
        var data = await _objectRepository.FilterDataObjectsByTitle(filteringByTitleRequest);
        if (data.Total == 0) return Ok(new ApiResponse<DataObjectDto>
        {
            Total = 0,
            Data = null,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No data objects have been found."}
        });
        return Ok(new ApiResponse<DataObjectDto>
        {
            Total = data.Total,
            Data = data.Data,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }
}