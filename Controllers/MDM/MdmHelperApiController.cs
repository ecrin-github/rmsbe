using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc;
using rmsbe.DataLayer.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class MDMStatisticsApiController : BaseApiController
{
    /*
    private readonly IObjectRepository _objectRepository;
    private readonly IStudyRepository _studyRepository;

    public MDMStatisticsApiController(
        IObjectRepository objectRepository,
        IStudyRepository studyRepository)
    {
        _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
    }

    [HttpGet("statistics/studies/statistics")]
    [SwaggerOperation(Tags = new []{"Statistics"})]
    public async Task<IActionResult> GetTotalStudies()
    {
        return Ok(new StatisticsResponse()
        {
            Total = await _studyRepository.GetTotalStudies()
        });    
    }

    [HttpGet("statistics/data-objects/statistics")]
    [SwaggerOperation(Tags = new []{"Statistics"})]
    public async Task<IActionResult> GetTotalDataObjects()
    {
        return Ok(new StatisticsResponse()
        {
            Total = await _objectRepository.GetTotalDataObjects()
        });    
    }
    

    public FilteringApiController(
        IObjectRepository objectRepository,
        IStudyRepository studyRepository)
    {
        _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
    }


    [HttpPost("pagination/studies")]
    [SwaggerOperation(Tags = new []{"Pagination"})]
    public async Task<IActionResult> PaginateStudies(PaginationRequest paginationRequest)
    {
        var data = await _studyRepository.PaginateStudies(paginationRequest);
        if (data.Total == 0) return Ok(new ApiResponse<Study>
        {
            Total = 0,
            Data = null,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No studies have been found."}
        });
        return Ok(new ApiResponse<Study>
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
        if (data.Total == 0) return Ok(new ApiResponse<DataObject>
        {
            Total = 0,
            Data = null,
            Page = paginationRequest.Page,
            Size = paginationRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No data objects have been found."}
        });
        return Ok(new ApiResponse<DataObject>
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
        if (data.Total == 0) return Ok(new ApiResponse<Study>
        {
            Total = 0,
            Data = null,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No studies have been found."}
        });
        return Ok(new ApiResponse<Study>
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
        if (data.Total == 0) return Ok(new ApiResponse<DataObject>
        {
            Total = 0,
            Data = null,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = NotFound().StatusCode,
            Messages = new List<string>() {"No data objects have been found."}
        });
        return Ok(new ApiResponse<DataObject>
        {
            Total = data.Total,
            Data = data.Data,
            Page = filteringByTitleRequest.Page,
            Size = filteringByTitleRequest.Size,
            StatusCode = Ok().StatusCode,
            Messages = null
        });
    }
    */
}
